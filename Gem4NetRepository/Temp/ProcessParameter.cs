using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4netRepository.Temp;
[ComplexType]
public class ProcessParameter
{
    public string DataType { get; set; }
    public int Length { get; set; }
    public string Unit { get; set; }
    public string Value { get; set; }
    public string Name { get; set; }
    public string? Definition { get; set; }
    public string? Remark { get; set; }

}
