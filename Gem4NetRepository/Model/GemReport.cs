using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.Model;
public class GemReport
{
    public int RPTID { get; set; }
    public string? Definition { get; set; }
    public string? Remark { get; set; }

    public IList<EventReportLink> EventReports { get; set; }
    public IList<ReportVariableLink> ReportVariables { get; set; }
}
