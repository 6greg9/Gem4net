using Gem4NetRepository.Model;
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
using static System.Net.WebRequestMethods;
namespace Gem4NetRepository;
public partial class GemRepository
{
    #region No Format
    public IEnumerable<ProcessProgram> GetProcessProgram(string PPID)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(DbFilePath))
            {
                var cn = _context.Database.GetDbConnection();
                var pps = cn.Query<ProcessProgram>("SELECT * FROM ProcessPrograms")
                    .Where(pp => pp.PPID == PPID).ToList();
                return pps;
            }
        }
    }
    public IEnumerable<ProcessProgram> GetProcessProgramAll()
    {
        using (_context = new GemDbContext(DbFilePath))
        {
            var cn = _context.Database.GetDbConnection();
            var PPs = cn.Query<ProcessProgram>("SELECT * FROM ProcessPrograms")
                .ToList();
            return PPs;
        }
    }
    public int CreateProcessProgram(ProcessProgram pp)
    {
        using (_context = new GemDbContext(DbFilePath))
        {
            var cn = _context.Database.GetDbConnection();
            cn.Execute("INSERT INTO ProcessPrograms(ID, PPID, UpdateTime, Status, PPBody," +
                " Editor, Description, ApprovalLevel, SoftwareRevision, EquipmentModelType) " +
                 "VALUES(@ID, @PPID, @UpdateTime, @Status, @PPBody," +
                 " @Editor, @Description, @ApprovalLevel, @SoftwareRevision, @EquipmentModelType)", pp);

            return 0;
        }
    }
    public int UpdateProcessProgram(ProcessProgram pp) { return 0; }

    public int DeleteProcessProgram(List<string> ppids)
    {
        using (_context = new GemDbContext(DbFilePath))
        {
            var cn = _context.Database.GetDbConnection();
            var rows = cn.Execute($"DELETE FROM ProcessPrograms where PPID IN @ppids", new { ppids = ppids });
            if (rows > 0)
            {
                return 0;
            }
            return 1;
        }
    }
    public int DeletedProcessProgramAll()
    {
        using (_context = new GemDbContext(DbFilePath))
        {
            var cn = _context.Database.GetDbConnection();
            var rows = cn.Execute($"DELETE FROM ProcessPrograms ");
            return 0;
        }
    }

    public Item ProcessProgramToSecsItem(ProcessProgram pp)
    {
        var ppid = A(pp.PPID);
        var ppbody = A(pp.PPBody);
        var ppSecs = L(ppid,ppbody);
        return ppSecs;
    }
    #endregion

    #region Formatted
    // 純增刪查改不做資料驗證
    public IEnumerable<FormattedProcessProgram> GetFormattedProcessProgram(string PPID)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(DbFilePath))
            {
                var cn = _context.Database.GetDbConnection();
                var pps = cn.Query<FormattedProcessProgram>("SELECT * FROM FormattedProcessProgram")
                    .Where(pp=>pp.PPID==PPID ).ToList();
                return pps;
            }
        }
    }
    public IEnumerable<FormattedProcessProgram> GetFormattedPPAll()
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(DbFilePath))
            {
                var cn = _context.Database.GetDbConnection();
                var PPs = cn.Query<FormattedProcessProgram>("SELECT * FROM FormattedProcessProgram")
                    .ToList();
                return PPs;
            }
        }
    }
    public int CreateFormattedProcessProgram(FormattedProcessProgram pp)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(DbFilePath))
            {
                var cn = _context.Database.GetDbConnection();
                
                var row= cn.Execute("INSERT INTO FormattedProcessPrograms(ID, PPID, UpdateTime,  PPBody," +
                    " Editor, Description, ApprovalLevel, SoftwareRevision, EquipmentModelType) " +
                     "VALUES(@ID, @PPID, @UpdateTime, @PPBody," +
                     " @Editor, @Description, @ApprovalLevel, @SoftwareRevision, @EquipmentModelType)", pp);
                if (row == 1)
                {
                    //Create Log
                    var fppLog = pp as FormattedProcessProgramLog;
                    fppLog.LogId = Guid.NewGuid();
                    fppLog.PPChangeStatus = 1;
                   _context.FormattedProcessProgramLogs.Add(fppLog);
                }

                return 1;
            }
        }
    }
    public int UpdateFormattedProcessProgram(FormattedProcessProgram fpp) {
        lock (lockObject)
        {
            using (_context = new GemDbContext(DbFilePath))
            {
                var id = fpp.ID;
                var cn = _context.Database.GetDbConnection();
                fpp.UpdateTime = DateTime.Now;
                var rowCount = cn.Execute("UPDATE FormattedProcessPrograms SET ID=@ID, PPID=@PPID,"+
                    " UpdateTime=@UpdateTime, PPBody=@PPBody," +
                    " Editor=@Editor, Description=@Description, ApprovalLevel=@ApprovalLevel, SoftwareRevision=@SoftwareRevision, EquipmentModelType=@EquipmentModel"
                     +$"WHERE ID = {id}", fpp);

                if (rowCount > 0)
                {
                    // Edited Log
                    var fppLog = fpp as FormattedProcessProgramLog;
                    fppLog.UpdateTime = DateTime.Now;
                    fppLog.PPChangeStatus = 2;
                    _context.FormattedProcessProgramLogs.Add(fppLog);
                    _context.SaveChanges();
                    return 0;
                }
                return 1;
            }
        }
    }
    /// <summary>
    /// for S7F17
    /// </summary>
    /// <param name="ppids"></param>
    /// <returns></returns>
    public int DeleteFormattedProcessProgram(List<string> ppids) {
        lock (lockObject)
        {
            using (_context = new GemDbContext(DbFilePath))
            {

                ///var rows = cn.Execute($"DELETE FROM FormattedProcessPrograms where PPID IN @ppids", new { ppids = ppids });
                var fpps = _context.FormattedProcessPrograms.Where(pp => ppids.Contains(pp.PPID));
                foreach (var fpp in fpps)
                {
                    _context.FormattedProcessPrograms.Remove(fpp);

                    //Delete Log
                    var fppLog = fpp as FormattedProcessProgramLog;
                    fppLog.UpdateTime = DateTime.Now;
                    fppLog.PPChangeStatus = 3;
                    _context.FormattedProcessProgramLogs.Add(fppLog);
                }
                _context.SaveChanges();
                return 1;
            }
        }
    }
    public int DeleteFormattedPPAll()
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(DbFilePath))
            {
                _context.FormattedProcessPrograms.ForEachAsync(fpp =>
                {
                    _context.FormattedProcessPrograms.Remove(fpp);

                    //Delete Log
                    var fppLog = fpp as FormattedProcessProgramLog;
                    fppLog.UpdateTime = DateTime.Now;
                    fppLog.PPChangeStatus = 3;
                    _context.FormattedProcessProgramLogs.Add(fppLog);
                });
                
                _context.SaveChanges();
                return 0;
            }
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
    #endregion
}
