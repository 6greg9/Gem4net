using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository;
public partial class GemRepository
{
    // 純增刪查改不做資料驗證
    public int GetProcessProgram(string PPID)
    {
        return 0;
    }
    public int GetProcessProgram(IEnumerable<string> PPIDs)
    {
        return 0;
    }
    public int CreateProcessProgram()
    {
        return 0;
    }
    public int UpdateProcessProgram() { return 0; }

    public int DeleteProcessProgram() { return 0; } 
}
