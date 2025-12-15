using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.SystemVar;

public enum SYSTEM_EVENT
{
    GEM_PP_CHANGE                 = 3 , //GemProcessProgramChange,(Send by AP)
    GEM_CONTROL_STATE_LOCAL       = 8 , //GemControlStateLocal,
    GEM_CONTROL_STATE_REMOTE      = 9 , //GemControlStateRemote,
    GEM_EQ_CONST_CHANGED          = 20, //GemEqConstChanged,
    GEM_MESSAGE_RECOGNITION       = 21, //GemMessageRecognition,(Send by AP)
    GEM_EQP_OFF_LINE              = 22, //GemEqpOffLine,
    GEM_SPOOLING_ACTIVED          = 23, //GemSpoolingActived,
    GEM_SPOOLING_DEACTIVED        = 24, //GemSpoolingDeactived,
    GEM_SPOOL_TRANSMIT_FAILURE    = 25, //GemSpoolTransmitFailure,
    GEM_LIMIT_ZONE_TRANSITION     = 51, //GemLimitZoneTransition,
}
