using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updateMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuID",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_MenuID",
                table: "Items",
                column: "MenuID");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Menu_MenuID",
                table: "Items",
                column: "MenuID",
                principalTable: "Menu",
                principalColumn: "MenuID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Menu_MenuID",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_MenuID",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "MenuID",
                table: "Items");
        }
    }
}
