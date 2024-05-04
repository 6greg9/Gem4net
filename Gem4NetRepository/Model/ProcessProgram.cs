using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.Model;
public class ProcessProgram
{
    public Guid ID { get; set; }
    /// <summary>
    /// or PPNAME ?
    /// </summary>
    public string PPID { get; set; }

    //更多資訊, Recipe Specifier

    public DateTime UpdateTime { get; set; }
    
    /// <summary>
    /// JSON !!!
    /// </summary>
    public string PPBody { get; set; }

    public string? Editor { get; set; }
    public string? Description { get; set; }
    public string? ApprovalLevel { get; set; }
    public string? SoftwareRevision { get; set; }
    public string? EquipmentModelType { get; set; }
}

public class ProcessProgramLog
{
    public Guid LogId { get; set; }
    /// <summary>
    /// For PPChangeStatus, 1 Created, 2 Edited, 3 Deleted , 4-64 Reserved
    /// </summary>
    public int PPChangeStatus { get; set; }
}
