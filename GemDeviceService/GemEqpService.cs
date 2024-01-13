using Microsoft.Extensions.Options;
using Secs4Net;
using Secs4Net.Extensions;
using GemVarRepository;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading.Channels;
using static Secs4Net.Item;
using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace GemDeviceService;
public class GemEqpService
{
    private SecsGem? _secsGem;
    private HsmsConnection? _connector;
    public SecsGemOptions GemOptions { get; private set; }
    public bool IsCommHostInit { get; private set; }

    private readonly ISecsGemLogger _logger;
    private readonly Channel<PrimaryMessageWrapper> recvBuffer = Channel.CreateUnbounded<PrimaryMessageWrapper>(
        new UnboundedChannelOptions()
        {
            SingleWriter = false,
            SingleReader = false
        }
        );

    private CommStateManager _commStateManager;
    private CtrlStateManager _ctrlStateManager;

    private Task RecieveMessageHandlerTask;
    private CancellationTokenSource _cancellationTokenSource = new();

    private GemRepository _GemRepo;

    public GemEqpService(ISecsGemLogger logger, GemRepository gemReposiroty, SecsGemOptions secsGemOptions, bool isCommHostInit = false)
    {
        _logger = logger;
        GemOptions = secsGemOptions;
        IsCommHostInit = isCommHostInit;

        Enable();
        RecieveMessageHandlerTask = Task.Run(() =>
        {
            while (true)
            {
                HandleRecievedSecsMessage();
            }
        });

        //_communicatinoState = CommunicationState.DISABLED;
        _GemRepo = ThreadSafeClassProxy.Create(gemReposiroty);
    }
    async void HandleRecievedSecsMessage() // 需要加個CancelToken
    {
        //await foreach (var ReceiveSecsMsg in recvBuffer.Reader.ReadAllAsync())
        // 會有神秘的處理延遲的抖動, 還是樸實一點...
        var ReceiveSecsMsg = await recvBuffer.Reader.ReadAsync();
        //{
        //var ReceiveSecsMsg = await recvBuffer.Reader.ReadAsync();
        try
        {
            //Check Comm State
            //Disable時什麼都不回應
            //NotCommunicatig時,由Manager處理S1F13流程
            //如果任何通訊發生失敗，應回到Not Communicating
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
            //Handle PrimaryMessage
            HandlePrimaryMessage(ReceiveSecsMsg);


        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        //await Task.Delay(5);
        // }
    }
    async void HandlePrimaryMessage(PrimaryMessageWrapper? primaryMsgWrapper)
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
                var svList = _GemRepo.GetSvList(vids);
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
                }else{
                    var svNameList = _GemRepo.GetSvNameListAll();
                    using (var rtnS1F12 = new SecsMessage(1, 12)
                    {
                        SecsItem = svNameList
                    })
                        await primaryMsgWrapper.TryReplyAsync(rtnS1F12);
                }
                break;
            //S1F13 EstablishCommunicationsRequest, 要看是Host/Eqp Init
            case SecsMessage msg when (msg.S == 1 && msg.F == 13):
                //var rtn = await _commStateManager.HandleHostInitCommReq(msg.SecsItem);
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
                    var rtn = await primaryMsgWrapper.TryReplyAsync(rtnS1F14);
                }
                break;
            //S1F15 Request OFF-LINE
            case SecsMessage msg when (msg.S == 1 && msg.F == 15):
                var result = _ctrlStateManager.HandleS1F15();
                using (var rtnMsg = new SecsMessage(1, 16)
                {
                    SecsItem = B((byte)result)
                })
                    primaryMsgWrapper.TryReplyAsync(rtnMsg);
                break;
            //S1F17 Request ON-LINE
            case SecsMessage msg when (msg.S == 1 && msg.F == 17):
                result = _ctrlStateManager.HandleS1F17();
                using (var rtnMsg = new SecsMessage(1, 18)
                {
                    SecsItem = B((byte)result)
                })
                    primaryMsgWrapper?.TryReplyAsync(rtnMsg);
                break;
            //S2F13 Equipment Constant Request
            case SecsMessage msg when (msg.S == 2 && msg.F == 13):
                var idCnt = msg.SecsItem?.Items.Length;
                if (idCnt is null or 0)
                {
                    var allEC = _GemRepo?.GetEcNameListAll();
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
                var ecs = _GemRepo?.GetEcNameList(ecids);
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
                var ecidecv = msg.SecsItem.Items
                .Select(item => (item.Items[0].FirstValue<int>(),
                                  item.Items[1])).ToList();
                var rtnS2F15 = _GemRepo.SetEcList(ecidecv);
                using (var rtnS2F16 = new SecsMessage(2, 16)
                {
                    SecsItem = B((byte)rtnS2F15)
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F16);
                // 還要發個事件 ?
                break;
            //S2F25 Loopback Diagnostic Request
            case SecsMessage msg when (msg.S == 2 && msg.F == 25):
                using (var rtnS2F26 = new SecsMessage(2, 26)
                {
                    SecsItem = msg.SecsItem
                })
                    await primaryMsgWrapper.TryReplyAsync(rtnS2F26);
                break;
            //S2F33 Define Report
            case SecsMessage msg when (msg.S == 2 && msg.F == 33):
                var reportDefines = msg.SecsItem[1].Items.Select((secsItem) =>
                {
                    var rptId = secsItem[0].FirstValue<int>();
                    var vids = secsItem[1].Items.Select(i => i.FirstValue<int>()).ToArray();
                    return ((rptId, vids));
                });
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
        _connector.LinkTestEnabled = false; //想解決莫名斷線
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
        //btnEnable.Enabled = false;
        _connector.LinkTestEnabled = false;//想解決莫名斷線
        _ = _connector.StartAsync(_cancellationTokenSource.Token);
        //btnDisable.Enabled = true;
        //_communicatinoState = CommunicationState.WAIT_CRA;
        try
        {
            await foreach (var primaryMessage in _secsGem.GetPrimaryMessageAsync(_cancellationTokenSource.Token))
            {
                await recvBuffer.Writer.WriteAsync(primaryMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.Debug(ex.ToString());
        }
    }
    public async void Disable()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
        if (_connector is not null)
        {
            await _connector.DisposeAsync();
        }
        _secsGem?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();

        _secsGem = null;
        //btnEnable.Enabled = true;
        //btnDisable.Enabled = false;
        //lbStatus.Text = "Disable";
        //recvBuffer.Clear();
        recvBuffer.Reader.ReadAllAsync();
        //richTextBox1.Clear();
    }
    #region For App Interface

    public event Action<string>? OnConnectStatusChanged;
    public event Action<string, string>? OnCommStateChanged;
    public event Action<string, string>? OnControlStateChanged;

    public event Action<SecsMessage>? OnSecsMessageSend;
    public event Action? OnProcessProgramChanged;
    /// <summary>
    /// 0 - accepted for display , 1 - message will not be displayed , 2 - terminal not available
    /// </summary>
    public event Func<string, int>? OnTerminalMessageReceived;
    /// <summary>
    /// for S2F41
    /// Input : ( RCMD, L( CPNAME, CPVAL ) ) , Output : ( HACK, L( CPNAME, CPVAL ) ) 
    /// HACK : 0 - ok, completed , 1 - invalid command , 2 - cannot do now , 3 - parameter error , 4 - initiated for asynchronous completion , 5 - rejected, already in desired condition , 6 - invalid object
    /// </summary>
    public Func<(string, List<(string, string)>),
                (int, List<(string, string)>)>? OnRemoteCommand;

    public ISecsGem? GetSecsWrapper     // 不在ON-LINE沒有辦法使用
        => (_ctrlStateManager.CurrentState is ControlState.LOCAL or ControlState.REMOTE)
                                        ? _secsGem : null;
    //數值類
    public Item? GetVariableById(int VID)
    {
        return _GemRepo.GetSv(VID); //這樣還有要?
    }
    public void GetVariableByName(string name)
    {
        // 
    }
    public int UpdateSV(int VID, object varValue)
    {
        return _GemRepo.SetVarValue(VID, varValue);
    }
    public int UpdateEC(List<(int, Item)> idValList)
    {
        return _GemRepo.SetEcList(idValList);
    }

    //Report類
    public async Task<int> SendTerminalMessageAsync(string terminalMessage, int terminalId)
    {
        int ack10 = -1;
        try
        {
            using (var s10f1 = new SecsMessage(10, 1)
            {
                SecsItem = L(
                B((byte)terminalId),
                A(terminalMessage))
            })
            {
                var rtns10f2 = await _secsGem.SendAsync(s10f1);
                // 應先資料驗證
                ack10 = rtns10f2.SecsItem.FirstValue<byte>();
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
        finally
        {

        }
        return ack10;

    }
    public void SendEventReport(int eventId)
    {
        var reports = _GemRepo.GetReport(eventId);
        Random random = new Random();
        var dataId = random.Next();
        using var s6f11 = new SecsMessage(6, 11)
        {
            SecsItem = L(
            U4((uint)dataId), //DATAID
            U4((uint)eventId), //CEID
            reports
            ),
        };
        _ = _secsGem?.SendAsync(s6f11);//射後不理
    }
    public void SendAlarmReport(string alarmId) { }

    //需要補上CommState, CtrlState的限制,
    //大部分語句在進入On-Line後才可使用, 理論上只須限制在ON-line

    public int EnableComm()
    {
        return _commStateManager.EnableComm();
    }
    public int DisableComm()
    {
        return _commStateManager.DisableComm();
    }
    /// <summary>
    /// EQUIPMENT_OFF_LINE,HOST_OFF_LINE,ATTEMPT_ON_LINE,LOCAL,REMOTE
    /// </summary>
    /// <returns></returns>
    public ControlState GetCurrentCommState()
    {
        return _ctrlStateManager.CurrentState;
    }
    public int RequestOnline()
    {
        return _ctrlStateManager.OnLineRequest();
    }
    public int GoOffline()
    {
        return _ctrlStateManager.OffLine();
    }
    public int GoOnlineLocal()
    {
        return _ctrlStateManager.OnLineLocal();
    }
    public int GoOnlineRemote()
    {
        return _ctrlStateManager.OnLineRemote();
    }

    /// <summary>
    /// Equip的主動Command ?
    /// </summary>
    /// <returns></returns>
    public int Command()
    {
        return 0;
    }

    #endregion
}
