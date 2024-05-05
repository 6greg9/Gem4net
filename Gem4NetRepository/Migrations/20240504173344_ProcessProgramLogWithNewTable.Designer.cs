﻿// <auto-generated />
using System;
using Gem4NetRepository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    [DbContext(typeof(GemDbContext))]
    [Migration("20240504173344_ProcessProgramLogWithNewTable")]
    partial class ProcessProgramLogWithNewTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("Gem4NetRepository.Model.EventReportLink", b =>
                {
                    b.Property<int>("ECID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RPTID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ECID", "RPTID");

                    b.HasIndex("RPTID");

                    b.ToTable("EventReportLinks");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.FormattedProcessProgram", b =>
                {
                    b.Property<string>("PPID")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApprovalLevel")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Editor")
                        .HasColumnType("TEXT");

                    b.Property<string>("EquipmentModelType")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("PPBody")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SoftwareRevision")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("PPID");

                    b.ToTable("FormattedProcessPrograms");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Gem4NetRepository.Model.GemAlarm", b =>
                {
                    b.Property<int>("ALID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ALCD")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ALED")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ALTX")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("AlarmEnableVid")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AlarmStateVid")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("DefaultAlarmEnable")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("DefaultAlarmState")
                        .HasColumnType("INTEGER");

                    b.HasKey("ALID");

                    b.ToTable("Alarms");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.GemEvent", b =>
                {
                    b.Property<int>("ECID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DATAID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Definition")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EnabledVid")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.Property<string>("Trigger")
                        .HasColumnType("TEXT");

                    b.HasKey("ECID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.GemReport", b =>
                {
                    b.Property<int>("RPTID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Definition")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.HasKey("RPTID");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.GemVariable", b =>
                {
                    b.Property<int>("VID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DefaultValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("Definition")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Length")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ListSVID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaxValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("MinValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.Property<bool>("System")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Unit")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VarType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("VID");

                    b.ToTable("Variables");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.ProcessProgram", b =>
                {
                    b.Property<string>("PPID")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApprovalLevel")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Editor")
                        .HasColumnType("TEXT");

                    b.Property<string>("EquipmentModelType")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("PPBody")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SoftwareRevision")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("PPID");

                    b.ToTable("ProcessPrograms");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Gem4NetRepository.Model.ReportVariableLink", b =>
                {
                    b.Property<int>("VID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RPTID")
                        .HasColumnType("INTEGER");

                    b.HasKey("VID", "RPTID");

                    b.HasIndex("RPTID");

                    b.ToTable("ReportVariableLinks");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.FormattedProcessProgramLog", b =>
                {
                    b.HasBaseType("Gem4NetRepository.Model.FormattedProcessProgram");

                    b.Property<Guid>("LogId")
                        .HasColumnType("TEXT");

                    b.Property<int>("PPChangeStatus")
                        .HasColumnType("INTEGER");

                    b.ToTable("FormattedProcessProgramLogs", (string)null);
                });

            modelBuilder.Entity("Gem4NetRepository.Model.ProcessProgramLog", b =>
                {
                    b.HasBaseType("Gem4NetRepository.Model.ProcessProgram");

                    b.Property<Guid>("LogId")
                        .HasColumnType("TEXT");

                    b.Property<int>("PPChangeStatus")
                        .HasColumnType("INTEGER");

                    b.ToTable("ProcessProgramLogs", (string)null);
                });

            modelBuilder.Entity("Gem4NetRepository.Model.EventReportLink", b =>
                {
                    b.HasOne("Gem4NetRepository.Model.GemEvent", "Event")
                        .WithMany("ReportEvents")
                        .HasForeignKey("ECID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gem4NetRepository.Model.GemReport", "Report")
                        .WithMany("EventReports")
                        .HasForeignKey("RPTID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Report");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.ReportVariableLink", b =>
                {
                    b.HasOne("Gem4NetRepository.Model.GemReport", "Report")
                        .WithMany("ReportVariables")
                        .HasForeignKey("RPTID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gem4NetRepository.Model.GemVariable", "Variable")
                        .WithMany("ReportVariables")
                        .HasForeignKey("VID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Report");

                    b.Navigation("Variable");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.FormattedProcessProgramLog", b =>
                {
                    b.HasOne("Gem4NetRepository.Model.FormattedProcessProgram", null)
                        .WithOne()
                        .HasForeignKey("Gem4NetRepository.Model.FormattedProcessProgramLog", "PPID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gem4NetRepository.Model.ProcessProgramLog", b =>
                {
                    b.HasOne("Gem4NetRepository.Model.ProcessProgram", null)
                        .WithOne()
                        .HasForeignKey("Gem4NetRepository.Model.ProcessProgramLog", "PPID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gem4NetRepository.Model.GemEvent", b =>
                {
                    b.Navigation("ReportEvents");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.GemReport", b =>
                {
                    b.Navigation("EventReports");

                    b.Navigation("ReportVariables");
                });

            modelBuilder.Entity("Gem4NetRepository.Model.GemVariable", b =>
                {
                    b.Navigation("ReportVariables");
                });
#pragma warning restore 612, 618
        }
    }
}