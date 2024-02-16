using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4netRepository.Model;
public class GemAlarm
{
    /// <summary> 警報ID </summary>
    public int ALID { get; set; } 
    /// <summary> 警報狀態 </summary>
    public bool ALCD { get; set; }
    public int? AlarmStateVid { get; set; }
    public bool DefaultAlarmState { get; set; }
    /// <summary> 警報啟用 </summary>
    public bool ALED { get; set; }
    public int? AlarmEnableVid { get; set; }
    public bool DefaultAlarmEnable { get; set; }
    /// <summary> 警報文字說明 </summary>
    public string ALTX { get; set; }

}
