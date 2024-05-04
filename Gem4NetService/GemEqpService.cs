using Microsoft.Extensions.Options;
using Secs4Net;
using Secs4Net.Extensions;
using Gem4NetRepository;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading.Channels;
using static Secs4Net.Item;
using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Gem4Net.Communication;
using Gem4Net.Control;
using Gem4NetRepository.Model;
using Gem4Net.TraceData;
using System.Reflection.Metadata.Ecma335;

namespace Gem4Net;
public partial class GemEqpService
{
    private SecsGem? _secsGem;
    private HsmsConnection? _connector;
    private CancellationTokenSource _hsmsCancellationTokenSource = new();
    public SecsGemOptions GemOptions { get; private set; }

    //之後要加GemServiceOptions
    public string MDLN { get; private set; } = "MDLN";//機台型號
    public string SOFTREV { get; private set; } = "SOFTREV";//軟體版本
    public bool IsCommHostInit { get; private set; }
    public int ClockFormatCode { get; private set; } = 1;

    private readonly ISecsGemLogger _logger;
    private readonly Channel<PrimaryMessageWrapper> RecvBuffer = Channel.CreateUnbounded<PrimaryMessageWrapper>(
        new UnboundedChannelOptions()
        {
            SingleWriter = false,
            SingleReader = false
        }
        );

    private CommStateManager _commStateManager;
    private CtrlStateManager _ctrlStateManager;

    private TraceDataManager _traceDataManager;

    private Task RecieveMessageHandlerTask;
    CancellationTokenSource SecsMsgHandlerTaskCTS = new();

    private GemRepository _GemRepo;

    public GemEqpService(ISecsGemLogger logger, GemRepository gemReposiroty, SecsGemOptions secsGemOptions, bool isCommHostInit = false)
    {
        _logger = logger;
        GemOptions = secsGemOptions;
        IsCommHostInit = isCommHostInit;
        _GemRepo = gemReposiroty;

        // ef core 第一次使用會花費很長時間
        MDLN = "MDLN";
        SOFTREV = "SOFTREV";

        Enable();

        RecieveMessageHandlerTask = Task.Run(() =>
        {
            var token = SecsMsgHandlerTaskCTS.Token;
            while (token.IsCancellationRequested != true)
            {
                try
                {
                    HandleRecievedSecsMessage(this.RecvBuffer, HandlePrimaryMessage);
                }
                catch (Exception ex)
                {
                    logger.Error("RecieveMessageHandler", ex);
                }
            }


        });
        //_communicatinoState = CommunicationState.DISABLED;
        //_GemRepo = ThreadSafeClassProxy.Create(gemReposiroty);

    }

    /// <summary>
    /// 啟動HSMS, 初始化GEM
    /// </summary>
    public async void Enable()
    {
        _secsGem?.Dispose();

        if (_connector is not null)
        {
            await _connector.DisposeAsync();
        }

        var options = Options.Create(GemOptions);
        //var options = secsGemOptions;
        _connector = new HsmsConnection(options, _logger);
        //_connector.LinkTestEnabled = false; 
        _secsGem = new SecsGem(options, _connector, _logger);

        //狀態管理
        _commStateManager = new CommStateManager(_secsGem, IsCommHostInit);
        _ctrlStateManager = new CtrlStateManager(_secsGem);
        _commStateManager.NotifyCommStateChanged += (transition) =>
        {
            if (transition.currentState == CommunicationState.COMMUNICATING)
            {
                _ctrlStateManager.EnterControlState(); //成功進入Communicating後, CtrlState開始
            }

            OnCommStateChanged?.Invoke(transition.currentState.ToString(),
                transition.previousState.ToString());
        };
        _ctrlStateManager.NotifyCommStateChanged += (transition) =>
        {
            OnControlStateChanged?.Invoke(transition.currentState.ToString(),
                transition.previousState.ToString());
            //SendEventReport(1);
        };

        _connector.ConnectionChanged += async (sender, connectState) =>
        {
            if (connectState == ConnectionState.Selected)
            {
                _commStateManager.EnterCommunicationState();
            }
            else
            {
                _commStateManager.LeaveCommunicationState();
            }

            OnConnectStatusChanged?.Invoke(connectState.ToString());
        };
        //Trace
        _traceDataManager = new TraceDataManager(_secsGem, this, _GemRepo);


        _connector.LinkTestEnabled = false;//想解決莫名斷線
        _ = _connector.StartAsync(_hsmsCancellationTokenSource.Token); // HSMS, 啟動

        try
        {
            await foreach (var primaryMessage in _secsGem.GetPrimaryMessageAsync(_hsmsCancellationTokenSource.Token))
            {
                await RecvBuffer.Writer.WriteAsync(primaryMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.Error("GetPrimaryMessageAsync", ex);
        }
    }
    public async void Disable() // 各種cancel, dispose
    {
        if (!_hsmsCancellationTokenSource.IsCancellationRequested)
        {
            _hsmsCancellationTokenSource.Cancel();
            _hsmsCancellationTokenSource.Dispose();
        }
        if (_connector is not null)
        {
            await _connector.DisposeAsync();
        }
        _secsGem?.Dispose();
        _hsmsCancellationTokenSource = new CancellationTokenSource();

        _secsGem = null;

        RecvBuffer.Reader.ReadAllAsync(); //清理 buffer
    }

    async void HandleRecievedSecsMessage(Channel<PrimaryMessageWrapper> recvBuffer, Action<PrimaryMessageWrapper> handlePrimaryMessage)
    {
        //await foreach (var ReceiveSecsMsg in recvBuffer.Reader.ReadAllAsync())
        // 會有神秘的處理延遲的抖動, 還是樸實一點...
        var ReceiveSecsMsg = await recvBuffer.Reader.ReadAsync();

        try
        {
            //Check Comm State
            //Disable時什麼都不回應
            //NotCommunicatig時,由Manager處理S1F13流程
            //如果任何通訊發生失敗，應回到Not Communicating
            var (s, f) = (ReceiveSecsMsg.PrimaryMessage.S, ReceiveSecsMsg.PrimaryMessage.F);

            //S1F13 Establish Communications Request
            if (s == 1 && f == 13)
            {
                int commAck = await _commStateManager.HandleS1F13(ReceiveSecsMsg.PrimaryMessage.SecsItem);
                using (var rtnS1F14 = new SecsMessage(1, 14)
                {
                    SecsItem = L(
                        B(0),
                        L(
                            A("MDLN"),
                            A("SOFTREV")
                            ))
                })
                {
                    var rtn = await ReceiveSecsMsg.TryReplyAsync(rtnS1F14);
                }
                return;

            }

            // Check Comm State
            if (_commStateManager.CurrentState is CommunicationState.DISABLED)
                return;
            if (_commStateManager.CurrentState is not CommunicationState.COMMUNICATING)
            { // Comm 有Host Trigger 和 Eqp Trigger
                var msg = ReceiveSecsMsg.PrimaryMessage;
                if (msg.S != 1 || msg.F != 13) //白名單方式
                    return;
            }
            //Check Ctrl State
            if (_ctrlStateManager.IsOnLine == false)
            {
                var msg = ReceiveSecsMsg.PrimaryMessage;
                if ((msg.S != 1 || msg.F != 13) || (msg.S != 1 || msg.F != 17))
                {
                    //sNf0
                    using var rtnSNF0 = new SecsMessage(msg.S, 0);
                    await ReceiveSecsMsg.TryReplyAsync(rtnSNF0);
                    return;
                }
            }

            //Format Validation, S9F7
            if (SecsItemSchemaValidator.IsValid(ReceiveSecsMsg.PrimaryMessage) == false)
            {
                _ = ReceiveSecsMsg.TryReplyAsync();//不帶Item, 會給S9F7
                                                   //await Task.Delay(10);
                                                   //continue;
                return;
            }

            //S1F15 Request OFF-LINE
            if (s == 1 && f == 15)
            {
                var result = _ctrlStateManager.HandleS1F15();
                using (var rtnMsg = new SecsMessage(1, 16)
                {
                    SecsItem = B((byte)result)
                })
                    ReceiveSecsMsg.TryReplyAsync(rtnMsg);
                return;
            }

            //S1F17 Request ON-LINE
            if (s == 1 && f == 17)
            {
                var result = _ctrlStateManager.HandleS1F17();
                using (var rtnMsg = new SecsMessage(1, 18)
                {
                    SecsItem = B((byte)result)
                })
                    ReceiveSecsMsg.TryReplyAsync(rtnMsg);
                return;
            }
            //Handle PrimaryMessage
            handlePrimaryMessage(ReceiveSecsMsg);  //就在這裡一大包

        }
        catch (Exception ex)
        {
            _commStateManager.EnterCommunicationState(); //重新來過
            // S9 ?
            this._logger.Error(ex.ToString());
        }
    }
    /// <summary>
    /// 前面validate 會先擋住
    /// </summary>
    /// <param name="primaryMsgWrapper"></param>
    void HandlePrimaryMessage(PrimaryMessageWrapper primaryMsgWrapper)
    {
        switch (primaryMsgWrapper.PrimaryMessage)
        {
            case SecsMessage msg when (msg.S == 1):
                HandleStream1(primaryMsgWrapper);
                break;
            case SecsMessage msg when (msg.S == 2):
                HandleStream2(primaryMsgWrapper);
                break;
            case SecsMessage msg when (msg.S == 5):
                HandleStream5(primaryMsgWrapper);
                break;
            case SecsMessage msg when (msg.S == 6):
                HandleStream6(primaryMsgWrapper);
                break;
            case SecsMessage msg when (msg.S == 7):
                HandleStream7(primaryMsgWrapper);
                break;
            case SecsMessage msg when (msg.S == 10):
                HandleStream10(primaryMsgWrapper);
                break;
            default:
                break;
        }
    }
    async void HandleStream1(PrimaryMessageWrapper? primaryMsgWrapper)
    {
        switch (primaryMsgWrapper.PrimaryMessage)
        {
            //S1F1 AreYouThere
            case SecsMessage msg when (msg.S == 1 && msg.F == 1):

                //Invoke, Handle
                using (var rtnS1F2 = new SecsMessage(1, 2)
                {
                    SecsItem = L(A("aaa"), A("bbb"))
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS1F2);
                break;
            //S1F3 Selected Equipment Status Request
            case SecsMessage msg when (msg.S == 1 && msg.F == 3):
                var vids = msg.SecsItem.Items.Select(item => item.FirstValue<int>());
                Item? svList;
                if (vids is null || vids.Count() == 0)
                {
                    svList = _GemRepo.GetVariableAll();
                }
                else
                {
                    svList = _GemRepo.GetSvList(vids);
                }

                using (var rtnS2F4 = new SecsMessage(1, 4)
                {
                    SecsItem = svList
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F4);
                break;
            //S1F11 Selected Equipment Status Request
            case SecsMessage msg when (msg.S == 1 && msg.F == 11):
                vids = msg.SecsItem.Items.Select(item => item.FirstValue<int>());
                if (vids.Any())
                {
                    var svNameList = _GemRepo.GetSvNameList(vids);
                    using (var rtnS1F12 = new SecsMessage(1, 12)
                    {
                        SecsItem = svNameList
                    })
                        await primaryMsgWrapper.TryReplyAsync(rtnS1F12);
                }
                else
                {
                    var svNameList = _GemRepo.GetSvNameListAll();
                    using (var rtnS1F12 = new SecsMessage(1, 12)
                    {
                        SecsItem = svNameList
                    })
                        await primaryMsgWrapper.TryReplyAsync(rtnS1F12);
                }
                break;
            default:
                break;
        }
    }
    async void HandleStream2(PrimaryMessageWrapper? primaryMsgWrapper)
    {
        switch (primaryMsgWrapper.PrimaryMessage)
        {
            //S2F13 Equipment Constant Request
            case SecsMessage msg when (msg.S == 2 && msg.F == 13):
                var idCnt = msg.SecsItem?.Items.Length;
                if (idCnt is null or 0)
                {
                    var allEC = _GemRepo?.GetEcValueListAll();
                    using (var rtnMsg = new SecsMessage(2, 14)
                    {
                        SecsItem = allEC
                    })
                        primaryMsgWrapper?.TryReplyAsync(rtnMsg);
                    break;
                }
                // 查EC
                var ecids = msg.SecsItem?.Items
                        .Select(item => item.FirstValue<int>());
                var ecs = _GemRepo?.GetEcValueList(ecids);
                // 如果一個查不到就要回空L
                if (ecs.Items.Where(item => item.Format == SecsFormat.ASCII && item.GetString() == "").Count() > 0)
                {

                    using (var rtnMsg = new SecsMessage(2, 14)
                    {
                        SecsItem = L()
                    })
                        primaryMsgWrapper?.TryReplyAsync(rtnMsg);
                    break;
                }

                using (var rtnMsg = new SecsMessage(2, 14)
                {
                    SecsItem = ecs
                })
                    primaryMsgWrapper?.TryReplyAsync(rtnMsg);
                break;
            //S2F15 New Equipment Constant Send
            case SecsMessage msg when (msg.S == 2 && msg.F == 15):
                int rtnS2F15 = 0;
                var ecidecv = msg.SecsItem.Items
                .Select(item => (item.Items[0].FirstValue<int>(),
                                  item.Items[1])).ToList();
                rtnS2F15 = OnEcRecieved!.Invoke(ecidecv);
                if (rtnS2F15 == 0)
                    rtnS2F15 = _GemRepo.SetEcList(ecidecv);

                using (var rtnS2F16 = new SecsMessage(2, 16)
                {
                    SecsItem = B((byte)rtnS2F15)
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F16);
                // 還要發個事件 ?
                break;
            //S2F17 Date and Time Request
            case SecsMessage msg when (msg.S == 2 && msg.F == 17):
                Item Clock;
                if (ClockFormatCode == 0)
                    Clock = A(DateTime.Now.ToString("yyMMddHHmmss"));
                else if (ClockFormatCode == 1)
                    Clock = A(DateTime.Now.ToString("yyyyMMddHHmmssff"));
                else
                    Clock = A(DateTime.Now.ToString("yyyyMMddHHmmssff"));

                using (var rtnS2F18 = new SecsMessage(2, 18)
                {
                    SecsItem = Clock
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F18);

                break;
            //S2F23 Trace Initialize Send
            case SecsMessage msg when (msg.S == 2 && msg.F == 23):
                var trid = msg.SecsItem.Items[0].Format == SecsFormat.ASCII ?
                    msg.SecsItem.Items[0].ToString() : Convert.ToString(msg.SecsItem.Items[0].FirstValue<int>());
                //A:8 hhmmsscc
                var dsperStr = msg.SecsItem.Items[1].GetString();
                if (dsperStr.Length is not 8 and not 6)
                {
                    await primaryMsgWrapper.TryReplyAsync();
                    break;
                }
                var dsper = TimeSpan.FromHours(Convert.ToDouble(dsperStr.Substring(0, 2))) +
                            TimeSpan.FromMinutes(Convert.ToDouble(dsperStr.Substring(2, 2))) +
                            TimeSpan.FromSeconds(Convert.ToDouble(dsperStr.Substring(4, 2)));
                if (dsperStr.Length is 8)
                    dsper += TimeSpan.FromMilliseconds(Convert.ToDouble(dsperStr.Substring(6, 2)) * 100);
                var totsmp = msg.SecsItem.Items[2].FirstValue<int>();
                var repgsz = msg.SecsItem.Items[3].FirstValue<int>();
                var lstSv = msg.SecsItem.Items[4].Items.ToArray().Select(item => item.FirstValue<int>()).ToList();
                int tiaack = 0;
                if (lstSv.Count() == 0)
                {
                    tiaack = _traceDataManager.TraceTerminate(trid);
                }
                else
                {
                    tiaack = _traceDataManager.TraceInitialize((dsperStr, dsper, totsmp, repgsz, lstSv));
                }
                using (var rtnS2F24 = new SecsMessage(2, 24)
                {
                    SecsItem = B((byte)tiaack)
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F24);

                break;
            //S2F25 Loopback Diagnostic Request
            case SecsMessage msg when (msg.S == 2 && msg.F == 25):
                using (var rtnS2F26 = new SecsMessage(2, 26)
                {
                    SecsItem = msg.SecsItem
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F26);
                break;
            //S2F29 Equipment Constant Namelist Request
            case SecsMessage msg when (msg.S == 2 && msg.F == 29):
                var vidLst = msg.SecsItem.Items.Select( i=> i.FirstValue<int>()).ToList();  

                var s2F30 = _GemRepo.GetEcDetailList(vidLst);
                using (var rtnS2F29 = new SecsMessage(2, 30)
                {
                    SecsItem = s2F30
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F29);
                break;
            //S2F33 Define Report
            case SecsMessage msg when (msg.S == 2 && msg.F == 33):
                var reportDefines = msg.SecsItem[1].Items.Select((secsItem) =>
                {
                    var rptId = secsItem[0].FirstValue<int>();
                    var vids = secsItem[1].Items.Select(i => i.FirstValue<int>()).ToArray();
                    return (rptId, vids);
                }).ToList();
                var DRACK = _GemRepo.DefineReport(reportDefines);
                using (var rtnS2F34 = new SecsMessage(2, 34)
                {
                    SecsItem = B(Convert.ToByte(DRACK))
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F34);
                break;
            //S2F35 Link Event Report
            case SecsMessage msg when (msg.S == 2 && msg.F == 35):
                var linkEventReports = msg.SecsItem[1].Items.Select((secsItem) =>
                {
                    var rptId = secsItem[0].FirstValue<int>();
                    var vids = secsItem[1].Items.Select(i => i.FirstValue<int>()).ToArray();
                    return ((rptId, vids));
                });
                var LRACK = _GemRepo.LinkEvent(linkEventReports);
                using (var rtnS2F36 = new SecsMessage(2, 36)
                {
                    SecsItem = B(Convert.ToByte(LRACK))
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F36);
                break;
            //S2F37 Enable/Disable Event Report
            case SecsMessage msg when (msg.S == 2 && msg.F == 37):
                var Enable = msg.SecsItem[0].FirstValue<bool>();
                var lstECIDs = msg.SecsItem[1].Items.Select(i => i.FirstValue<int>()).ToList();
                var ERACK = _GemRepo.EnableEvent(Enable, lstECIDs);
                using (var rtnS2F38 = new SecsMessage(2, 38)
                {
                    SecsItem = B(Convert.ToByte(ERACK))
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F38);
                break;
            //S2F41 Host Command Send
            case SecsMessage msg when (msg.S == 2 && msg.F == 41):
                var RemoteCmd = new RemoteCommand();
                RemoteCmd.HCACK = -1;
                RemoteCmd.Name = msg.SecsItem.Items[0].GetString();
                foreach (var par in msg.SecsItem.Items[1].Items)
                {
                    RemoteCmd.Parameters.Add(
                        new CommandParameter { CPACK = -1, Name = par[0].ToString(), Value = par[1] });
                }

                var cmdResult = OnRemoteCommandReceived.Invoke(RemoteCmd);//交給應用程式惹
                var rtnSecsItem = msg.SecsItem;
                rtnSecsItem.Items[0] = B((byte)cmdResult.HCACK);

                int index = 0;
                foreach (var par in cmdResult.Parameters)
                {
                    rtnSecsItem.Items[1][index][1] = B((byte)par.CPACK);
                    index++;
                }

                using (var rtnS2F42 = new SecsMessage(2, 42)
                {
                    SecsItem = rtnSecsItem
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F42);
                break;
            default:
                break;
        }
    }
    async void HandleStream5(PrimaryMessageWrapper? primaryMsgWrapper)
    {
        switch (primaryMsgWrapper.PrimaryMessage)
        {
            //S5F3 Enable/Disable Alarm Send
            case SecsMessage msg when (msg.S == 5 && msg.F == 3):

                var aled = (int)msg.SecsItem.Items[0].FirstValue<byte>() >= 128 ? true : false;
                var alid = (int)msg.SecsItem.Items[1].FirstValue<int>();
                var ackc5 = _GemRepo.EnableAlarm(alid, aled); // 這個回應碼要再看清楚

                using (var rtnS10F4 = new SecsMessage(5, 4)
                {
                    SecsItem = B(Convert.ToByte(ackc5))
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS10F4);
                break;
            //S5F5 Enable/Disable Alarm Send
            case SecsMessage msg when (msg.S == 5 && msg.F == 5):
                var alarmIdVector = new List<uint>();

                IEnumerable<GemAlarm> alrmLst;
                if (alarmIdVector.Count == 0)
                {
                    alrmLst = _GemRepo.GetAlarmAll();
                }
                else
                {
                    alrmLst = _GemRepo.GetAlarm(alarmIdVector.Select(uint16 => (int)uint16));
                }
                var secsAlrmLst = alrmLst.Select(alrm =>
                {
                    if (alrm is null)
                        return L();
                    var alcd = alrm.ALCD;//? 128 : 0;
                    var alid = Convert.ToUInt32(alrm.ALID);
                    var altx = alrm.ALTX;
                    return L(B((byte)alcd), U4(alid), A(altx));

                }).ToArray();

                using (var rtnS5F6 = new SecsMessage(5, 6)
                {
                    SecsItem = L(secsAlrmLst)
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS5F6);
                break;
            //S5F7 Enable Alarm Send
            case SecsMessage msg when (msg.S == 5 && msg.F == 7):


                var secsEnabledAlrmLst = _GemRepo.GetAlarmAll()
                    .Where(alrm => alrm.ALED == true).Select(alrm =>
                {
                    if (alrm is null)
                        return L();
                    var alcd = alrm.ALCD;// ? 128 : 0;
                    var alid = Convert.ToUInt32(alrm.ALID);
                    var altx = alrm.ALTX;
                    return L(B((byte)alcd), U4(alid), A(altx));

                }).ToArray();

                using (var rtnS5F8 = new SecsMessage(5, 8)
                {
                    SecsItem = L(secsEnabledAlrmLst)
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS5F8);
                break;
            default:
                break;
        }
    }
    async void HandleStream6(PrimaryMessageWrapper? primaryMsgWrapper)
    {
        switch (primaryMsgWrapper.PrimaryMessage)
        {
            //S6F15 Event Report Request
            case SecsMessage msg when (msg.S == 6 && msg.F == 15):
                var eventId = msg.SecsItem[0].FirstValue<int>();
                var reports = _GemRepo.GetReportsByCeid(eventId);
                Random random = new Random();
                var dataId = random.Next();
                using (var rtnS6F16 = new SecsMessage(6, 16)
                {
                    SecsItem = L(
                    U4((uint)dataId), //DATAID
                    U4((uint)eventId), //CEID
                    reports
                    ),
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS6F16);

                break;
            //S6F19 Individual Report Request
            case SecsMessage msg when (msg.S == 6 && msg.F == 19):
                var rptId = msg.SecsItem.FirstValue<int>();
                var rptItem = _GemRepo.GetReportByRpid(rptId);
                using (var rtnS6F20 = new SecsMessage(6, 20)
                {
                    SecsItem = rptItem
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS6F20);
                break;
            default:
                break;
        }
    }
    async void HandleStream7(PrimaryMessageWrapper? primaryMsgWrapper)
    {
        switch (primaryMsgWrapper.PrimaryMessage)
        {
            //S7F17 Delete Process Program Send
            case SecsMessage msg when (msg.S == 7 && msg.F == 17):
                int ackc7 = -1;

                var ppids = msg.SecsItem.Items.Select(i => i.GetString()).ToList();
                ackc7 = OnProcessProgramDeleteReq.Invoke(ppids);
                

                using (var rtnS7F18 = new SecsMessage(7, 18)
                {
                    SecsItem = B((byte)ackc7)
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS7F18);
                break;
            //S7F19 Current Process Program Dir Request
            case SecsMessage msg when (msg.S == 7 && msg.F == 19):
                var ppArry = _GemRepo.GetFormattedPPAll().Select(pp => A(pp.PPID)).ToArray();
                var itemS7F20 = L(ppArry);
                using (var rtnS7F20 = new SecsMessage(7, 20)
                {
                    SecsItem = itemS7F20
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS7F20);
                break;
            //S7F23 Formatted Process Program Send
            case SecsMessage msg when (msg.S == 7 && msg.F == 23):
                var ACKC7 = OnFormattedProcessProgramReceived.Invoke(msg.SecsItem); //已經存在是要蓋過去?
                using (var rtnS7F24 = new SecsMessage(7, 24)
                {
                    SecsItem = B((byte)ACKC7)
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS7F24);
                break;
            //S7F25 Formatted Process Program Request
            case SecsMessage msg when (msg.S == 7 && msg.F == 25):

                var fpp = _GemRepo.GetFormattedProcessProgram(msg.SecsItem.GetString()).ToList();

                var pp = _GemRepo.FormattedProcessProgramToSecsItem(fpp.First());
                using (var rtnS7F26 = new SecsMessage(7, 26)
                {
                    SecsItem = pp
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS7F26);
                break;
            default:
                break;
        }
    }
    async void HandleStream10(PrimaryMessageWrapper? primaryMsgWrapper)
    {
        switch (primaryMsgWrapper.PrimaryMessage)
        {
            //S10F3 Terminal Display, Single
            case SecsMessage msg when (msg.S == 10 && msg.F == 3):

                var terminalText = msg.SecsItem.Items[1].GetString();
                var ackc10 = await Task.Run(() =>
                {
                    return OnTerminalMessageReceived?.Invoke(terminalText);
                });

                using (var rtnS10F4 = new SecsMessage(10, 4)
                {
                    SecsItem = B(Convert.ToByte(ackc10))
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS10F4);
                break;
            default:
                break;
        }
    }
}
