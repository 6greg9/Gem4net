using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GemVarRepository.Model;
public class GemVarContext : DbContext
{

    public DbSet<GemVariable> Variables { get; set; }
    public DbSet<GemEvent> Events { get; set; }
    public DbSet<GemReport> Reports { get; set; }
    public DbSet<EventReportLink> EventReportLinks { get; set; }
    public DbSet<ReportVariableLink> ReportVariableLinks { get; set; }
    public DbSet<ProcessProgram> ProcessPrograms { get; set; }
    public DbSet<FormattedProcessProgram> FormattedProcessPrograms { get; set; }

    public string DbPath { get; }
    public GemVarContext(string dbPath)
    {
        var folder = Environment.SpecialFolder.MyDocuments;
        var path = Environment.GetFolderPath(folder);
        //DbPath = System.IO.Path.Join(path, "GemVariablesDb.sqlite");
        DbPath = System.IO.Path.Join(path, dbPath);
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");//EF8才支持sqlite

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GemEvent>().HasKey(sc => sc.ECID);
        modelBuilder.Entity<GemReport>().HasKey(sc => sc.RPTID);
        modelBuilder.Entity<GemVariable>().HasKey(sc => sc.VID);

        modelBuilder.Entity<EventReportLink>().HasKey(sc => new { sc.ECID, sc.RPTID });

        modelBuilder.Entity<EventReportLink>()
            .HasOne<GemEvent>(sc => sc.Event)
            .WithMany(s => s.ReportEvents)
            .HasForeignKey(sc => sc.ECID);//


        modelBuilder.Entity<EventReportLink>()
            .HasOne<GemReport>(sc => sc.Report)
            .WithMany(s => s.EventReports)
            .HasForeignKey(sc => sc.RPTID);

        modelBuilder.Entity<ReportVariableLink>().HasKey(sc => new { sc.VID, sc.RPTID });

        modelBuilder.Entity<ReportVariableLink>()
            .HasOne<GemReport>(sc => sc.Report)
            .WithMany(s => s.ReportVariables)
            .HasForeignKey(sc => sc.RPTID);

        modelBuilder.Entity<ReportVariableLink>()
            .HasOne<GemVariable>(sc => sc.Variable)
            .WithMany(s => s.ReportVariables)
            .HasForeignKey(sc => sc.VID);

        modelBuilder.Entity<FormattedProcessProgram>().HasKey(sc => new { sc.ID, sc.PPID });
        //modelBuilder.Entity<ProcessParameter>()
        //    .HasKey(sc => new { sc.ProcessProgramId, sc.ProcessCommandCode, sc.Name });
        //modelBuilder.Entity<ProcessParameter>()
        //    .HasOne<FormattedProcessProgram>(sc => sc.ProcessProgramVersion)
        //    .WithMany(sc => sc.ProcessParameters)
        //    .HasForeignKey(sc => sc.ProcessProgramId);

        modelBuilder.Entity<ProcessProgram>().HasKey(sc => sc.PPID);

        //modelBuilder.Entity<FormattedProcessProgram>()
        //    .HasKey(sc => new { sc.PPID });//內部加上ComplexType標籤
    }
}
