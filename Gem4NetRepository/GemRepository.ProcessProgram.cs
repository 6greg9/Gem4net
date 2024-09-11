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
    
    #region No Format
    public async Task<IEnumerable<ProcessProgram>> GetProcessProgram(string PPID)
    {
        return await LockGemRepo<IEnumerable<ProcessProgram>>(() =>
        {
            var pps = _context.ProcessPrograms
                    .Where(pp => pp.PPID == PPID).ToList();
            return pps;
        });

    }
    public async Task<IEnumerable<ProcessProgram>> GetProcessProgramAll()
    {
        await semSlim.WaitAsync();
        try
        {

            using (_context = new GemDbContext(_config))
            {

                var PPs = _context.ProcessPrograms
                .ToList();
                return PPs;
            }
        }

        finally { semSlim.Release(); }
 
    }
    public async Task<int> CreateProcessProgram(ProcessProgram pp)
    {

        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {
                pp.UpdateTime = DateTime.UtcNow;
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
                    var ppLog = Mapper.Map<ProcessProgramLog>(pp);

                    ppLog.PPChangeStatus = ppChangeStatus;


                    _context.ProcessProgramLogs.Add(ppLog);
                }

            }
        }
        finally { semSlim.Release(); }

        return 0;
    }

    public async Task<int> DeleteProcessProgram(List<string> ppids)
    {
        await semSlim.WaitAsync();
        try
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
        finally { semSlim.Release(); }
    }
    public async Task<int> DeletedProcessProgramAll()
    {
        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {

                _context.RemoveRange(_context.ProcessPrograms);
                _context.SaveChanges();
                return 0;
            }

        }
        finally { semSlim.Release(); }
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
    public async Task<IEnumerable<FormattedProcessProgram>> GetFormattedProcessProgram(string PPID)
    {
        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {

                var pps = _context.FormattedProcessPrograms
                    .Where(pp => pp.PPID == PPID).ToList();
                return pps;
            }

        }

        finally { semSlim.Release(); }
        
    }
    public async Task<IEnumerable<FormattedProcessProgram>> GetFormattedPPAll()
    {
        await semSlim.WaitAsync();
        try
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

        finally { semSlim.Release(); }
        
    }
    public async Task<int> CreateFormattedProcessProgram(FormattedProcessProgram fpp)
    {

        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {

                fpp.UpdateTime = DateTime.UtcNow;
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
                    LogFPPChanged(2);
                }
                else
                {
                    _context.FormattedProcessPrograms.Add(fpp);
                    LogFPPChanged(1);

                }

                _ = _context.SaveChanges();

                /// <summary>
                /// For PPChangeStatus, 1 Created, 2 Edited, 3 Deleted , 4-64 Reserved
                /// </summary>
                void LogFPPChanged(int ppChangeStatus)
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
    /// for S7F17
    /// </summary>
    /// <param name="ppids"></param>
    /// <returns></returns>
    public async Task<int> DeleteFormattedProcessProgram(List<string> ppids)
    {
        await semSlim.WaitAsync();
        try
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
                    fppLog.UpdateTime = DateTime.UtcNow;
                    
                    fppLog.PPChangeStatus = 3;
                    _context.FormattedProcessProgramLogs.Add(fppLog);
                }
                _context.SaveChanges();
                return 1;
            }

        }
        finally { semSlim.Release(); }
        
    }
    public async Task<int> DeleteFormattedPPAll()
    {
        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {
                _ = _context.FormattedProcessPrograms.ForEachAsync(fpp =>
                {
                    _context.FormattedProcessPrograms.Remove(fpp);

                    //Delete Log
                    var fppLog = Mapper.Map<FormattedProcessProgramLog>(fpp);
                    fppLog.UpdateTime = DateTime.UtcNow;
                    fppLog.LogId = Guid.NewGuid();
                    fppLog.PPChangeStatus = 3;
                    _context.FormattedProcessProgramLogs.Add(fppLog);
                });

                _context.SaveChanges();
                return 0;
            }

        }
        finally { semSlim.Release(); }
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

            var PPCommands = new List<ProcessCommand>();
            foreach (var processCmd in secsFpp.Items[3].Items)
            {
                string cmdCode = "";
                switch (processCmd.Items[0].Format)
                {
                    case SecsFormat.ASCII:
                        cmdCode = processCmd.Items[0].GetString();
                        break;
                    case SecsFormat.I2:
                        cmdCode = processCmd.Items[0].FirstValue<short>().ToString();
                        break;
                    case SecsFormat.U2:
                        cmdCode = processCmd.Items[0].FirstValue<ushort>().ToString();
                        break;
                    case SecsFormat.I4:
                        cmdCode = processCmd.Items[0].FirstValue<int>().ToString();
                        break;
                    case SecsFormat.U4:
                        cmdCode = processCmd.Items[0].FirstValue<uint>().ToString();
                        break;

                }
                var pCmd = new ProcessCommand { CommandCode = cmdCode };
                var paras = processCmd.Items[1];
                foreach (var para in paras.Items) // 這個要很注意客製
                {
                    var p = new ProcessParameter();
                    p.DataType = para.Format.ToString();
                    p.Value = ItemToVarString(para);
                    pCmd.ProcessParameters.Add(p);
                }
                PPCommands.Add(pCmd);
                
            }
            fpp.PPBody = JsonSerializer.Serialize(PPCommands);
            return 0;
        }
        catch (Exception ex)
        {
            return 1;
        }

    }
    #endregion
}
