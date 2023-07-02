using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemDeviceService;
public enum CommunicationState
{
    DISABLED,
    WAIT_CR_FROM_HOST,
    WAIT_DELAY,
    WAIT_CRA,
    COMMUNICATING

}
