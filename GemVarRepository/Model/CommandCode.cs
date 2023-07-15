using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository.Model;
/// <summary>
/// for process program
/// </summary>
internal class CommandCode
{
    public string Code { get; set; }
    public string Name { get; set; }
    IList<CommandCodePara> ParaList { get; set;}
}
