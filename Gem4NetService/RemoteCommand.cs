using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Gem4Net;
public class RemoteCommand
{
    public string Name { get; set; }
    public List<CommandParameter> Parameters { get; set; } = new();
    /// <summary>
    /// 0 - ok, completed, 1 - invalid command, 2 - cannot do now, 3 - parameter error, 4 - initiated for asynchronous completion, 5 - rejected, already in desired condition, 6 - invalid object
    /// </summary>
    public int HCACK {  get; set; }

}
public class CommandParameter
{
    public string Name { get; set; }
    public object Value { get; set; }
    /// <summary>
    /// 1 - unknown CPNAME, 2 - illegal value for CPVAL, 3 - illegal format for CPVAL
    /// </summary>
    public int CPACK {  get; set; }
}