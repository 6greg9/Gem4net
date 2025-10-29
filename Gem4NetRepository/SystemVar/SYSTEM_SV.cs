using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetService.SystemVar;
public enum SYSTEM_SV
{
    //SV,,,,,,
    // SV Const Name Define,ID,Type,Length,Unit,SV Name,Remark
    //LICENSE_CODE_SVID_             = 1 ,      //ASCII,39,, License Code,(Read Only)
    //LICENSE_STATUS_SVID_           = 2 ,      //UINT_1,,, License Status,"0:No License, 1:Normal License, 2:Small License (Read Only)"
    GEM_CLOCK_                     = 3 ,      //ASCII,19,, GemClock,
    GEM_CONTROL_STATE_             = 4 ,      //UINT_1,,, GemControlState,"1:Eqp OffLine, 2:Attemp-OnLine, 3:Host Off-Line, 4:OnLine Local, 5:OnLine Remote"
    GEM_LINK_STATE_                = 5 ,      //UINT_1,,, GemLinkState,"0:Disabled, 1:Enabled/Not Communicating , 2: Communicating"
    GEM_COMM_MODE_                 = 6 ,      //UINT_1,,, SECSCommunicationMode,"0:HSMS Mode(default), 1:SECS Mode (Set by AP)"
    GEM_PREVIOUS_CEID_             = 11,      //UINT_4,,, GemPreviousCEID,
    GEM_OFF_LINE_SUB_STATE_SV_     = 12,      //UINT_1,,, GemOffLineSubState,"1:Eqp OffLine, 2:Attempt OnLine, 3:HostOffLiine, 0:unknown"
    GEM_PREVIOUS_CONTROL_STATE_    = 13,      //UINT_1,,, GemPreviousControlState,"1:Eqp OffLine, 2:Attemp-OnLine, 3:Host Off-Line, 4:OnLine Local, 5:OnLine Remote"
    GEM_PREVIOUS_PROCESS_STATE_    = 14,      //UINT_1,,, GemPreviousProcessState,(Set by AP)
    GEM_PROCESS_STATE_             = 15,      //UINT_1,,, GemProcessState,(Set by AP)
    GEM_MDLN_                      = 24,      //ASCII            ,20,, GemMDLN,(Set by AP)
    GEM_SOFTREV_                   = 25,      //ASCII,20,, GemSOFTREV,(Set by AP)
    GEM_ALARM_ENABLED_             = 39,      //LIST,,, List GemAlarmsEnabled,
    GEM_ALARM_SET_                 = 40,      //LIST,,, List GemAlarmSet,
    GEM_EVENT_ENABLED_             = 41,      //LIST,,, List GemEventsEnabled,
    GEM_PP_EXEC_NAME_              = 42,      //ASCII,256,, GemPPExecName,(Set by AP)
    PP_FORMAT_                     = 43,      //UINT_1,,, PPFormat,"1: Unformatted process programs(default), 2: Formatted process programs, 3 Both unformatted and formatted process programs"
    GEM_SPOOL_COUNT_ACTUAL_        = 53,      //UINT_4,,, GemSpoolCountActual,
    GEM_SPOOL_COUNT_TOTAL_         = 54,      //UINT_4,,, GemSpoolCountTotal,
    GEM_SPOOL_FULL_TIME_           = 55,      //ASCII,19,, GemSpoolFullTime,
    GEM_SPOOL_LOAD_SUBSTATE_       = 56,      //UINT_1,,,GemSpoolLoadSubstate,"6:Not Full , 7:Full //?"
    GEM_SPOOL_START_TIME_          = 57,      //ASCII,19,, GemSpoolStartTime,
    GEM_SPOOL_STATE_               = 58,      //UINT_1,,, GemSpoolState,"1:Inactive , 2:Active"
    GEM_SPOOL_UNLOAD_SUBSTATE_     = 59,      //UINT_1,,,GemSpoolUnloadSubstate,"3:Purge , 4:transmit , No Spool Out //?"
    GEM_SOFTWARE_REVISION_         = 80,      //ASCII,20,, Gem Soft Ware Revision,
}
