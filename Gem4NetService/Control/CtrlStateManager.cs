using Secs4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4net.Control;
public class CtrlStateManager
{
    #region State, TransitionEvent
    ControlState _currentState;
    public ControlState CurrentState
    {
        get { return _currentState; }
        private set
        {
            PreviousState = _currentState;
            _currentState = value;
            if (CurrentState != PreviousState)
            {
                NotifyCommStateChanged?.Invoke((_currentState, PreviousState));//別手動Ivoke狀態變化

            }
        }
    }
    public ControlState PreviousState { get; private set; }
    public event Action<(ControlState currentState, ControlState previousState)>? NotifyCommStateChanged;
    public bool IsOnLine => CurrentState is ControlState.LOCAL or ControlState.REMOTE;
    #endregion

    public ControlState DefaultOnOffLine = ControlState.EQUIPMENT_OFF_LINE;
    public ControlState DefaultLocalRemote = ControlState.LOCAL;
    public ControlState DefaultAfterFailOnline = ControlState.HOST_OFF_LINE;
    Task CtrlStateCheckTask = Task.CompletedTask;
    CancellationTokenSource CtrlStateCheckTaskCts;
    ISecsGem _secsGem;
    public CtrlStateManager(ISecsGem secsGem)
    {
        _secsGem = secsGem;

    }

    #region StateMachine

    public void EnterControlState()
    {
        CurrentState = DefaultLocalRemote;

        CtrlStateCheckTaskCts = new();
        var token = CtrlStateCheckTaskCts.Token;
        CtrlStateCheckTask = Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                switch (_currentState)
                {
                    case ControlState.EQUIPMENT_OFF_LINE:
                        CtrlStateCheckTaskCts.Cancel(); //可以不用一直執行
                        break;
                    case ControlState.ATTEMPT_ON_LINE:
                        var onlineReqResult = await S1F2Waiter;
                        if (S1F2Waiter.IsFaulted == true)
                        {
                            CurrentState = DefaultAfterFailOnline;
                            break;
                        }

                        if (onlineReqResult.S == 1 && onlineReqResult.F == 2)
                        {
                            CurrentState = DefaultLocalRemote;
                        }
                        else
                        {
                            CurrentState = DefaultAfterFailOnline;
                        }
                        break;
                    case ControlState.HOST_OFF_LINE:
                        CtrlStateCheckTaskCts.Cancel();
                        break;
                    case ControlState.LOCAL:
                        CtrlStateCheckTaskCts.Cancel();
                        break;
                    case ControlState.REMOTE:
                        CtrlStateCheckTaskCts.Cancel();
                        break;
                    default: break;
                }

                await Task.Delay(50);
            }
        });
    }

    #endregion

    #region Action



    #endregion

    #region Command

    Task<SecsMessage> S1F2Waiter;

    public int OnLineRequest()
    {
        if (CurrentState != ControlState.EQUIPMENT_OFF_LINE)
        {
            return 1;
        }
        //可能可以改用Func, 就不用注入_secsGem
        using (var S1F1 = new SecsMessage(1, 1))
            S1F2Waiter = _secsGem.SendAsync(S1F1); //在while外部       
        CurrentState = ControlState.ATTEMPT_ON_LINE;
        EnterControlState();
        return 0;
    }

    public int OffLine()
    {
        CurrentState = ControlState.EQUIPMENT_OFF_LINE;
        return 0;
    }

    public int OnLineRemote()
    {
        if (CurrentState is ControlState.LOCAL or ControlState.REMOTE)
        {
            CurrentState = ControlState.REMOTE;
            return 0;
        }
        return 1;
    }

    public int OnLineLocal()
    {
        if (CurrentState is ControlState.LOCAL or ControlState.REMOTE)
        {
            CurrentState = ControlState.LOCAL;
            return 0;
        }
        return 1;
    }

    public int HandleS1F15()
    {
        if (IsOnLine == true)
        {
            CurrentState = ControlState.HOST_OFF_LINE;
            return 0;
        }
        return 1;
    }

    public int HandleS1F17()
    {
        if (CurrentState == ControlState.HOST_OFF_LINE)
        {
            CurrentState = DefaultOnOffLine;
            return 0;
        }
        return 1;
    }

    #endregion
}
