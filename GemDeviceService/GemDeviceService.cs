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

namespace GemDeviceService;
public class GemDeviceService
{
    private SecsGem? _secsGem;
    private HsmsConnection? _connector;
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

    public GemDeviceService(ISecsGemLogger logger, GemRepository gemReposiroty, SecsGemOptions secsGemOptions)
    {
        _logger = logger;
        Enable();
        RecieveMessageHandlerTask = Task.Factory.StartNew(
            async () =>
            {
                while (true)
                {
                    await foreach (var ReceiveSecsMsg in recvBuffer.Reader.ReadAllAsync())
                    {
                        //var ReceiveSecsMsg = await recvBuffer.Reader.ReadAsync();
                        try
                        {
                            
                            //Check Comm State
                            //Disable時什麼都不回應
                            //NotCommunicatig時,由Manager處理S1F13流程
                            //如果任何通訊發生失敗，應回到Not Communicating

                            //Check Ctrl State

                            //Format Validation, S9F7
                            if (SecsItemSchemaValidator.IsValid(ReceiveSecsMsg.PrimaryMessage) == false)
                            {
                                _ = ReceiveSecsMsg.TryReplyAsync();//不帶Item, 會給S9F7
                                await Task.Delay(10);
                                continue;
                            }
                            //Handle PrimaryMessage
                            switch (ReceiveSecsMsg.PrimaryMessage)
                            {
                                //S1F1 AreYouThere
                                case SecsMessage msg when (msg.S == 1 && msg.F == 1):

                                    //Invoke, Handle
                                    using (var rtnS1F2 = new SecsMessage(1, 2)
                                    {
                                        SecsItem = L(A("aaa"), A("bbb"))
                                    })
                                        await ReceiveSecsMsg.TryReplyAsync(rtnS1F2);
                                    break;
                                //S1F3 Selected Equipment Status Request
                                case SecsMessage msg when (msg.S == 1 && msg.F == 3):
                                    var vids = msg.SecsItem.Items.Select(item => item.FirstValue<int>());
                                    var svList = _GemRepo.GetSvListByVidList(vids);
                                    using (var rtnS2F4 = new SecsMessage(1, 4)
                                    {
                                        SecsItem = svList
                                    })
                                        await ReceiveSecsMsg.TryReplyAsync(rtnS2F4);
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
                                            await ReceiveSecsMsg.TryReplyAsync(rtnS1F12);
                                    }
                                    else
                                    {
                                        var svNameList = _GemRepo.GetSvNameListAll();
                                        using (var rtnS1F12 = new SecsMessage(1, 12)
                                        {
                                            SecsItem = svNameList
                                        })
                                            await ReceiveSecsMsg.TryReplyAsync(rtnS1F12);
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
                                        var rtn = await ReceiveSecsMsg.TryReplyAsync(rtnS1F14);
                                    }            
                                    break;
                                //S1F15 Request OFF-LINE
                                case SecsMessage msg when (msg.S == 1 && msg.F == 15):
                                    var result = _ctrlStateManager.HandleS1F15();
                                    using (var rtnMsg = new SecsMessage(1, 16)
                                    {
                                        SecsItem = B((byte)result)
                                    })
                                        ReceiveSecsMsg.TryReplyAsync(rtnMsg);
                                    break;
                                //S1F17 Request ON-LINE
                                case SecsMessage msg when (msg.S == 1 && msg.F == 17):
                                    result = _ctrlStateManager.HandleS1F17();
                                    using (var rtnMsg = new SecsMessage(1, 18)
                                    {
                                        SecsItem = B((byte)result)
                                    })
                                        ReceiveSecsMsg.TryReplyAsync(rtnMsg);
                                    break;
                                //S2F15 New Equipment Constant Send
                                case SecsMessage msg when (msg.S == 2 && msg.F == 15):
                                    var ecidecv = msg.SecsItem.Items
                                    .Select(item => (item.Items[0].FirstValue<int>(),
                                                      item.Items[1])).ToList();
                                    //Debug.WriteLine("")
                                    var rtnS2F15 = _GemRepo.SetECByIdLst(ecidecv);
                                    using (var rtnS2F16 = new SecsMessage(2, 16)
                                    {
                                        SecsItem = B((byte)rtnS2F15)
                                    })
                                        await ReceiveSecsMsg.TryReplyAsync(rtnS2F16);
                                    // 還要發個事件 ?
                                    break;
                                //S2F25 Loopback Diagnostic Request
                                case SecsMessage msg when (msg.S == 2 && msg.F == 25):
                                    using (var rtnS2F26 = new SecsMessage(2, 26)
                                    {
                                        SecsItem = msg.SecsItem
                                    })
                                        await ReceiveSecsMsg.TryReplyAsync(rtnS2F26);
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                        await Task.Delay(5);
                    }
                }
            });

        //_communicatinoState = CommunicationState.DISABLED;
        _GemRepo = gemReposiroty;
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

        var options = Options.Create(new SecsGemOptions
        {
            IsActive = true,
            IpAddress = "127.0.0.1",
            Port = 5000,
            //SocketReceiveBufferSize = 8096,
            SocketReceiveBufferSize = 1024,
            DeviceId = 0,
            LinkTestInterval = 1000*60,
            T6 = 5000
        });
        //var options = secsGemOptions;
        _connector = new HsmsConnection(options, _logger);
        _connector.LinkTestEnabled = false; //想解決莫名斷線
        _secsGem = new SecsGem(options, _connector, _logger);

        //狀態管理
        _commStateManager = new CommStateManager(_secsGem, false);
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

            OnConnectStatusChange?.Invoke(connectState.ToString());
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

    public Func<string, string>? RelayRemoteCmd;

    public event Action<string>? OnConnectStatusChange;
    public event Action<string, string>? OnCommStateChanged;
    public event Action<string, string>? OnControlStateChanged;

    public event Action<SecsMessage>? OnSecsMessageSend;
    public event Action OnProcessProgramChanged;
    public event Action<string> OnTerminalMessageReceived;

    /// <summary>
    /// for S2F41
    /// Input : ( RCMD, L( CPNAME, CPVAL ) ) , Output : ( HACK, L( CPNAME, CPVAL ) ) 
    /// HACK : 0 - ok, completed , 1 - invalid command , 2 - cannot do now , 3 - parameter error , 4 - initiated for asynchronous completion , 5 - rejected, already in desired condition , 6 - invalid object
    /// </summary>
    public Func<(string, List<(string, string)>),
                (int, List<(string, string)>)>? OnRemoteCommand;

    public ISecsGem? GetSecsWrapper => (_ctrlStateManager.CurrentState is ControlState.LOCAL or ControlState.REMOTE)
                                        ? _secsGem : null;
    //數值類
    public void GetVariableById(int VID) { }
    public void GetVariableByName(string name) { }
    public void UpdateSV(int VID) { }
    public void UpdateEC(int VID) { }

    //Report類
    public void SendTerminalMessage(string terminalMessage) { }
    public void SendEventReport(int eventId) {
        var reports = _GemRepo.GetReportByEventId(eventId);
        Random random = new Random();
        var dataId = random.Next();
        var s6f11 = new SecsMessage(6, 11)
        {
            SecsItem = L(
            U4((uint)dataId), //DATAID
            U4((uint)eventId), //CEID
            reports
            ),
        };
        _ = _secsGem.SendAsync(s6f11);//射後不理
    }
    public void SendAlarmReport(string alarmId) { }

    //需要補上CommState, CtrlState的限制,
    //大部分語句在進入On-Line後才可使用, 理論上只須限制在ON-line

    public int EnableComm()
    {
        return 0;
    }
    public int DisableComm()
    {
        return 0;
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
