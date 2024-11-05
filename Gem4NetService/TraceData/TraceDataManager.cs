using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem4NetRepository;
using Secs4Net;
namespace Gem4Net.TraceData;
public class TraceDataManager
{
    GemRepository _repo { get; set; }
    SecsGem _secsGem { get; set; }
    GemEqpService _gemEqpService { get; set; }
    List<DataTracer> _tracerList { get; set; } = new();

    public TimeSpan MinSamplePeriod { get; set; } = TimeSpan.FromMilliseconds(500);
    //試試Tasks.DataFlow
    public TraceDataManager(SecsGem secsGem, GemEqpService gemService, GemRepository repo)
    {
        _repo = repo;
        _secsGem = secsGem;
        _gemEqpService = gemService;
    }
    /// <summary>
    /// TIAACK Format: B:1 :  0 - ok, 1 - too many SVIDs, 2 - no more traces allowed, 3 - invalid period, 
    /// 4 - unknown SVID, 5 - bad REPGSZ
    /// </summary>
    /// <returns></returns>
    public async Task<int> TraceInitialize((string trid, TimeSpan dataSamplePeriod, int totalSampleAmount,
        int reportGroupSize, List<int> sampleVIDs) traceInit)
    {
        if (_tracerList.Where(tr => tr.TRID == traceInit.trid).Any() == true)
        {
            //已有重複TRID
            return 2;
        }
        if (traceInit.dataSamplePeriod < MinSamplePeriod) // SamplePeriod限制
        {
            return 3;
        }
        var variableLstItem = await _repo.GetSvNameListAll();
        var variableLst = variableLstItem.Items.ToList();
        foreach (var vid in traceInit.sampleVIDs) // 檢查不存在VID
        {
            if (variableLst.Where(item => item.Items[0].FirstValue<int>() == vid).Any() == false)
            {
                return 4;
            }
        }

        var newTrace = new DataTracer(traceInit.trid, traceInit.dataSamplePeriod, 
            traceInit.totalSampleAmount, traceInit.reportGroupSize, traceInit.sampleVIDs);
        var format = await _gemEqpService.GetSecsTimeFormat();
        newTrace.TimeFormat = format.FirstValue<int>();
        newTrace.OnSample += HandleSampleForTracer;
        newTrace.OnTraceEventSend += HandleTraceEventSend;
        _tracerList.Add(newTrace);
        return 0;
    }

    public int TraceTerminate(string trid)
    {
        if (_tracerList.Where(tr => tr.TRID == trid).Any() == true)
        {
            _tracerList.Remove(_tracerList.Where(tr => tr.TRID == trid).First());
            return 0;
        }
        //已有TRID不存在
        return 1;
    }
    public int TraceTerminateAll()
    {
        if(_tracerList.Any())
            _tracerList.RemoveAll(tr => true);
        return 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    async Task< List<Item?> > HandleSampleForTracer(List<int> lstVid)
    {
        var svListItem = await _repo.GetSvList(lstVid);
        return svListItem.Items.ToList();
    }
    /// <summary>
    /// S6F1
    /// </summary>
    /// <param name="sender"></param>
    void HandleTraceEventSend(DataTracer sender)
    {
        var stime = () =>
        {
            if(sender.TimeFormat == 0)
            {
                return DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            }else if(sender.TimeFormat == 1)
            {
                return DateTime.UtcNow.ToString("yyyyMMddHHmmssff");
            }
            else
            {
                return DateTime.UtcNow.ToString("yyyyMMddHHmmssff");
            }
        };
        var s6f1Item = Item.L(
            Item.A(sender.TRID),
            Item.U4((byte)sender.SampleNumber),
            Item.A( stime()),
            Item.L( sender.SVsBag.ToArray())

        );
        var secsMsg = new SecsMessage(6, 1)
        {
            SecsItem = s6f1Item,
        };

        try
        {
            _secsGem.SendAsync(secsMsg);
        }
        catch (Exception ex) { 
            
        }
        
        if( sender.TotalSampleAmount== sender.TotalSampleCounter) // 清理廢棄物
        {
            _= Task.Run(async () => { 
                await Task.Delay(1000*5);
                _tracerList.RemoveAt(_tracerList.FindIndex(tr=>tr.TRID==sender.TRID));
            });
            
        }

    }
}

