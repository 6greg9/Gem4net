using Secs4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemDeviceService;
internal class CommStateManager
{
    CommunicationState _currentState;
    public CommunicationState CurrentState { 
        get { return _currentState; }
        private set { 
            _currentState = value;
            NotifyCommStateChanged?.Invoke(_currentState);
        }}
    bool IsHostInitial = false;
    string MDLD = "MDLD";
    string SOFTREV = "SOFTREV";
    Task CommStateCheckTask;
    CancellationTokenSource CommStateCheckTaskCts;
    Task CommDelayTimerTask;
    CancellationTokenSource CommDelayTimerTaskCts;
    SecsGem _secsGem; //可能要給個介面
    public CommStateManager(SecsGem secsGem, bool isHostInit = false)
    {
        IsHostInitial= isHostInit;
        _secsGem = secsGem;
        CurrentState = CommunicationState.DISABLED;
        
    }
    public void EnterCommunication() {
        //要看IsHostInit給不同Action
        if (IsHostInitial == true)
        {
            CurrentState = CommunicationState.WAIT_CR_FROM_HOST;
            return;
        }
        CommStateCheckTaskCts= new CancellationTokenSource();
        var token = CommStateCheckTaskCts.Token;
        CommStateCheckTask = Task.Run(async () =>
        {
            
            var S1F13 = new SecsMessage(1,13)
            {
                SecsItem = Item.L( Item.A(MDLD),Item.A(SOFTREV))
            };
            var S1F14Waiter = _secsGem.SendAsync(S1F13);
            //要看IsHostInit, 而進入不同狀態
            CurrentState = CommunicationState.WAIT_CRA;
            while( !token.IsCancellationRequested && ! (CurrentState==CommunicationState.COMMUNICATING) )
            {
                switch (CurrentState)
                {
                    case CommunicationState.WAIT_CRA:
                        var S1F14 = await S1F14Waiter;
                        if(S1F14Waiter.IsCompleted)
                        {
                            //格式檢查
                            //Root
                            var Root = S1F14.SecsItem;
                            if (!(Root.Format == SecsFormat.List) || (Root.Count != 2))
                                break;
                            //First
                            //要看IsHostInit而有不同判斷
                            var FirstLevel = Root.Items;
                            if(FirstLevel.Count() != 2)
                            {
                                break;
                            }
                            var COMMACK = FirstLevel.Take(1).FirstOrDefault();
                            if(COMMACK == Item.B(1) ) //被拒絕
                            {
                                GotoWaitDelay();
                            }
                            else if ( COMMACK == Item.B(0) )//成功
                            {
                                CurrentState = CommunicationState.COMMUNICATING;
                            }

                        }
                        else
                        {
                            GotoWaitDelay();
                        }

                        
                        break;
                    case CommunicationState.WAIT_DELAY:
                        if( CommDelayTimerTask.IsCompleted == true)
                        {
                            GotoWaitCRA();
                        }
                        break;
                    
                    default: break;
                }
                await Task.Delay(50);
            }
            void GotoWaitCRA()
            {
                var S1F13 = new SecsMessage(1,13)
                {
                    SecsItem = Item.L( Item.A(MDLD),Item.A(SOFTREV))
                };
                S1F14Waiter = _secsGem.SendAsync(S1F13); //在while外部
                CurrentState = CommunicationState.WAIT_CRA;
            }
            void GotoWaitDelay()
            {
                CommDelayTimerTask = Task.Delay(1000);
                CurrentState = CommunicationState.WAIT_DELAY;
            }
        });
    }
    public void LeaveCommunication() {
        CommStateCheckTaskCts?.Cancel();
        CurrentState = CommunicationState.DISABLED;
    }
    public Task<int> HandleHostInitCommReq(Item secsItem)
    {
        return Task.Run(() =>
        {
            if (IsHostInitial == true)
            {
                CurrentState = CommunicationState.COMMUNICATING;
                return 0;
            }
            else
            {
                return 1;//不應該
            }
        });
        
    }
    public event Action<CommunicationState>? NotifyCommStateChanged;
}
