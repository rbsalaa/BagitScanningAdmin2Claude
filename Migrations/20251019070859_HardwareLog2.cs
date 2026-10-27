using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class HardwareLog2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KiosikID",
                table: "KiosikHardwareLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KiosikName",
                table: "KiosikHardwareLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KiosikID",
                table: "KiosikHardwareLog");

            migrationBuilder.DropColumn(
                name: "KiosikName",
                table: "KiosikHardwareLog");
        }
    }
}
