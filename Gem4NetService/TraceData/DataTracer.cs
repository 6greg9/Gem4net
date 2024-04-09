using Secs4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Gem4Net.TraceData;
public class DataTracer
{
    /// <summary>
    /// trace request ID 
    /// </summary>
    public string TRID { get; private set; }
    /// <summary>
    /// DSPER: data sample period, hhmmss is always supported, A:8 hhmmsscc may be supported 
    /// </summary>
    public TimeSpan DataSamplePeriod { get; private set; }
    public int TotalSampleAmount { get; private set; }
    public int TotalSampleCounter { get; private set; }
    /// <summary>
    /// REPGSZ: report group size, 就是幾次Sample送出一次Report
    /// </summary>
    public int ReportGroupSize { get; private set; }
    /// <summary>
    /// SMPLN: sample number ?
    /// </summary>
    public int SampleNumber { get; private set; }

    public string DetailDescription { get; set; }

    public List<int> SampledVIDs { get; private set; }
    public event Func<List<int>,List<Item>> OnSample;
    public List<Item> SVsBag { get; private set; } = new List<Item>();
    public event Action<DataTracer> OnTraceEventSend;

    Timer SampleTimer { get; set; }
    public DataTracer(string trid, TimeSpan dataSamplePeriod, int totalSampleAmount,
        int reportGroupSize, List<int> sampleVIDs)
    {
        TRID = trid;
        TotalSampleAmount = totalSampleAmount;
        ReportGroupSize = reportGroupSize;
        SampleNumber = 0;
        SampledVIDs = sampleVIDs;
        DataSamplePeriod = dataSamplePeriod;

        SampleTimer = new Timer((e) => {
            TotalSampleCounter += 1;
            SampleNumber += 1;
            var SVs = OnSample?.Invoke(SampledVIDs);
            SVsBag?.AddRange(SVs);

            if(SampleNumber == ReportGroupSize)
            {
                SampleNumber = 0;
                TraceDataSend();
            }
            if(TotalSampleAmount == TotalSampleCounter)
            {
                SampleTimer?.Change(Timeout.Infinite, Timeout.Infinite); //Stop
            }
            else
            {
                SampleTimer?.Change(DataSamplePeriod, DataSamplePeriod*2); //do once after SamplePeriod
            }
            
        },
            null, 0, 0 ); // do once once imediately

    }
    void TraceDataSend()
    {
        OnTraceEventSend?.Invoke(this);
        SVsBag.Clear();
    }
}
