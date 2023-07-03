﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository.Model;
public class GemReport
{
    public int RPTID { get; set; }

    public IList<EventReportLink> EventReports { get; set; }
    public IList<ReportVariableLink> ReportVariables { get; set; }
}