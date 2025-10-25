using PooledAwait;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gem4Net.Spooling;

public class SpoolManager
{
    #region State, TransitionEvent
    SpoolingState _currentState;
    public SpoolingState CurrentState
    {
        get { return _currentState; }
        private set
        {
            PreviousState = _currentState;
            _currentState = value;
            if (CurrentState != PreviousState)
                NotifySpoolingStateChanged?.Invoke((_currentState, PreviousState));//別手動Ivoke狀態變化

        }
    }
    public SpoolingState PreviousState { get; private set; }
    public event Action<(SpoolingState currentState, SpoolingState previousState)>? NotifySpoolingStateChanged;

    public SpoolLoadState LoadState { get; private set; }
    public SpoolLoadState UnloadState { get; private set; }
    #endregion

    //SpoolAreaSize ?

    /// <summary>
    /// An equipment constant containing the maximum number of messages that the
    /// equipment shall transmit from the spool in response to an S6, F23 ‘Transmit Spooled Messages’ request.If
    /// MaxSpoolTransmit is set to zero, no limit is placed on the messages sent from the spool.Multi-block inquire/grant
    /// messages are not counted in this total.
    /// </summary>
    public int MaxSpoolTransmit { get; set; } = 0;
    /// <summary>
    /// A Boolean equipment constant used to indicate to the equipment whether to overwrite
    /// data in the spool area or to discard further messages whenever the spool area limits are exceeded
    /// </summary>
    public bool OverWriteSpool { get; set; }

    // SendQueue
    // SpoolArea
    // SpoolCountActual
    // SpoolCountTotal

    // SpoolMessageTable for S2F43

    public DateTime SpoolFullTime { get; private set; }
    public DateTime SpoolStartTime { get; private set; }
    public SpoolManager()
    {

    }

    /// <summary>
    /// Communication state changes from COMMUNICATIN to NOT COMMUNICATING 
    /// or from WAIT CRA to WAIT DELAY and Enable Spool is true.
    /// </summary>
    public void ActivateSpooling()
    {
        //SpoolCountActual and SpoolCountTotal are initialized to zero.Any open transactions with the host are aborted.
        //SpoolStart - Time(SV) is set to current time. Alert the operator that spooling is active.

        CurrentState = SpoolingState.SPOOL_INACTIVE;
    }
}
