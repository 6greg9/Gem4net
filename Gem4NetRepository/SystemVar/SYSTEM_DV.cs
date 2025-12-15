using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.SystemVar;
public enum SYSTEM_DV
{

    GEM_PP_CHANGE_NAME            = 9 , // ASCII,256,, GemPPChangeName,(Set by AP)
    GEM_PP_CHANGE_STATUS          = 10, // UINT_1,,, GemPPChangeStatus,"1:Create, 2:Changed, 3:Deleded (Set by AP)"
    FORMATTED_PP_CHANGE_CONTENT   = 16, // LIST,,, Formatted PPChange Content, The content of SVID:9(_GEM_PP_CHANGE_NAME_)
    EVENT_NAME                    = 17, // ASCII,100,, Last event name,
    GEM_ALARM_ID                  = 38, // UINT_4,,, GemAlarmID,
    GEM_ECID_CHANGED              = 46, // UINT_4,,, GemECIDChanged,
    GEM_EC_VALUE_CHANGED          = 47, // ASCII,1000,, ECValueChanged,
    GEM_PREVIOUS_EC_VALUE         = 48, // ASCII,1000,, PreviousECValue,
    GEM_LIMIT_VID                 = 62, // UINT_4,,, VID of zone transition,
    GEM_EVENT_LIMIT               = 63, // BINARY,,, LIMITID of the limit crossed by LimitVariable,
    GEM_TRANSTION_TYPE            = 64, // BINARY,,, The direction of the zone transition which has occurred," 0 = transition from lower to upper zone, 1 = transition from upper to lower zone"
}
