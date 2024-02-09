using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GemVarRepository.Migrations
{
    /// <inheritdoc />
    public partial class AAddAlarm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GemAlarm",
                columns: table => new
                {
                    ALID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ALCD = table.Column<bool>(type: "INTEGER", nullable: false),
                    AlarmStateVid = table.Column<int>(type: "INTEGER", nullable: true),
                    DefaultAlarmState = table.Column<bool>(type: "INTEGER", nullable: false),
                    ALED = table.Column<bool>(type: "INTEGER", nullable: false),
                    AlarmEnableVid = table.Column<int>(type: "INTEGER", nullable: true),
                    DefaultAlarmEnable = table.Column<bool>(type: "INTEGER", nullable: false),
                    ALTX = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GemAlarm", x => x.ALID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GemAlarm");
        }
    }
}
