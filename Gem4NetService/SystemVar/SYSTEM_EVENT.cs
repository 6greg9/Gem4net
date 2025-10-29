using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4Net.SystemVar
{
    public enum SYSTEM_EVENT
    {
        GEM_PP_CHANGE_                 = 3 , //GemProcessProgramChange,(Send by AP)
        GEM_CONTROL_STATE_LOCAL_       = 8 , //GemControlStateLocal,
        GEM_CONTROL_STATE_REMOTE_      = 9 , //GemControlStateRemote,
        GEM_EQ_CONST_CHANGED_          = 20, //GemEqConstChanged,
        GEM_MESSAGE_RECOGNITION_       = 21, //GemMessageRecognition,(Send by AP)
        GEM_EQP_OFF_LINE_              = 22, //GemEqpOffLine,
        GEM_SPOOLING_ACTIVED_          = 23, //GemSpoolingActived,
        GEM_SPOOLING_DEACTIVED_        = 24, //GemSpoolingDeactived,
        GEM_SPOOL_TRANSMIT_FAILURE_    = 25, //GemSpoolTransmitFailure,
        GEM_LIMIT_ZONE_TRANSITION_     = 51, //GemLimitZoneTransition,
    }
}
