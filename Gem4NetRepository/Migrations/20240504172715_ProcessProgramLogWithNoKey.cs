using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gem4NetRepository.Migrations
{
    /// <inheritdoc />
    public partial class ProcessProgramLogWithNoKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FormattedProcessPrograms");

            migrationBuilder.RenameColumn(
                name: "Format",
                table: "ProcessPrograms",
                newName: "UpdateTime");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalLevel",
                table: "ProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ProcessPrograms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Editor",
                table: "ProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentModelType",
                table: "ProcessPrograms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "ProcessPrograms",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                name: "SoftwareRevision",
                table: "ProcessPrograms",
                type: "TEXT",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalLevel",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "Editor",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "EquipmentModelType",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "LogId",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "PPChangeStatus",
                table: "ProcessPrograms");

            migrationBuilder.DropColumn(
                name: "SoftwareRevision",
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

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "ProcessPrograms",
                newName: "Format");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FormattedProcessPrograms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
