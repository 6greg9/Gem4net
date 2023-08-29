using GemVarRepository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
namespace GemVarRepository;
public class Program
{
    public Program()
    {
        using var db = new GemVarContext();
        var aaa =    db.Variables.Where(v=> v.Name == "SV");
        //var Reports = db.Events.Where( e => e.ECID == 1)
        //    .Include( e => e.ReportEvents );
        var ReportIDs = db.EventReportLinks.Where(e => e.ECID== 1)
            .Include(e=>e.Report).Select( rpt => rpt.RPTID);

        //var ReportData = db.Reports.Where( rpt => ReportIDs.Contains( rpt.RPTID))
        //    .Include(e=>e.v)
        var ReportIDss = db.Events//.Where( evnt=> evnt.ECID== 1)
            .Include(evnt=>evnt.ReportEvents)
            .ThenInclude(RptEvnt=>RptEvnt.Report)//JOIN
            .Select(evnt=> evnt.ReportEvents.Where( rel=> rel.ECID==1).FirstOrDefault());//
            
            


    }
}
