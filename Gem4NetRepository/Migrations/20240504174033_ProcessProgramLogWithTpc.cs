using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    /// <inheritdoc />
    public partial class ProcessProgramLogWithTpc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormattedProcessProgramLogs_FormattedProcessPrograms_PPID",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessProgramLogs_ProcessPrograms_PPID",
                table: "ProcessProgramLogs");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalLevel",
                table: "ProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "ProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentModelType",
                table: "ProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "ProcessProgramLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PPBody",
                table: "ProcessProgramLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareRevision",
                table: "ProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "ProcessProgramLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ApprovalLevel",
                table: "FormattedProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FormattedProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "FormattedProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentModelType",
                table: "FormattedProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "FormattedProcessProgramLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PPBody",
                table: "FormattedProcessProgramLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareRevision",
                table: "FormattedProcessProgramLogs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "FormattedProcessProgramLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalLevel",
                table: "ProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "Editor",
                table: "ProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "EquipmentModelType",
                table: "ProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "PPBody",
                table: "ProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "SoftwareRevision",
                table: "ProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "ProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "ApprovalLevel",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "Editor",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "EquipmentModelType",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "PPBody",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "SoftwareRevision",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "FormattedProcessProgramLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_FormattedProcessProgramLogs_FormattedProcessPrograms_PPID",
                table: "FormattedProcessProgramLogs",
                column: "PPID",
                principalTable: "FormattedProcessPrograms",
                principalColumn: "PPID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessProgramLogs_ProcessPrograms_PPID",
                table: "ProcessProgramLogs",
                column: "PPID",
                principalTable: "ProcessPrograms",
                principalColumn: "PPID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
