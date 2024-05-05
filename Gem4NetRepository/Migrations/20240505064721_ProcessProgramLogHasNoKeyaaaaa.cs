using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    /// <inheritdoc />
    public partial class ProcessProgramLogHasNoKeyaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ProcessPrograms",
                newName: "LogId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "FormattedProcessPrograms",
                newName: "LogId");

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

            migrationBuilder.RenameColumn(
                name: "LogId",
                table: "ProcessPrograms",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "LogId",
                table: "FormattedProcessPrograms",
                newName: "ID");

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "ProcessProgramLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "FormattedProcessProgramLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessPrograms",
                table: "ProcessPrograms",
                column: "PPID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessProgramLogs",
                table: "ProcessProgramLogs",
                column: "PPID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormattedProcessPrograms",
                table: "FormattedProcessPrograms",
                column: "PPID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormattedProcessProgramLogs",
                table: "FormattedProcessProgramLogs",
                column: "PPID");
        }
    }
}
