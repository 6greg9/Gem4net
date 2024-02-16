using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4netRepository.Model;
public class EventReportLink
{
    public int RPTID { get; set; }
    public GemReport Report { get; set; }
    public int ECID { get; set; }
    public GemEvent Event { get; set; }
}
