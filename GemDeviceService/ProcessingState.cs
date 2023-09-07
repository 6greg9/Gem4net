using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemDeviceService;
public enum ProcessingState //必須自定義, 而不適用enum
{
    INIT,
    IDLE,
    SETUP,
    READY,
    EXECUTING,
    PAUSE

}
