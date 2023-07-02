﻿// <auto-generated />
using GemVarRepository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GemVarRepository.Migrations
{
    [DbContext(typeof(GemVarContext))]
    [Migration("20230603075230_Test")]
    partial class Test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("GemVarRepository.Model.EventReportLink", b =>
                {
                    b.Property<int>("ECID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RPTID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ECID", "RPTID");

                    b.HasIndex("RPTID");

                    b.ToTable("EventReportRelation");
                });

            modelBuilder.Entity("GemVarRepository.Model.GemEvent", b =>
                {
                    b.Property<int>("ECID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Definition")
                        .HasColumnType("TEXT");

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

            modelBuilder.Entity("GemVarRepository.Model.GemReport", b =>
                {
                    b.Property<int>("RPTID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("RPTID");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("GemVarRepository.Model.GemVariable", b =>
                {
                    b.Property<int>("VID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DefaultValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Definition")
                        .HasColumnType("TEXT");

                    b.Property<int>("Length")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaxValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MinValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.Property<bool>("System")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Unit")
                        .IsRequired()
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

            modelBuilder.Entity("GemVarRepository.Model.EventReportLink", b =>
                {
                    b.HasOne("GemVarRepository.Model.GemEvent", "Event")
                        .WithMany("ReportEvents")
                        .HasForeignKey("ECID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GemVarRepository.Model.GemReport", "Report")
                        .WithMany("EventReports")
                        .HasForeignKey("RPTID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Report");
                });

            modelBuilder.Entity("GemVarRepository.Model.GemEvent", b =>
                {
                    b.Navigation("ReportEvents");
                });

            modelBuilder.Entity("GemVarRepository.Model.GemReport", b =>
                {
                    b.Navigation("EventReports");
                });
#pragma warning restore 612, 618
        }
    }
}
