using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    /// <inheritdoc />
    public partial class TestPostgresNewNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessPrograms",
                table: "ProcessPrograms",
                column: "LogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessProgramLogs",
                table: "ProcessProgramLogs",
                column: "LogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormattedProcessPrograms",
                table: "FormattedProcessPrograms",
                column: "LogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormattedProcessProgramLogs",
                table: "FormattedProcessProgramLogs",
                column: "LogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessPrograms",
                table: "ProcessPrograms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessProgramLogs",
                table: "ProcessProgramLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormattedProcessPrograms",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormattedProcessProgramLogs",
                table: "FormattedProcessProgramLogs");
        }
    }
}
