using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GemVarRepository.Migrations
{
    /// <inheritdoc />
    public partial class _20231126ProcessProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Variables");

            migrationBuilder.AddColumn<int>(
                name: "ListSVID",
                table: "Variables",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormattedProcessProgram",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PPID = table.Column<string>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormattedProcessProgram", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessProgram",
                columns: table => new
                {
                    PPID = table.Column<string>(type: "TEXT", nullable: false),
                    PPBody = table.Column<string>(type: "TEXT", nullable: false),
                    Format = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessProgram", x => x.PPID);
                });

            migrationBuilder.CreateTable(
                name: "ProcessParameter",
                columns: table => new
                {
                    ProcessProgramId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProcessCommandCode = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DataType = table.Column<string>(type: "TEXT", nullable: false),
                    Length = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    Definition = table.Column<string>(type: "TEXT", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessParameter", x => new { x.ProcessProgramId, x.ProcessCommandCode, x.Name });
                    table.ForeignKey(
                        name: "FK_ProcessParameter_FormattedProcessProgram_ProcessProgramId",
                        column: x => x.ProcessProgramId,
                        principalTable: "FormattedProcessProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessParameter");

            migrationBuilder.DropTable(
                name: "ProcessProgram");

            migrationBuilder.DropTable(
                name: "FormattedProcessProgram");

            migrationBuilder.DropColumn(
                name: "ListSVID",
                table: "Variables");

            migrationBuilder.AddColumn<DateTime>(
                name: "Version",
                table: "Variables",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
