﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GemVarRepository.Model;
public class FormattedProcessProgram
{
    public Guid ID { get; set; }
    /// <summary>
    /// or PPNAME ?
    /// </summary>
    public string PPID {  get; set; }

    //更多資訊, Recipe Specifier

    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// For PPChangeStatus, 1 Created, 2 Edited, 3 Deleted , 4-64 Reserved
    /// </summary>
    public int Status { get; set; }
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
public class PPBody
{
    public List<ProcessCommand>? ProcessCommands { get; set; } = new();
}
public class ProcessCommand
{
    public string CommandCode { get; set; }
    public List<ProcessParameter>? ProcessParameters { get; set; } = new();//JSON column
}
public class ProcessParameter
{
    public string DataType { get; set; }
    public int Length { get; set; }
    public string Unit { get; set; }
    public string Value { get; set; }
    public string Name { get; set; }
    public string? Definition { get; set; }
    public string? Remark { get; set; }

}

public class PPBodyHandler : SqlMapper.TypeHandler<PPBody>
{
    public override PPBody Parse(object value)
    {
        return JsonSerializer.Deserialize<PPBody>((string)value);
    }

    public override void SetValue(IDbDataParameter parameter, PPBody value)
    {
        parameter.Value = JsonSerializer.Serialize(value);
    }
}
