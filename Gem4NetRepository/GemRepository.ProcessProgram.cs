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
            using (_context = new GemDbContext(_config))
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
        using (_context = new GemDbContext(_config))
        {
            var cn = _context.Database.GetDbConnection();
            var PPs = cn.Query<ProcessProgram>("SELECT * FROM ProcessPrograms")
                .ToList();
            return PPs;
        }
    }
    public int CreateProcessProgram(ProcessProgram pp)
    {
        using (_context = new GemDbContext(_config))
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
        using (_context = new GemDbContext(_config))
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
        using (_context = new GemDbContext(_config))
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
        var ppSecs = L(ppid, ppbody);
        return ppSecs;
    }

    public int PharseSecsItemToPP(Item secsFpp, out ProcessProgram pp)
    {
        pp = new ProcessProgram();
        try
        {
            pp.PPID = secsFpp.Items[0].GetString();
            pp.PPBody = secsFpp.Items[1].GetString();
            
            return 0;
        }
        catch (Exception ex)
        {
            return 1;
        }

    }

    #endregion

    #region Formatted
    // 純增刪查改不做資料驗證
    public IEnumerable<FormattedProcessProgram> GetFormattedProcessProgram(string PPID)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var cn = _context.Database.GetDbConnection();
                var pps = cn.Query<FormattedProcessProgram>("SELECT * FROM FormattedProcessProgram")
                    .Where(pp => pp.PPID == PPID).ToList();
                return pps;
            }
        }
    }
    public IEnumerable<FormattedProcessProgram> GetFormattedPPAll()
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var cn = _context.Database.GetDbConnection();
                var PPs = cn.Query<FormattedProcessProgram>("SELECT * FROM FormattedProcessPrograms")
                    .ToList();
                return PPs;
            }
        }
    }
    public int CreateFormattedProcessProgram(FormattedProcessProgram fpp)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                //var cn = _context.Database.GetDbConnection();

                //var row= cn.Execute("INSERT INTO FormattedProcessPrograms(ID, PPID, UpdateTime,  PPBody," +
                //    " Editor, Description, ApprovalLevel, SoftwareRevision, EquipmentModelType) " +
                //     "VALUES(@ID, @PPID, @UpdateTime, @PPBody," +
                //     " @Editor, @Description, @ApprovalLevel, @SoftwareRevision, @EquipmentModelType)", fpp);
                //Create Log, 要先Log再更新正在使用的表
                var fppLog = Mapper.Map<FormattedProcessProgramLog>(fpp);
                fppLog.LogId = Guid.NewGuid();
                fppLog.PPChangeStatus = 1;
                _context.FormattedProcessProgramLogs.Add(fppLog);

                _context.FormattedProcessPrograms.Add(fpp);

                _context.SaveChanges();
                return 0;

            }
        }
    }
    public int UpdateFormattedProcessProgram(FormattedProcessProgram fpp)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var ppid = fpp.PPID;
                var cn = _context.Database.GetDbConnection();
                fpp.LogId = Guid.NewGuid();
                fpp.UpdateTime = DateTime.Now;
                var rowCount = cn.Execute("UPDATE FormattedProcessPrograms SET LogId=@LogId, PPID=@PPID," +
                    " UpdateTime=@UpdateTime, PPBody=@PPBody," +
                    " Editor=@Editor, Description=@Description, ApprovalLevel=@ApprovalLevel, SoftwareRevision=@SoftwareRevision, EquipmentModelType=@EquipmentModel"
                     + $"WHERE PPID = {ppid}", fpp);

                if (rowCount > 0)
                {
                    // Edited Log
                    //var fppLog = fpp as FormattedProcessProgramLog;
                    var fppLog = Mapper.Map<FormattedProcessProgramLog>(fpp);
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
    public int DeleteFormattedProcessProgram(List<string> ppids)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {

                ///var rows = cn.Execute($"DELETE FROM FormattedProcessPrograms where PPID IN @ppids", new { ppids = ppids });
                var fpps = _context.FormattedProcessPrograms.Where(sc=>sc.GetType() != typeof(FormattedProcessProgramLog))
                    .Where(pp => ppids.Contains(pp.PPID));
                var test = fpps.ToList();
                foreach (var fpp in fpps)
                {
                    _context.FormattedProcessPrograms.Remove(fpp);

                    //Delete Log
                    var fppLog = Mapper.Map<FormattedProcessProgramLog>(fpp);
                    fppLog.LogId = Guid.NewGuid();
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
            using (_context = new GemDbContext(_config))
            {
                _context.FormattedProcessPrograms.ForEachAsync(fpp =>
                {
                    _context.FormattedProcessPrograms.Remove(fpp);

                    //Delete Log
                    var fppLog = Mapper.Map<FormattedProcessProgramLog>(fpp);
                    fppLog.UpdateTime = DateTime.Now;
                    fppLog.PPChangeStatus = 3;
                    _context.FormattedProcessProgramLogs.Add(fppLog);
                });

                _context.SaveChanges();
                return 0;
            }
        }
    }
    

    public Item FormattedProcessProgramToSecsItem(FormattedProcessProgram fpp)
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

    public int PharseSecsItemToFormattedPP(Item secsFpp, out FormattedProcessProgram fpp)
    {
        fpp = new FormattedProcessProgram();
        try
        {
            fpp.PPID = secsFpp.Items[0].GetString();
            fpp.EquipmentModelType = secsFpp.Items[1].GetString();
            fpp.SoftwareRevision = secsFpp.Items[2].GetString();
            foreach (var processCmd in secsFpp.Items[3].Items)
            {
                var pCmd = new ProcessCommand { CommandCode = processCmd.Items[0].GetString() };
                var paras = processCmd.Items[1];
                foreach (var para in paras.Items) // 這個要很注意客製
                {
                    var p = new ProcessParameter();
                    p.Value = para.GetString();
                }
            }
            return 0;
        }catch (Exception ex)
        {
            return 1;
        }
        
    }
    #endregion
}
