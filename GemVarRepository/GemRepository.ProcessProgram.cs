using GemVarRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace GemVarRepository;
public partial class GemRepository
{
    // 純增刪查改不做資料驗證
    public int GetProcessProgram(string PPID)
    {
        using (_context = new GemVarContext())
        {
            var cn = _context.Database.GetDbConnection();
            //cn.Execute
            return 0;
        }
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
