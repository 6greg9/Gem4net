using Gem4NetRepository;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using Secs4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4Net.Communication;
internal class CommStateManager
{

    ISecsGem _secsGem; //可能要給個介面
    ISecsGemLogger Logger;
    GemRepository GemRepo;
    public GemEqpAppOptions EqpAppOptions { get; private set; }
    public int EstablishCommunicationsTimeout { get
        {
            try
            {
                var vid = EqpAppOptions.EstablishCommunicationsTimeoutVID;
                if (vid is int VID)
                {
                    var secItem = GemRepo.GetVariable(VID).Result;
                    if (secItem is not null
                        && secItem.Format is SecsFormat.U2 ) // or SecsFormat.U1 or  SecsFormat.U4 or SecsFormat.U8)
                    {
                        var timeoutSec = secItem.FirstValue<UInt16>();
                        return  timeoutSec ;
                    }
                }
                return EqpAppOptions.EstablishCommunicationsTimeout ?? 20;

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return 20;
            }
        } }
    public CommStateManager(ISecsGem secsGem,ISecsGemLogger logger,
                GemEqpAppOptions eqpAppOptions, GemRepository gemRepo)
    {
        EqpAppOptions = eqpAppOptions;
        _secsGem = secsGem;
        Logger = logger;
        GemRepo = gemRepo;

        CurrentState = CommunicationState.DISABLED;

    }

    #region State, TransitionEvent
    CommunicationState _currentState;
    public CommunicationState CurrentState
    {
        get { return _currentState; }
        private set
        {
            PreviousState = _currentState;
            _currentState = value;
            if (CurrentState != PreviousState)
                NotifyCommStateChanged?.Invoke((_currentState, PreviousState));//別手動Ivoke狀態變化
        }
    }

    public CommunicationState PreviousState { get; private set; }
    public event Action<(CommunicationState currentState, CommunicationState previousState)>? NotifyCommStateChanged;
    #endregion

    #region State Machine

    Task CommStateCheckTask = Task.CompletedTask;
    CancellationTokenSource CommStateCheckTaskCts;
    Task CommDelayTimerTask;
    CancellationTokenSource CommDelayTimerTaskCts;

    public void EnterCommunicationState()
    {
        //要看IsHostInit給不同Action
        if (Convert.ToBoolean(EqpAppOptions.IsCommHostInit) == true)
        {
            CurrentState = CommunicationState.WAIT_CR_FROM_HOST;
            return;
        }
        if (CommStateCheckTask.Status == TaskStatus.Running)
            return;

        CommStateCheckTaskCts = new CancellationTokenSource();
        var token = CommStateCheckTaskCts.Token;
        CommStateCheckTask = Task.Run(async () =>
        {

            Task<SecsMessage> S1F14Waiter;
            //要看IsHostInit, 而進入不同狀態
            GotoWaitCRA();
            while (!token.IsCancellationRequested && !(CurrentState == CommunicationState.COMMUNICATING))
            {
                switch (CurrentState)
                {
                    case CommunicationState.WAIT_CRA:
                        await Task.WhenAny(S1F14Waiter, 
                            Task.Delay(TimeSpan.FromSeconds(EstablishCommunicationsTimeout)));


                        if (S1F14Waiter.IsCompleted)
                        {
                            var S1F14 = S1F14Waiter.Result;
                            //格式檢查
                            //Root
                            var Root = S1F14.SecsItem;
                            var result = SecsItemSchemaValidator.IsValid.Invoke(S1F14);
                            if(!result)
                                break;

                            
                            //First
                            //要看IsHostInit而有不同判斷
                            var FirstLevel = Root.Items;
                            if (FirstLevel.Count() != 2)
                            {
                                break;
                            }
                            var COMMACK = FirstLevel.Take(1).FirstOrDefault();
                            if (COMMACK == Item.B(1)) //被拒絕
                            {
                                GotoWaitDelay();
                            }
                            else if (COMMACK == Item.B(0))//成功
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
                        await CommDelayTimerTask;

                        GotoWaitCRA();

                        break;

                    default:
                        break; //WAIT_CRA_FROM_HOST 之類的
                }

                await Task.Delay(50);
            }
            void GotoWaitCRA()
            {
                //可能可以改用Func, 就不用注入_secsGem
                using (var S1F13 = new SecsMessage(1, 13)
                {
                    SecsItem = Item.L(Item.A(EqpAppOptions.ModelType), Item.A(EqpAppOptions.SoftwareVersion))
                })
                    S1F14Waiter = _secsGem.SendAsync(S1F13); //在while外部
                CurrentState = CommunicationState.WAIT_CRA;
            }
            void GotoWaitDelay()
            {
                CommDelayTimerTaskCts?.Cancel();
                CommDelayTimerTaskCts = new CancellationTokenSource();
                CommDelayTimerTask = Task.Delay(TimeSpan.FromSeconds(EstablishCommunicationsTimeout), CommDelayTimerTaskCts.Token);// Tooxx?
                CurrentState = CommunicationState.WAIT_DELAY;
            }
        });
    }
    public void LeaveCommunicationState()
    {
        DisableComm();
    }
    #endregion

    #region Action

    /// <summary>
    /// return commAck
    /// </summary>
    /// <param name="secsItem"></param>
    /// <returns></returns>
    public Task<int> HandleS1F13(Item secsItem)
    {
        if (CurrentState == CommunicationState.WAIT_CR_FROM_HOST)
        {
            CurrentState = CommunicationState.COMMUNICATING;
            return Task.FromResult(0);
        }
        return HandleHostInitCommReq(secsItem);
    }
    Task<int> HandleHostInitCommReq(Item secsItem)
    {
        return Task.Run(() =>
        {
            if (Convert.ToBoolean(EqpAppOptions.IsCommHostInit) == true)
            {
                CurrentState = CommunicationState.COMMUNICATING;
                return 0;
            }
            else
            {
                return 1;//模式不應該
            }
        });

    }

    #endregion

    #region Command 

    public int EnableComm()
    {
        if (_currentState == CommunicationState.DISABLED)
        {
            EnterCommunicationState();
            return 0;
        }
        return 1;
    }
    public int DisableComm()
    {
        if (_currentState != CommunicationState.DISABLED)
        {
            CommStateCheckTaskCts?.Cancel();
            CommDelayTimerTaskCts?.Cancel();
            CurrentState = CommunicationState.DISABLED;
            return 0;
        }
        return 1;

    }
    public int GoNotCommunicating()
    {
        if (CurrentState != CommunicationState.COMMUNICATING)
        {
            CommStateCheckTaskCts?.Cancel();
            CommDelayTimerTaskCts?.Cancel();
            EnterCommunicationState();
            return 1;
        }
        return 0;
    }

    #endregion
}
