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
using System.Threading;

namespace Gem4Net;
public partial class GemEqpService
{
    private SecsGem? _secsGem;
    private HsmsConnection? _connector;
    private CancellationTokenSource _hsmsCancellationTokenSource = new();
    public SecsGemOptions GemOptions { get; private set; }
    public GemEqpAppOptions EqpAppOptions { get; private set; }
    //之後要加GemServiceOptions

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
    public GemEqpService(ISecsGemLogger logger, GemRepository gemReposiroty,
        IOptions<SecsGemOptions> secsGemOptions, IOptions<GemEqpAppOptions> gemEqpAppOptions)
    {
        _logger = logger;
        GemOptions = secsGemOptions.Value;
        EqpAppOptions = gemEqpAppOptions.Value;
        _GemRepo = gemReposiroty;

        // ef core 第一次使用會花費很長時間
        //_ = Enable();

        RecieveMessageHandlerTask = Task.Run(async () =>
        {
            var token = SecsMsgHandlerTaskCTS.Token;
            while (token.IsCancellationRequested != true)
            {
                try
                {
                    await HandleRecievedSecsMessage(this.RecvBuffer,
                        new Action<PrimaryMessageWrapper>(async (msg) =>await HandlePrimaryMessage(msg)
                     ));
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
    public async Task Enable()
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
        _commStateManager = new CommStateManager(_secsGem, _logger, EqpAppOptions, _GemRepo);
        _ctrlStateManager = new CtrlStateManager(_secsGem, _logger, EqpAppOptions);
        _commStateManager.NotifyCommStateChanged += (transition) =>
        {

            if (transition.currentState == CommunicationState.COMMUNICATING)
            {
                _ctrlStateManager.EnterControlState(); //成功進入Communicating後, CtrlState開始
            }
            else
            {
                _traceDataManager.TraceTerminateAll();
            }


            OnCommStateChanged?.Invoke(transition.currentState.ToString(),
                transition.previousState.ToString());
        };
        _ctrlStateManager.NotifyCommStateChanged += (transition) =>
        {
            // 可能邏輯為需停止trace
            //if (transition.currentState is not ControlState.REMOTE or ControlState.LOCAL)
            //    _traceDataManager?.TraceTerminateAll();

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
            else if (_commStateManager.CurrentState != CommunicationState.DISABLED)
            {
                _commStateManager.LeaveCommunicationState();
            }
            if (connectState != ConnectionState.Selected)
            {
                _traceDataManager.TraceTerminateAll();
            }
            OnConnectStatusChanged?.Invoke(connectState.ToString());
        };
        //Trace
        _traceDataManager = new TraceDataManager(_secsGem, this, _GemRepo);



        //_connector.LinkTestEnabled = false;//想解決莫名斷線
        //hsmsCancellationTokenSource!.Cancel();
        _hsmsCancellationTokenSource = new();

        _connector.Start(_hsmsCancellationTokenSource.Token); // HSMS, 啟動

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
    public async Task Disable() // 各種cancel, dispose
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

    async Task HandleRecievedSecsMessage(Channel<PrimaryMessageWrapper> recvBuffer, Action<PrimaryMessageWrapper> handlePrimaryMessage)
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
                            A(EqpAppOptions.ModelType),
                            A(EqpAppOptions.SoftwareVersion)
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
                if (!(msg.S == 1 && msg.F == 13)
                 && !(msg.S == 1 && msg.F == 15)
                 && !(msg.S == 1 && msg.F == 17))
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
                    await ReceiveSecsMsg.TryReplyAsync(rtnMsg);
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
                    await ReceiveSecsMsg.TryReplyAsync(rtnMsg);
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



    ~GemEqpService()
    {
        _hsmsCancellationTokenSource?.Cancel();
        SecsMsgHandlerTaskCTS?.Cancel();


        if (_connector is not null)
        {
            _connector.Reconnect();
            _connector?.DisposeAsync()
                .ConfigureAwait(false).GetAwaiter().GetResult();
        }

        _secsGem?.Dispose();


    }
    /// <summary>
    /// Off-Line時候記得把背後在
    /// </summary>
    void HandleOffLine()
    {

    }
}
