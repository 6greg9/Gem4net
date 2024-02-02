using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemVarRepository;
namespace GemDeviceService.TraceData;
public class TraceDataService
{
    GemRepository _repo {  get; set; }
    List<DataTracer> _tracerList { get; set; } = new();
    //試試Tasks.DataFlow
    public TraceDataService(GemRepository repo) { 
        _repo = repo;
    }
}

