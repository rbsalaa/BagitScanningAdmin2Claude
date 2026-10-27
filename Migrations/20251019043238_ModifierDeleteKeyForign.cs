using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class ModifierDeleteKeyForign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_ModifierCategory_ModifierCategoryID",
                table: "Modifiers");

            migrationBuilder.AlterColumn<int>(
                name: "ModifierCategoryID",
                table: "Modifiers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Modifiers_ModifierCategory_ModifierCategoryID",
                table: "Modifiers",
                column: "ModifierCategoryID",
                principalTable: "ModifierCategory",
                principalColumn: "ModifierCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_ModifierCategory_ModifierCategoryID",
                table: "Modifiers");

            migrationBuilder.AlterColumn<int>(
                name: "ModifierCategoryID",
                table: "Modifiers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Modifiers_ModifierCategory_ModifierCategoryID",
                table: "Modifiers",
                column: "ModifierCategoryID",
                principalTable: "ModifierCategory",
                principalColumn: "ModifierCategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
