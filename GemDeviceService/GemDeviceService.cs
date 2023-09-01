using Microsoft.Extensions.Options;
using Secs4Net;
using Secs4Net.Extensions;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading.Channels;

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

    public GemDeviceService(bool IsActive = true, string IP = "127.0.0.1", int Port = 5000,
        int SocketBufferSize = 65535, int DiviceId = 0, ISecsGemLogger logger = null)
    {
        _logger = logger;
        Enable();
        RecieveMessageHandlerTask = Task.Factory.StartNew(
            async () =>
            {
                try
                {
                    await foreach (var SecsMsg in recvBuffer.Reader.ReadAllAsync())
                    {

                        //Check Comm State
                        //Disable時什麼都不回應
                        //NotCommunicatig時,由Manager處理S1F13流程
                        //如果任何通訊發生失敗，應回到Not Communicating

                        //Check Ctrl State

                        //Format Validation, S9F7
                        if (SecsItemSchemaValidator.IsValid(SecsMsg.PrimaryMessage) == false)
                        {
                            _ = SecsMsg.TryReplyAsync();//不帶Item, 會給S9F7
                            await Task.Delay(10);
                            continue;
                        }
                        //Handle PrimaryMessage
                        switch (SecsMsg.PrimaryMessage)
                        {
                            //S1F1 AreYouThere
                            case SecsMessage msg when (msg.S == 1 && msg.F == 1):

                                //Invoke, Handle
                                var rtnMsg = new SecsMessage(1, 2)
                                {
                                    SecsItem = Item.L( Item.A("aaa"),Item.A("bbb"))
                                };

                                await SecsMsg.TryReplyAsync(rtnMsg);
                                break;
                            //S1F13 EstablishCommunicationsRequest, 要看是Host/Eqp Init
                            case SecsMessage msg when (msg.S == 1 && msg.F == 13):
                                //var rtn = await _commStateManager.HandleHostInitCommReq(msg.SecsItem);

                                rtnMsg = new SecsMessage(1, 14)
                                {
                                    SecsItem = Item.L(
                                        Item.B(0),
                                        Item.L(
                                            Item.A("MDLN"),
                                            Item.A("SOFTREV")
                                            ))
                                };

                                var rtn = await SecsMsg.TryReplyAsync(rtnMsg);
                                break;
                            default:
                                break;
                        }

                        await Task.Delay(30);
                    }
                }
                catch (Exception ex)
                {

                }

            });

        //_communicatinoState = CommunicationState.DISABLED;

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
            SocketReceiveBufferSize = 8096,
            DeviceId= 0,
            T6= 5000
        });

        _connector = new HsmsConnection(options, _logger);
        _secsGem = new SecsGem(options, _connector, _logger);

        //狀態管理
        _commStateManager = new CommStateManager(_secsGem, false);
        _ctrlStateManager = new CtrlStateManager(_secsGem);
        _commStateManager.NotifyCommStateChanged += (transition) =>
        {
            if (transition.currentState == CommunicationState.COMMUNICATING)
            {

            }
            //_ctrlStateManager.enterControl

        };

        _connector.ConnectionChanged += async (sender, connectState) =>
        {
            if (connectState == ConnectionState.Selected)
            {
                _commStateManager.EnterCommunicationState();

            }
            else
            {
                _commStateManager.LeaveCommunication();
            }

            OnConnectStatusChange?.Invoke(connectState.ToString());
        };
        //btnEnable.Enabled = false;
        _connector.LinkTestEnabled = true;
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

    public void ReadVariable(int VID) { }
    public void UpdateVariable(int VID) { }
    public void TriggerEvent(int ECID) { }
    public Action? OnRemoteCmd;
    public Action<string>? OnConnectStatusChange;
    public ISecsGem? GetSecsWrapper => (_ctrlStateManager.CurrentState == ControlState.LOCAL ||
                                        _ctrlStateManager.CurrentState == ControlState.REMOTE) ?
                                        _secsGem : null;
    //需要補上CommState, CtrlState的限制,
    //大部分語句在進入On-Line後才可使用, 理論上只須限制在ON-line

    #endregion
}
