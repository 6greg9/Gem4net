using Secs4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.Model;
public class GemVariable
{
    public int     VID { get; set; }
    /// <summary>
    /// LIST , BOOL , ASCII , UINT_1 , UINT_2 , UINT_4 , UINT_8
    /// INT_1 , INT_2 , INT_4 , INT_8 , FLOAT_4 , FLOAT_8
    /// </summary>
    
    public string  DataType { get; set; } = string.Empty;
    public int?     Length { get; set; }
    public string?  Unit { get; set; }
    public string Value { get; set; } = string.Empty;
    public string SecsValue { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Definition { get; set; }
    public string? Remark { get; set; }
    /// <summary> EC, EV, SV/// </summary>
    public string VarType { get; set; } = string.Empty;
    /// <summary>System / Equpipment ... </summary>
    public string SourceDataType { get; set; } = string.Empty;
    public string?  MinValue { get; set; }
    public string?  MaxValue { get; set; }
    public string?  DefaultValue { get; set; } 
    //public int? ListSVID { get; set; } //在屬於的ListSV底下
    //public DateTime Version { get; set; }
    public IList<ReportVariableLink> ReportVariables { get; set; }
}
