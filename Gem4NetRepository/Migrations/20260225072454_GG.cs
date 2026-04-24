using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    /// <inheritdoc />
    public partial class GG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "System",
                table: "Variables");

            migrationBuilder.AddColumn<string>(
                name: "SourceDataType",
                table: "Variables",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceDataType",
                table: "Variables");

            migrationBuilder.AddColumn<bool>(
                name: "System",
                table: "Variables",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
