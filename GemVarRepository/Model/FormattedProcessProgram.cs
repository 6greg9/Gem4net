using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository.Model;
public class FormattedProcessProgram
{
    public Guid Id { get; set; }
    public string PPID {  get; set; }
    public DateTime UpdateTime { get; set; }

    public List<ProcessParameter> ProcessParameters { get; set; } = new();
}
