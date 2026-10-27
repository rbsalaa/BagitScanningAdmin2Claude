using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class ModifierCategoryXmodifierTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModifierModifierCategory_ModifierCategory_ModifierCategoryID",
                table: "ModifierModifierCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ModifierModifierCategory_Modifiers_ModifierID",
                table: "ModifierModifierCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ModifierModifierCategory_ModifierCategory_ModifierCategoryID",
                table: "ModifierModifierCategory",
                column: "ModifierCategoryID",
                principalTable: "ModifierCategory",
                principalColumn: "ModifierCategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModifierModifierCategory_Modifiers_ModifierID",
                table: "ModifierModifierCategory",
                column: "ModifierID",
                principalTable: "Modifiers",
                principalColumn: "ModifierID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModifierModifierCategory_ModifierCategory_ModifierCategoryID",
                table: "ModifierModifierCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ModifierModifierCategory_Modifiers_ModifierID",
                table: "ModifierModifierCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ModifierModifierCategory_ModifierCategory_ModifierCategoryID",
                table: "ModifierModifierCategory",
                column: "ModifierCategoryID",
                principalTable: "ModifierCategory",
                principalColumn: "ModifierCategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModifierModifierCategory_Modifiers_ModifierID",
                table: "ModifierModifierCategory",
                column: "ModifierID",
                principalTable: "Modifiers",
                principalColumn: "ModifierID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
