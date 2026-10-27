using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class alohacatPKinModifiresw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemModifier_Modifiers_ModifierID",
                table: "ItemModifier");

            migrationBuilder.DropIndex(
                name: "IX_ItemModifier_ModifierID",
                table: "ItemModifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ItemModifier_ModifierID",
                table: "ItemModifier",
                column: "ModifierID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemModifier_Modifiers_ModifierID",
                table: "ItemModifier",
                column: "ModifierID",
                principalTable: "Modifiers",
                principalColumn: "ModifierID");
        }
    }
}
