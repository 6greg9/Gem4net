using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemDeviceService;
public enum ControlState
{
    EQUIPMENT_OFF_LINE,
    HOST_OFF_LINE,
    ATTEMPT_ON_LINE,
    LOCAL,
    REMOTE
}
