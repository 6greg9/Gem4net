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
    public int GetProcessProgramFormatted(string PPID)
    {
        using (_context = new GemVarContext())
        {
            var cn = _context.Database.GetDbConnection();
            var pp = cn.Query<FormattedProcessProgram>("SELECT * FROM FormattedProcessProgram").ToList();
            return 0;
        }
        return 0;
    }
    public int GetProcessProgramFormatted(IEnumerable<string> PPIDs)
    {
        return 0;
    }
    public IEnumerable<FormattedProcessProgram> GetFormattedPPAll()
    {
        using (_context = new GemVarContext())
        {
            var cn = _context.Database.GetDbConnection();
            var PPs = cn.Query<FormattedProcessProgram>("SELECT * FROM FormattedProcessProgram").ToList();
            return PPs;
        }
    }
    public int CreateProcessProgram(FormattedProcessProgram pp)
    {
        using (_context = new GemVarContext())
        {
            var cn = _context.Database.GetDbConnection();
            cn.Execute("INSERT INTO FormattedProcessPrograms(ID, PPID, UpdateTime, Status, PPBody," +
                " Editor, Description, ApprovalLevel, SoftwareRevision, EquipmentModelType) " +
                 "VALUES(@ID, @PPID, @UpdateTime, @Status, @PPBody," +
                 " @Editor, @Description, @ApprovalLevel, @SoftwareRevision, @EquipmentModelType)", pp);

            return 0;
        }
    }
    public int UpdateProcessProgram() { return 0; }

    public int DeleteProcessProgram() { return 0; } 
}
