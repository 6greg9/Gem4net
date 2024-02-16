using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4Net.Control;
public enum ControlState
{
    EQUIPMENT_OFF_LINE,
    HOST_OFF_LINE,
    ATTEMPT_ON_LINE,
    LOCAL,
    REMOTE
}
