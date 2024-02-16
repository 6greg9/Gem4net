using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4netRepository.Model;
public class ReportVariableLink
{
    public int RPTID { get; set; }
    public GemReport Report { get; set; }
    public int VID { get; set; }
    public GemVariable Variable { get; set; }
}
