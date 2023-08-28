using Secs4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemDeviceService;
internal class CtrlStateManager
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
            NotifyCommStateChanged?.Invoke((_currentState, PreviousState));//別手動Ivoke狀態變化
        }
    }
    public ControlState PreviousState { get; private set; }
    public event Action<(ControlState currentState,ControlState previousState )>? NotifyCommStateChanged;
    #endregion

    Task CtrlStateCheckTask;
    CancellationTokenSource CtrlStateCheckTaskCts;
    SecsGem _secsGem;
    public CtrlStateManager(SecsGem secsGem) {
        _secsGem= secsGem;
    }
}
