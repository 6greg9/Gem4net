using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository.Model;
internal class ProcessProgram
{
    public string Name { get; set; }
    IList<CommandCode> CommandCodeList { get; set; }
}
