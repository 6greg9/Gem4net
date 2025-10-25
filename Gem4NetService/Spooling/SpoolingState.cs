using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4Net.Spooling;

public enum SpoolingState
{
    SPOOL_INACTIVE,
    SPOOL_ACTIVE,

    POWER_OFF // ?

}
public enum SpoolLoadState
{
    SPOOL_NOT_FULL,
    SPOOL_FULL
}
public enum SpoolUnloadState { 
    NO_SPOOL_OUTPUT,
    TRANSMIT_SPOOL,
    PURGE_SPOOL
}
