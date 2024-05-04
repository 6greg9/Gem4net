using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    /// <inheritdoc />
    public partial class ProcessProgramLogWithNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "LogId",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "PPChangeStatus",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropColumn(
                name: "LogId",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropColumn(
                name: "PPChangeStatus",
                table: "FormattedProcessPrograms");

            migrationBuilder.CreateTable(
                name: "FormattedProcessProgramLogs",
                columns: table => new
                {
                    PPID = table.Column<string>(type: "TEXT", nullable: false),
                    LogId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PPChangeStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormattedProcessProgramLogs", x => x.PPID);
                    table.ForeignKey(
                        name: "FK_FormattedProcessProgramLogs_FormattedProcessPrograms_PPID",
                        column: x => x.PPID,
                        principalTable: "FormattedProcessPrograms",
                        principalColumn: "PPID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessProgramLogs",
                columns: table => new
                {
                    PPID = table.Column<string>(type: "TEXT", nullable: false),
                    LogId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PPChangeStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessProgramLogs", x => x.PPID);
                    table.ForeignKey(
                        name: "FK_ProcessProgramLogs_ProcessPrograms_PPID",
                        column: x => x.PPID,
                        principalTable: "ProcessPrograms",
                        principalColumn: "PPID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormattedProcessProgramLogs");

            migrationBuilder.DropTable(
                name: "ProcessProgramLogs");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ProcessPrograms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "LogId",
                table: "ProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PPChangeStatus",
                table: "ProcessPrograms",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "FormattedProcessPrograms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "LogId",
                table: "FormattedProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PPChangeStatus",
                table: "FormattedProcessPrograms",
                type: "INTEGER",
                nullable: true);
        }
    }
}
