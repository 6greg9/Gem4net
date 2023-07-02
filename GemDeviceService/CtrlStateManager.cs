using Secs4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemDeviceService;
internal class CtrlStateManager
{
    ControlState CurrentState ;
    Task CtrlStateCheckTask;
    SecsGem _secsGem;
    CtrlStateManager(SecsGem secsGem) {
        _secsGem= secsGem;
    }
}
