﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemDeviceService;
public enum ProcessingState
{
    INIT,
    IDLE,
    SETUP,
    READY,
    EXECUTING,
    PAUSE

}