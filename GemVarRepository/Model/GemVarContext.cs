﻿using System;
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
    public DbSet<EventReportLink> EventReportRelation { get; set; }
    public string DbPath { get; }
    public GemVarContext()
    {
        var folder = Environment.SpecialFolder.MyDocuments;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "GemVariablesDb.sqlite");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

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
    }
}