using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Gem4NetRepository.Model;
public class GemDbContext : DbContext
{
    //這裡屬性名稱會影響在資料庫裡的名稱
    public DbSet<GemVariable> Variables { get; set; }
    public DbSet<GemEvent> Events { get; set; }
    public DbSet<GemReport> Reports { get; set; }
    public DbSet<EventReportLink> EventReportLinks { get; set; }
    public DbSet<ReportVariableLink> ReportVariableLinks { get; set; }
    public DbSet<ProcessProgram> ProcessPrograms { get; set; }
    public DbSet<ProcessProgramLog> ProcessProgramLogs { get; set; }
    public DbSet<FormattedProcessProgram> FormattedProcessPrograms { get; set; }
    public DbSet<FormattedProcessProgramLog> FormattedProcessProgramLogs { get; set; }
    public DbSet<GemAlarm> Alarms { get; set; }
    public string DbPath { get; private set; }
    //IConfiguration configuration { get; set; }
    public GemDbContext(string dbFile = "GemSqliteDb")
    {
        var folder = Environment.SpecialFolder.MyDocuments;
        var path = Environment.GetFolderPath(folder);

        //參數方式
        //var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        //var configuration = builder.Build();
        //DbPath = configuration.GetConnectionString(dbFile)
        //    ?? System.IO.Path.Join(path, "GemVariablesDb.sqlite");

        //migration用的
        DbPath = System.IO.Path.Join(path, "GemVariablesDb.sqlite");

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
        modelBuilder.Entity<GemAlarm>().HasKey(sc => sc.ALID);
        modelBuilder.Entity<ProcessProgram>().UseTpcMappingStrategy().HasKey(sc => sc.PPID);
        modelBuilder.Entity<ProcessProgramLog>().UseTpcMappingStrategy().ToTable("ProcessProgramLogs"); 
        modelBuilder.Entity<FormattedProcessProgram>().UseTpcMappingStrategy().HasKey(sc =>  sc.PPID );
        modelBuilder.Entity<FormattedProcessProgramLog>().UseTpcMappingStrategy().ToTable("FormattedProcessProgramLogs");

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

        
        //modelBuilder.Entity<ProcessParameter>()
        //    .HasKey(sc => new { sc.ProcessProgramId, sc.ProcessCommandCode, sc.Name });
        //modelBuilder.Entity<ProcessParameter>()
        //    .HasOne<FormattedProcessProgram>(sc => sc.ProcessProgramVersion)
        //    .WithMany(sc => sc.ProcessParameters)
        //    .HasForeignKey(sc => sc.ProcessProgramId);

        
        
        //modelBuilder.Entity<FormattedProcessProgram>()
        //    .HasKey(sc => new { sc.PPID });//內部加上ComplexType標籤
    }
}
