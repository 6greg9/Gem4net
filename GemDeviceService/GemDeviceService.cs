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
                            case SecsMessage msg when (msg.S == 1 && msg.F == 13):
                                //var rtn = await _commStateManager.HandleHostInitCommReq(msg.SecsItem);
                                if (true)
                                {
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
                                    
                                }
                                else
                                {
                                    SecsMsg.TryReplyAsync();
                                }
                                break;
                            case SecsMessage msg when (msg.S == 1 && msg.F == 14):
                                break;
                            default:
                                break;
                        }
                        await Task.Delay(50);
                    }
                }
                catch(Exception ex)
                {

                }
                
            });

        //_communicatinoState = CommunicationState.DISABLED;
    }
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
            SocketReceiveBufferSize = 1024,
            DeviceId= 0,
            T6= 5000
        });

        _connector = new HsmsConnection(options, _logger);
        _secsGem = new SecsGem(options, _connector, _logger);
        _commStateManager = new CommStateManager(_secsGem,false);
        _connector.ConnectionChanged += async (sender,connectState)=>
        {
            if (connectState == ConnectionState.Selected)
            {
                _commStateManager.EnterCommunication();

            }
            else
            {
                _commStateManager.LeaveCommunication();
            }

            OnConnectStatusChange?.Invoke( connectState.ToString());
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
    #endregion
}
