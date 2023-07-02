using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GemVarRepository.Migrations
{
    /// <inheritdoc />
    public partial class Update_202306222200 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportVariableLink",
                columns: table => new
                {
                    RPTID = table.Column<int>(type: "INTEGER", nullable: false),
                    VID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportVariableLink", x => new { x.VID, x.RPTID });
                    table.ForeignKey(
                        name: "FK_ReportVariableLink_Reports_RPTID",
                        column: x => x.RPTID,
                        principalTable: "Reports",
                        principalColumn: "RPTID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportVariableLink_Variables_VID",
                        column: x => x.VID,
                        principalTable: "Variables",
                        principalColumn: "VID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportVariableLink_RPTID",
                table: "ReportVariableLink",
                column: "RPTID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportVariableLink");
        }
    }
}
