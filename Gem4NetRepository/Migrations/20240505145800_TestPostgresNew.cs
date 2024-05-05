using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    /// <inheritdoc />
    public partial class TestPostgresNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alarms",
                columns: table => new
                {
                    ALID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ALCD = table.Column<int>(type: "integer", nullable: false),
                    AlarmStateVid = table.Column<int>(type: "integer", nullable: true),
                    DefaultAlarmState = table.Column<bool>(type: "boolean", nullable: false),
                    ALED = table.Column<bool>(type: "boolean", nullable: false),
                    AlarmEnableVid = table.Column<int>(type: "integer", nullable: true),
                    DefaultAlarmEnable = table.Column<bool>(type: "boolean", nullable: false),
                    ALTX = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarms", x => x.ALID);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ECID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DATAID = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    EnabledVid = table.Column<int>(type: "integer", nullable: true),
                    Definition = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    Trigger = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ECID);
                });

            migrationBuilder.CreateTable(
                name: "FormattedProcessProgramLogs",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    PPID = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PPBody = table.Column<string>(type: "text", nullable: false),
                    Editor = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ApprovalLevel = table.Column<string>(type: "text", nullable: true),
                    SoftwareRevision = table.Column<string>(type: "text", nullable: true),
                    EquipmentModelType = table.Column<string>(type: "text", nullable: true),
                    PPChangeStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FormattedProcessPrograms",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    PPID = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PPBody = table.Column<string>(type: "text", nullable: false),
                    Editor = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ApprovalLevel = table.Column<string>(type: "text", nullable: true),
                    SoftwareRevision = table.Column<string>(type: "text", nullable: true),
                    EquipmentModelType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ProcessProgramLogs",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    PPID = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PPBody = table.Column<string>(type: "text", nullable: false),
                    Editor = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ApprovalLevel = table.Column<string>(type: "text", nullable: true),
                    SoftwareRevision = table.Column<string>(type: "text", nullable: true),
                    EquipmentModelType = table.Column<string>(type: "text", nullable: true),
                    PPChangeStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ProcessPrograms",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    PPID = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PPBody = table.Column<string>(type: "text", nullable: false),
                    Editor = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ApprovalLevel = table.Column<string>(type: "text", nullable: true),
                    SoftwareRevision = table.Column<string>(type: "text", nullable: true),
                    EquipmentModelType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    RPTID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Definition = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.RPTID);
                });

            migrationBuilder.CreateTable(
                name: "Variables",
                columns: table => new
                {
                    VID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataType = table.Column<string>(type: "text", nullable: false),
                    Length = table.Column<int>(type: "integer", nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Definition = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    VarType = table.Column<string>(type: "text", nullable: false),
                    System = table.Column<bool>(type: "boolean", nullable: false),
                    MinValue = table.Column<string>(type: "text", nullable: true),
                    MaxValue = table.Column<string>(type: "text", nullable: true),
                    DefaultValue = table.Column<string>(type: "text", nullable: true),
                    ListSVID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variables", x => x.VID);
                });

            migrationBuilder.CreateTable(
                name: "EventReportLinks",
                columns: table => new
                {
                    RPTID = table.Column<int>(type: "integer", nullable: false),
                    ECID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventReportLinks", x => new { x.ECID, x.RPTID });
                    table.ForeignKey(
                        name: "FK_EventReportLinks_Events_ECID",
                        column: x => x.ECID,
                        principalTable: "Events",
                        principalColumn: "ECID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventReportLinks_Reports_RPTID",
                        column: x => x.RPTID,
                        principalTable: "Reports",
                        principalColumn: "RPTID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportVariableLinks",
                columns: table => new
                {
                    RPTID = table.Column<int>(type: "integer", nullable: false),
                    VID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportVariableLinks", x => new { x.VID, x.RPTID });
                    table.ForeignKey(
                        name: "FK_ReportVariableLinks_Reports_RPTID",
                        column: x => x.RPTID,
                        principalTable: "Reports",
                        principalColumn: "RPTID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportVariableLinks_Variables_VID",
                        column: x => x.VID,
                        principalTable: "Variables",
                        principalColumn: "VID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventReportLinks_RPTID",
                table: "EventReportLinks",
                column: "RPTID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportVariableLinks_RPTID",
                table: "ReportVariableLinks",
                column: "RPTID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alarms");

            migrationBuilder.DropTable(
                name: "EventReportLinks");

            migrationBuilder.DropTable(
                name: "FormattedProcessProgramLogs");

            migrationBuilder.DropTable(
                name: "FormattedProcessPrograms");

            migrationBuilder.DropTable(
                name: "ProcessProgramLogs");

            migrationBuilder.DropTable(
                name: "ProcessPrograms");

            migrationBuilder.DropTable(
                name: "ReportVariableLinks");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Variables");
        }
    }
}
