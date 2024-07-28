using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.Model;
public class GemAlarm
{
    /// <summary> 警報ID </summary>
    public int ALID { get; set; } 
    /// <summary> 警報狀態 0~255, 128為分界 </summary>
    public int ALCD { get; set; }
    public int DefaultAlarmState { get; set; }
    /// <summary> 警報啟用 </summary>
    public bool ALED { get; set; }
    public bool DefaultAlarmEnable { get; set; }
    /// <summary> 警報文字說明 </summary>
    public string ALTX { get; set; }

}
