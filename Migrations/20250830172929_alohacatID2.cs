using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class alohacatID2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemsModifierID",
                table: "ItemModifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemsModifierID",
                table: "ItemModifier",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
