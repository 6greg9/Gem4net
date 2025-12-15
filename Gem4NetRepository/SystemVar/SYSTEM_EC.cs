using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.SystemVar;
public enum SYSTEM_EC
{
    //EnableSpooling,
    //EstablishingCommunicationsTimeout,
    //MaxSpoolTransmit,
    //OverWriteSpool,
    //TimeFormat

    GEM_INIT_COMM_STATE           = 7 ,// UINT_1,0,1,0,, GemInitCommState,"0:Disable, 1:Enable"
    GEM_INIT_CONTROL_STATE        = 8 ,// UINT_1,1,2,1,, GemInitControlState,"1:OffLine, 2:OnLine"
    GEM_WBIT_S5                   = 21,// UINT_1,0,1,1,, GemWBitS5,"0:Not Set, 1:Set  (for send S5Fx )"
    GEM_WBIT_S6                   = 22,// UINT_1,0,1,1,, GemWBitS6,"0:Not Set, 1:Set  (for send S6F11 )"
    GEM_WBIT_S10                  = 23,// UINT_1,0,1,1,, GemWBitS10,"0:Not Set, 1:Set (for send S10F1)"
    GEM_POLL_DELAY                = 26,// UINT_2,0,65535,0,,GemPollDelay,"0:no S1F1 sent, >0: delay"
    GEM_ESTAB_COMM_DELAY          = 44,// UINT_2,1,10000,5, Sec, GemEstabCommDelay,
    GEM_OFF_LINE_SUBSTATE         = 49,// UINT_1,1,3,1,, GemOffLineSubstate,"1:Eqp. OFF-line , 2:Attempt On-line , 3:Host Off-line"
    GEM_ON_LINE_FAILED            = 50,// UINT_1,1,3,1,, GemOnlineFailed,"1:Eqp. OFF-line , 3:Host Off-line"
    GEM_ON_LINE_SUBSTATE          = 51,// UINT_1,4,5,4,, GemOnLineSubstate,"4:On-line/Local, 5:ON-line/Remote"
    GEM_MAX_SPOOL_TRANSMIT        = 52,// UINT_4,1,200000000,100,, GemMaxSpoolTransmit,
    GEM_LIMIT_DELAY               = 65,// UINT_2,1,65535,10,Sec,GemLimitDelay,
    GEM_CONFIG_SPOOL              = 66,// UINT_1,0,1,0,, GemConfigSpool,"0:Disable, 1:Enable"
    GEM_OVER_WRITE_SPOOL          = 67,// BOOLEAN,0,1,0,, GemOverWriteSpool,"1:Overwrite, 0:Do not overwrite"
    GEM_TIME_FORMAT               = 68,// UINT_1,0,3,1,, GemTimeFormat,"0:12-bytes, 1:16-bytes, 2:14-bytes, 3:ISO8601 format"
    GEM_DATAID_FORMAT             = 71,// UINT_1,1,6,5,, GemDATAIDFormat,"1:INT_1, 2:INT_2, 3:INT_4, 4:UINT_1, 5:UINT_2, 6:UINT_4 (for send S6F11)"
    GEM_RPTID_FORMAT              = 73,// UINT_1,0,9,0,,GemRPTIDFormat,
    GEM_TRID_FORMAT               = 74,// UINT_1,0,9,0,,GemTRIDFormat,
    GEM_SAMPLN_FORMAT             = 75,// UINT_1,1,6,5,, GemSAMPLNFormat,"1:INT_1, 2:INT_2, 3:INT_4, 4:UINT_1, 5:UINT_2, 6:UINT_4 (for send S6F1)"


}
