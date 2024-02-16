using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.Temp;
/// <summary>
/// for process program
/// </summary>
[ComplexType]
public class ProcessCommand
{
    public string CommandCode { get; set; }
    public List<ProcessParameter>? ProcessParameters { get; set; } = new();//JSON column
}
