using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GemVarRepository.Migrations
{
    /// <inheritdoc />
    public partial class AAdddAlarm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GemAlarm",
                table: "GemAlarm");

            migrationBuilder.RenameTable(
                name: "GemAlarm",
                newName: "Alarms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alarms",
                table: "Alarms",
                column: "ALID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Alarms",
                table: "Alarms");

            migrationBuilder.RenameTable(
                name: "Alarms",
                newName: "GemAlarm");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GemAlarm",
                table: "GemAlarm",
                column: "ALID");
        }
    }
}
