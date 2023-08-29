using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GemVarRepository.Migrations
{
    /// <inheritdoc />
    public partial class Update20230829 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventReportRelation_Events_ECID",
                table: "EventReportRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_EventReportRelation_Reports_RPTID",
                table: "EventReportRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventReportRelation",
                table: "EventReportRelation");

            migrationBuilder.RenameTable(
                name: "EventReportRelation",
                newName: "EventReportLinks");

            migrationBuilder.RenameIndex(
                name: "IX_EventReportRelation_RPTID",
                table: "EventReportLinks",
                newName: "IX_EventReportLinks_RPTID");

            migrationBuilder.AddColumn<DateTime>(
                name: "Version",
                table: "Variables",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventReportLinks",
                table: "EventReportLinks",
                columns: new[] { "ECID", "RPTID" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventReportLinks_Events_ECID",
                table: "EventReportLinks",
                column: "ECID",
                principalTable: "Events",
                principalColumn: "ECID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventReportLinks_Reports_RPTID",
                table: "EventReportLinks",
                column: "RPTID",
                principalTable: "Reports",
                principalColumn: "RPTID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventReportLinks_Events_ECID",
                table: "EventReportLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_EventReportLinks_Reports_RPTID",
                table: "EventReportLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventReportLinks",
                table: "EventReportLinks");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Variables");

            migrationBuilder.RenameTable(
                name: "EventReportLinks",
                newName: "EventReportRelation");

            migrationBuilder.RenameIndex(
                name: "IX_EventReportLinks_RPTID",
                table: "EventReportRelation",
                newName: "IX_EventReportRelation_RPTID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventReportRelation",
                table: "EventReportRelation",
                columns: new[] { "ECID", "RPTID" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventReportRelation_Events_ECID",
                table: "EventReportRelation",
                column: "ECID",
                principalTable: "Events",
                principalColumn: "ECID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventReportRelation_Reports_RPTID",
                table: "EventReportRelation",
                column: "RPTID",
                principalTable: "Reports",
                principalColumn: "RPTID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
