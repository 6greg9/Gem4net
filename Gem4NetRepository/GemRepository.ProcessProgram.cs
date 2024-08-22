using Gem4NetRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Secs4Net;
using static Secs4Net.Item;
using System.Text.Json;
using Secs4Net.Json;
using Npgsql.Internal.TypeHandling;
using static System.Net.WebRequestMethods;

namespace Gem4NetRepository;

public partial class GemRepository // 這部分應該是可以獨立
{
    static SemaphoreSlim semSlim = new SemaphoreSlim(1, 1);
    #region No Format
    public IEnumerable<ProcessProgram> GetProcessProgram(string PPID)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {

                var pps = _context.ProcessPrograms
                    .Where(pp => pp.PPID == PPID).ToList();
                return pps;
            }
        }
    }
    public IEnumerable<ProcessProgram> GetProcessProgramAll()
    {
        using (_context = new GemDbContext(_config))
        {

            var PPs = _context.ProcessPrograms
                .ToList();
            return PPs;
        }
    }
    public async Task<int> CreateProcessProgram(ProcessProgram pp)
    {

        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {
                //Create Log, 要先Log再更新正在使用的表
                var doesExist = _context.ProcessPrograms.Any(p => p.PPID == pp.PPID);// 潛在問題,沒有在同個transaction, 需要上鎖

                if (doesExist)
                {
                    var target = _context.ProcessPrograms.Where(p => p.PPID == pp.PPID).Take(1).Single();//.Take(1);
                                                                                     //});
                    _context.ProcessPrograms.Remove(target);
                    _context.ProcessPrograms.Add(pp);
                    //_context.FormattedProcessPrograms.Update(target);
                    LogPPChanged(2);
                }
                else
                {
                    _context.ProcessPrograms.Add(pp);
                    LogPPChanged(1);

                }

                _ = _context.SaveChanges();


                /// <summary>
                /// For PPChangeStatus, 1 Created, 2 Edited, 3 Deleted , 4-64 Reserved
                /// </summary>
                void LogPPChanged(int ppChangeStatus)
                {
                    var fppLog = Mapper.Map<FormattedProcessProgramLog>(pp);

                    fppLog.PPChangeStatus = ppChangeStatus;


                    _context.FormattedProcessProgramLogs.Add(fppLog);
                }

            }
        }

        finally { semSlim.Release(); }

        return 0;
    }
    public int UpdateProcessProgram(ProcessProgram pp) { return 0; }

    public int DeleteProcessProgram(List<string> ppids)
    {
        using (_context = new GemDbContext(_config))
        {
            //var cn = _context.Database.GetDbConnection();
            //var rows = cn.Execute($"DELETE FROM ProcessPrograms where PPID IN @ppids", new { ppids = ppids });

            _context.Remove(_context.ProcessPrograms.Where(pp => ppids.Contains(pp.PPID)));
            var rows = _context.SaveChanges();
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

            _context.RemoveRange(_context.ProcessPrograms);
            _context.SaveChanges();
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

                var pps = _context.FormattedProcessPrograms
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
                var PPs = _context.FormattedProcessPrograms.ToList();
                //var tableName = "FormattedProcessPrograms";
                //var cn = _context.Database.GetDbConnection();
                //var PPs = cn.Query<FormattedProcessProgram>($"SELECT * FROM {tableName}")
                //    .ToList();
                return PPs;
            }
        }
    }
    public async Task<int> CreateFormattedProcessProgram(FormattedProcessProgram fpp)
    {

        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {

                //var cn = _context.Database.GetDbConnection();
                //var sql = "INSERT INTO \"FormattedProcessPrograms\"( \"ID\", \"PPID\", \"UpdateTime\",  \"PPBody\"," +
                //    " \"Editor\", \"Description\", \"ApprovalLevel\", \"SoftwareRevision\", \"EquipmentModelType\") " +
                //     "VALUES(@ID, @PPID, @UpdateTime, @PPBody," +
                //     " @Editor, @Description, @ApprovalLevel, @SoftwareRevision, @EquipmentModelType)";
                //var row = cn.Execute(sql, fpp);
                //Create Log, 要先Log再更新正在使用的表
                var doesExist = _context.FormattedProcessPrograms.Any(p => p.PPID == fpp.PPID);// 潛在問題,沒有在同個transaction, 需要上鎖

                if (doesExist)
                {
                    var target = _context.FormattedProcessPrograms.Where(p => p.PPID == fpp.PPID).Take(1).Single();//.Take(1);
                                                                                                                   //await target.ForEachAsync(p => //行不通
                                                                                                                   //{
                                                                                                                   //    var guid = p.LogId;
                                                                                                                   //    p = fpp;
                                                                                                                   //    p.LogId = guid;
                                                                                                                   //});
                    _context.FormattedProcessPrograms.Remove(target);
                    _context.FormattedProcessPrograms.Add(fpp);
                    //_context.FormattedProcessPrograms.Update(target);
                    LogPPChanged(2);
                }
                else
                {
                    _context.FormattedProcessPrograms.Add(fpp);
                    LogPPChanged(1);

                }

                _ = _context.SaveChanges();


                /// <summary>
                /// For PPChangeStatus, 1 Created, 2 Edited, 3 Deleted , 4-64 Reserved
                /// </summary>
                void LogPPChanged(int ppChangeStatus)
                {
                    var fppLog = Mapper.Map<FormattedProcessProgramLog>(fpp);
                   
                    fppLog.PPChangeStatus = ppChangeStatus;


                    _context.FormattedProcessProgramLogs.Add(fppLog);
                }

            }
        }

        finally { semSlim.Release(); }
        
        return 0;
    }
    /// <summary>
    /// 忘記要用在哪邊了...
    /// </summary>
    /// <param name="fpp"></param>
    /// <returns></returns>
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
                var rowCount = cn.Execute($"UPDATE {nameof(FormattedProcessProgram)} SET LogId=@LogId, PPID=@PPID," +
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
                var fpps = _context.FormattedProcessPrograms.Where(sc => sc.GetType() != typeof(FormattedProcessProgramLog))
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
                    fppLog.UpdateTime = DateTime.UtcNow;
                    fppLog.LogId  = Guid.NewGuid();
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
        
        
        var secsPPbody = Item.L();
        var PPbody = new PPBodyHandler().Parse(fpp.PPBody);
        var ppBodyLst = new List<Item>();
        foreach (var processCmd in PPbody)
        {
            
            var secsParaLst = new List<Item>();
            foreach (var para in processCmd.ProcessParameters)
            {
                
                var secsPara = VarStringToItem(para.DataType, para.Value);
                secsParaLst.Add(secsPara);

            }
            var secsPPcmd = L(
                              A(processCmd.CommandCode),
                              L(secsParaLst.ToArray()));
            
            ppBodyLst.Add(secsPPcmd);
        }
        var secsFpp = Item.L(
                            A(fpp.PPID),
                            A(fpp.EquipmentModelType),
                            A(fpp.SoftwareRevision),
                            L(ppBodyLst.ToArray()))
            ;
        
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
        }
        catch (Exception ex)
        {
            return 1;
        }

    }
    #endregion
}
