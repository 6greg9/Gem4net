using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository.Model;
public class GemEvent
{
    public int ECID { get; set; }
    public int? DATAID { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; } //這裡是true,false
    public int? EnabledVid { get;set; } //這裡SV數值會是B(0),B(128)
    public string? Definition { get; set; }
    public string? Remark { get; set; }
    public string? Trigger { get; set; }

    public IList<EventReportLink> ReportEvents { get; set; }
}
