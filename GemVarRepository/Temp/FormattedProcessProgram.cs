using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository;//.Model;
public class FormattedProcessProgram
{
    public string PPID { get; set; }
    public List<ProcessCommand>? PPBody { get; set; } =new();//JSON column
}
