using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class alohaID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlohaProductID",
                table: "Modifiers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlohaProductID",
                table: "ModifierCategory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlohaProductID",
                table: "ItemModifier",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlohaProductID",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "AlohaProductID",
                table: "ModifierCategory");

            migrationBuilder.DropColumn(
                name: "AlohaProductID",
                table: "ItemModifier");
        }
    }
}
