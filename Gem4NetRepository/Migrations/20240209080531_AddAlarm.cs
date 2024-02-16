using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddAlarm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ECID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DATAID = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnabledVid = table.Column<int>(type: "INTEGER", nullable: true),
                    Definition = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    Trigger = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ECID);
                });

            migrationBuilder.CreateTable(
                name: "FormattedProcessPrograms",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    PPID = table.Column<string>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PPBody = table.Column<string>(type: "TEXT", nullable: false),
                    Editor = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovalLevel = table.Column<string>(type: "TEXT", nullable: true),
                    SoftwareRevision = table.Column<string>(type: "TEXT", nullable: true),
                    EquipmentModelType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormattedProcessPrograms", x => new { x.ID, x.PPID });
                });

            migrationBuilder.CreateTable(
                name: "ProcessPrograms",
                columns: table => new
                {
                    PPID = table.Column<string>(type: "TEXT", nullable: false),
                    PPBody = table.Column<string>(type: "TEXT", nullable: false),
                    Format = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessPrograms", x => x.PPID);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    RPTID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Definition = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.RPTID);
                });

            migrationBuilder.CreateTable(
                name: "Variables",
                columns: table => new
                {
                    VID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataType = table.Column<string>(type: "TEXT", nullable: false),
                    Length = table.Column<int>(type: "INTEGER", nullable: true),
                    Unit = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Definition = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    VarType = table.Column<string>(type: "TEXT", nullable: false),
                    System = table.Column<bool>(type: "INTEGER", nullable: false),
                    MinValue = table.Column<string>(type: "TEXT", nullable: true),
                    MaxValue = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultValue = table.Column<string>(type: "TEXT", nullable: true),
                    ListSVID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variables", x => x.VID);
                });

            migrationBuilder.CreateTable(
                name: "EventReportLinks",
                columns: table => new
                {
                    RPTID = table.Column<int>(type: "INTEGER", nullable: false),
                    ECID = table.Column<int>(type: "INTEGER", nullable: false)
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
                    RPTID = table.Column<int>(type: "INTEGER", nullable: false),
                    VID = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "EventReportLinks");

            migrationBuilder.DropTable(
                name: "FormattedProcessPrograms");

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
