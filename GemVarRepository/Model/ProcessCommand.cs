using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository.Model;
/// <summary>
/// for process program
/// </summary>
public class ProcessCommand
{
    public string CommandCode { get; set; }
    public List<ProcessParameter>? ProcessParameters { get; set; } = new();//JSON column
}
