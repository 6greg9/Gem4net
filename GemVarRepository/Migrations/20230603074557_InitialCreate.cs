using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GemVarRepository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Definition = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    Trigger = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ECID);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    RPTID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
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
                    Length = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Definition = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    VarType = table.Column<string>(type: "TEXT", nullable: false),
                    System = table.Column<bool>(type: "INTEGER", nullable: false),
                    MinValue = table.Column<string>(type: "TEXT", nullable: false),
                    MaxValue = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultValue = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variables", x => x.VID);
                });

            migrationBuilder.CreateTable(
                name: "EventReportRelation",
                columns: table => new
                {
                    RPTID = table.Column<int>(type: "INTEGER", nullable: false),
                    ECID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventReportRelation", x => new { x.ECID, x.RPTID });
                    table.ForeignKey(
                        name: "FK_EventReportRelation_Events_ECID",
                        column: x => x.ECID,
                        principalTable: "Events",
                        principalColumn: "ECID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventReportRelation_Reports_RPTID",
                        column: x => x.RPTID,
                        principalTable: "Reports",
                        principalColumn: "RPTID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventReportRelation_RPTID",
                table: "EventReportRelation",
                column: "RPTID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventReportRelation");

            migrationBuilder.DropTable(
                name: "Variables");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
