using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class UpdateKiosikMenuID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kiosik_Menu_MenuID",
                table: "Kiosik");

            migrationBuilder.AlterColumn<int>(
                name: "MenuID",
                table: "Kiosik",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosik_Menu_MenuID",
                table: "Kiosik",
                column: "MenuID",
                principalTable: "Menu",
                principalColumn: "MenuID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kiosik_Menu_MenuID",
                table: "Kiosik");

            migrationBuilder.AlterColumn<int>(
                name: "MenuID",
                table: "Kiosik",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosik_Menu_MenuID",
                table: "Kiosik",
                column: "MenuID",
                principalTable: "Menu",
                principalColumn: "MenuID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
