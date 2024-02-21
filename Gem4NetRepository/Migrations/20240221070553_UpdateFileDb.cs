using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFileDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FormattedProcessPrograms",
                table: "FormattedProcessPrograms");

            migrationBuilder.AlterColumn<string>(
                name: "ALTX",
                table: "Alarms",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormattedProcessPrograms",
                table: "FormattedProcessPrograms",
                column: "PPID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FormattedProcessPrograms",
                table: "FormattedProcessPrograms");

            migrationBuilder.AlterColumn<bool>(
                name: "ALTX",
                table: "Alarms",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormattedProcessPrograms",
                table: "FormattedProcessPrograms",
                columns: new[] { "ID", "PPID" });
        }
    }
}
