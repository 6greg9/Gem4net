using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemVarRepository;
namespace GemDeviceService.TraceData;
public class TraceDataManager
{
    GemRepository _repo {  get; set; }
    List<DataTracer> _tracerList { get; set; } = new();
    //試試Tasks.DataFlow
    public TraceDataManager(GemRepository repo) { 
        _repo = repo;
    }
}

