using Gem4netRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Secs4Net;
using static Secs4Net.Item;
using SQLitePCL;
namespace Gem4netRepository;
public partial class GemRepository
{
    // 純增刪查改不做資料驗證
    public IEnumerable<FormattedProcessProgram> GetProcessProgramFormatted(string PPID)
    {
        using (_context = new GemDbContext())
        {
            var cn = _context.Database.GetDbConnection();
            var pps = cn.Query<FormattedProcessProgram>("SELECT * FROM FormattedProcessProgram").ToList();
            return pps;
        }
        return null;
    }
    public int GetProcessProgramFormatted(IEnumerable<string> PPIDs)
    {
        return 0;
    }
    public IEnumerable<FormattedProcessProgram> GetFormattedPPAll()
    {
        using (_context = new GemDbContext())
        {
            var cn = _context.Database.GetDbConnection();
            var PPs = cn.Query<FormattedProcessProgram>("SELECT * FROM FormattedProcessProgram").ToList();
            return PPs;
        }
    }
    public int CreateProcessProgram(FormattedProcessProgram pp)
    {
        using (_context = new GemDbContext())
        {
            var cn = _context.Database.GetDbConnection();
            cn.Execute("INSERT INTO FormattedProcessPrograms(ID, PPID, UpdateTime, Status, PPBody," +
                " Editor, Description, ApprovalLevel, SoftwareRevision, EquipmentModelType) " +
                 "VALUES(@ID, @PPID, @UpdateTime, @Status, @PPBody," +
                 " @Editor, @Description, @ApprovalLevel, @SoftwareRevision, @EquipmentModelType)", pp);

            return 0;
        }
    }
    public int UpdateProcessProgram(FormattedProcessProgram fpp) { return 0; }

    public int DeleteProcessProgram(List<string> ppids) {
        using (_context = new GemDbContext())
        {
            var cn = _context.Database.GetDbConnection();
            var rows = cn.Execute($"DELETE FROM FormattedProcessPrograms where PPID IN @ppids", new { ppids = ppids });
            if (rows > 0)
            {
                return 0;
            }
            return 1;
        }
    }
    public int DeleteProcessProgramAll()
    {
        using (_context = new GemDbContext())
        {
            var cn = _context.Database.GetDbConnection();
            var rows = cn.Execute($"DELETE FROM FormattedProcessPrograms ");
            return 0;
        }
    }

    public Item FormattedProcessProgramToSecsItem (FormattedProcessProgram fpp)
    {
        var secsFpp = Item.L();
        secsFpp.Items.Append(A(fpp.PPID));
        secsFpp.Items.Append(A(fpp.EquipmentModelType));
        secsFpp.Items.Append(A(fpp.SoftwareRevision));
        var secsPPbody = Item.L();
        var PPbody = new PPBodyHandler().Parse(fpp.PPBody);
        foreach (var processCmd in PPbody)
        {
            var secsPPcmd = Item.L();
            secsPPcmd.Items.Append(A(processCmd.CommandCode));
            var secsParaLst = Item.L();
            foreach (var para in processCmd.ProcessParameters)
            {
                var secsPara = VarStringToItem(para.DataType, para.Value);
                return secsPara;
                
            }
            secsPPcmd.Items.Append(secsParaLst);
            secsPPbody.Items.Append(secsPPcmd);
        }
        secsFpp.Items.Append(secsPPbody);
        return secsFpp;
    }
}
