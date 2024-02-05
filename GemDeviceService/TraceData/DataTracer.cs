using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GemDeviceService.TraceData;
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
    public int SampleCount { get; private set; }
    /// <summary>
    /// REPGSZ: report group size, 就是幾次Sample送出一次Report
    /// </summary>
    public int ReportGroupSize { get; private set; }

    public string DetailDescription { get; set; }

    public List<int> SampledVIDs { get; private set; }

    Timer SampleTimer { get; set; }
    public DataTracer(string trid, TimeSpan dataSamplePeriod, int totalSampleAmount,
        int reportGroupSize, List<int> sampleVIDs)
    {
        TRID = trid;
        TotalSampleAmount = totalSampleAmount;
        ReportGroupSize = reportGroupSize;
        SampledVIDs = sampleVIDs;

    }
}
