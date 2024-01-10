using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GemVarRepository.Migrations
{
    /// <inheritdoc />
    public partial class TestFormattedPP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportVariableLink_Reports_RPTID",
                table: "ReportVariableLink");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportVariableLink_Variables_VID",
                table: "ReportVariableLink");

            migrationBuilder.DropTable(
                name: "ProcessParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportVariableLink",
                table: "ReportVariableLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessProgram",
                table: "ProcessProgram");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormattedProcessProgram",
                table: "FormattedProcessProgram");

            migrationBuilder.RenameTable(
                name: "ReportVariableLink",
                newName: "ReportVariableLinks");

            migrationBuilder.RenameTable(
                name: "ProcessProgram",
                newName: "ProcessPrograms");

            migrationBuilder.RenameTable(
                name: "FormattedProcessProgram",
                newName: "FormattedProcessPrograms");

            migrationBuilder.RenameIndex(
                name: "IX_ReportVariableLink_RPTID",
                table: "ReportVariableLinks",
                newName: "IX_ReportVariableLinks_RPTID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FormattedProcessPrograms",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Variables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "MinValue",
                table: "Variables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "MaxValue",
                table: "Variables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Length",
                table: "Variables",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "Variables",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Definition",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Reports",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DATAID",
                table: "Events",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EnabledVid",
                table: "Events",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalLevel",
                table: "FormattedProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FormattedProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "FormattedProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentModelType",
                table: "FormattedProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PPBody",
                table: "FormattedProcessPrograms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoftwareRevision",
                table: "FormattedProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FormattedProcessPrograms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportVariableLinks",
                table: "ReportVariableLinks",
                columns: new[] { "VID", "RPTID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessPrograms",
                table: "ProcessPrograms",
                column: "PPID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormattedProcessPrograms",
                table: "FormattedProcessPrograms",
                columns: new[] { "ID", "PPID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReportVariableLinks_Reports_RPTID",
                table: "ReportVariableLinks",
                column: "RPTID",
                principalTable: "Reports",
                principalColumn: "RPTID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportVariableLinks_Variables_VID",
                table: "ReportVariableLinks",
                column: "VID",
                principalTable: "Variables",
                principalColumn: "VID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportVariableLinks_Reports_RPTID",
                table: "ReportVariableLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportVariableLinks_Variables_VID",
                table: "ReportVariableLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportVariableLinks",
                table: "ReportVariableLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessPrograms",
                table: "ProcessPrograms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormattedProcessPrograms",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropColumn(
                name: "Definition",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DATAID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EnabledVid",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ApprovalLevel",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropColumn(
                name: "Editor",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropColumn(
                name: "EquipmentModelType",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropColumn(
                name: "PPBody",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropColumn(
                name: "SoftwareRevision",
                table: "FormattedProcessPrograms");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FormattedProcessPrograms");

            migrationBuilder.RenameTable(
                name: "ReportVariableLinks",
                newName: "ReportVariableLink");

            migrationBuilder.RenameTable(
                name: "ProcessPrograms",
                newName: "ProcessProgram");

            migrationBuilder.RenameTable(
                name: "FormattedProcessPrograms",
                newName: "FormattedProcessProgram");

            migrationBuilder.RenameIndex(
                name: "IX_ReportVariableLinks_RPTID",
                table: "ReportVariableLink",
                newName: "IX_ReportVariableLink_RPTID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "FormattedProcessProgram",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Variables",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MinValue",
                table: "Variables",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaxValue",
                table: "Variables",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Length",
                table: "Variables",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "Variables",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportVariableLink",
                table: "ReportVariableLink",
                columns: new[] { "VID", "RPTID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessProgram",
                table: "ProcessProgram",
                column: "PPID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormattedProcessProgram",
                table: "FormattedProcessProgram",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProcessParameter",
                columns: table => new
                {
                    ProcessProgramId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProcessCommandCode = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DataType = table.Column<string>(type: "TEXT", nullable: false),
                    Definition = table.Column<string>(type: "TEXT", nullable: true),
                    Length = table.Column<int>(type: "INTEGER", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_ReportVariableLink_Reports_RPTID",
                table: "ReportVariableLink",
                column: "RPTID",
                principalTable: "Reports",
                principalColumn: "RPTID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportVariableLink_Variables_VID",
                table: "ReportVariableLink",
                column: "VID",
                principalTable: "Variables",
                principalColumn: "VID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
