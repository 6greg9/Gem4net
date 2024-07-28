using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.Model;
public class GemEvent
{
    public int ECID { get; set; }
    public int? DATAID { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; } //這裡是true,false
    public string? Definition { get; set; }
    public string? Remark { get; set; }
    public string? Trigger { get; set; }

    public IList<EventReportLink> ReportEvents { get; set; }
}
