using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4Net;
public class GemEqpAppConfig
{
    /// <summary>
    /// MDLN     Format: A:20 (invariant)
    /// </summary>
    public string ModelType { get; set; }
    /// <summary>
    /// SOFTREV     Format: A:20
    /// </summary>
    public string SoftwareVersion { get; set; }
}
