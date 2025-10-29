using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.SystemVar;
public enum SYSTEM_DV
{

    GEM_PP_CHANGE_NAME_            = 9 , // ASCII,256,, GemPPChangeName,(Set by AP)
    GEM_PP_CHANGE_STATUS_          = 10, // UINT_1,,, GemPPChangeStatus,"1:Create, 2:Changed, 3:Deleded (Set by AP)"
    FORMATTED_PP_CHANGE_CONTENT_   = 16, // LIST,,, Formatted PPChange Content, The content of SVID:9(_GEM_PP_CHANGE_NAME_)
    EVENT_NAME                     = 17, // ASCII,100,, Last event name,
    GEM_ALARM_ID_                  = 38, // UINT_4,,, GemAlarmID,
    GEM_ECID_CHANGED_              = 46, // UINT_4,,, GemECIDChanged,
    GEM_EC_VALUE_CHANGED_          = 47, // ASCII,1000,, ECValueChanged,
    GEM_PREVIOUS_EC_VALUE_         = 48, // ASCII,1000,, PreviousECValue,
    GEM_LIMIT_VID_                 = 62, // UINT_4,,, VID of zone transition,
    GEM_EVENT_LIMIT_               = 63, // BINARY,,, LIMITID of the limit crossed by LimitVariable,
    GEM_TRANSTION_TYPE_            = 64, // BINARY,,, The direction of the zone transition which has occurred," 0 = transition from lower to upper zone, 1 = transition from upper to lower zone"
}
