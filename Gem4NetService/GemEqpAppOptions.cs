using Gem4Net.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4Net;
public class GemEqpAppOptions
{
    /// <summary>
    /// MDLN     Format: A:20 (invariant)
    /// </summary>
    public string ModelType { get; set; }
    /// <summary>
    /// SOFTREV     Format: A:20
    /// </summary>
    public string SoftwareVersion { get; set; }
    /// <summary>
    /// for TIME, ECV TimeFormat controls format, 0=A:12 YYMMDDHHMMSS, 1=A:16 YYYYMMDDHHMMSScc,2=YYYY-MM-DDTHH:MM:SS.s[s]*{Z|+hh:mm|-hh:mm}     
    /// </summary>
    public int ClockFormatCode { get; set; }
    public int TimeFormatVID { get; set; }
    public int ClockVID { get; set; }
    

    #region Communication State
    /// <summary>
    /// HSMS連上後, Comm是否預設Enabled
    /// </summary>
    public int IsCommDefaultEnabled { get; set; }
    /// <summary>
    /// HOST-INITIATED(等Host的S1F13) 或 EQPIPMENT-INITIATED(主動定時S1F13)
    /// </summary>
    public int IsCommHostInit { get; set; }
    
    #endregion

    #region Control State
    public int DefaultInitControlState { get; set; }
    public int DefaultAfterFailOnline { get; set; }
    public int DefaultLocalRemote { get; set; }

    #endregion

    #region W bit
    /// <summary>
    /// 0: False, 1: True
    /// </summary>
    public int IsS5WbitUsed {  get; set; }
    public int IsS6WbitUsed { get; set; }
    public int IsS10WbitUsed { get; set; }
    #endregion

    #region Spool
    public int IsSpoolEnabled {  get; set; }
    public int OverWriteSpool { get; set; }
    /// <summary>
    /// EQPIPMENT-INITIATED的時候, S1F13的等待時間, 同等CommDelaySecond, 注意與HSMS的T3秒數大小
    /// </summary>
    public int? EstablishCommunicationsTimeout { get; set; }
    public int? EstablishCommunicationsTimeoutVID { get; set; }
    #endregion

    #region ProcessProgram
    /// <summary>
    /// 0:Ascii, 1:U2, 2:U4, 3:I2, 4:I4
    /// </summary>
    public int CommandCodeFormat { get; set; }
    #endregion
}
