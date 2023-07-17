using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository.Model;
public class GemVariable
{
    public int VID { get; set; }
    public string DataType { get; set; }
    public int Length { get; set; }
    public string Unit { get; set; }
    public string Value { get; set; }
    public string Name { get; set; }
    public string? Definition { get; set; }
    public string? Remark { get; set; }
    public string VarType { get; set; }
    public bool System { get; set; }
    public string MinValue { get; set; }
    public string MaxValue { get; set; }
    public string DefaultValue { get; set; }
    public DateTime Version { get; set; }
    public IList<ReportVariableLink> ReportVariables { get; set; }
}
