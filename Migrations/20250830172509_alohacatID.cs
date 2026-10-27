using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class alohacatID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemModifier_Modifiers_ModifierId",
                table: "ItemModifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemModifier",
                table: "ItemModifier");

            migrationBuilder.RenameColumn(
                name: "ModifierId",
                table: "ItemModifier",
                newName: "ModifierID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemModifier_ModifierId",
                table: "ItemModifier",
                newName: "IX_ItemModifier_ModifierID");

            migrationBuilder.AlterColumn<int>(
                name: "ModifierID",
                table: "ItemModifier",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ModifierCategoryID",
                table: "ItemModifier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemModifier",
                table: "ItemModifier",
                columns: new[] { "ItemId", "ModifierCategoryID" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemModifier_ModifierCategoryID",
                table: "ItemModifier",
                column: "ModifierCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemModifier_ModifierCategory_ModifierCategoryID",
                table: "ItemModifier",
                column: "ModifierCategoryID",
                principalTable: "ModifierCategory",
                principalColumn: "ModifierCategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemModifier_Modifiers_ModifierID",
                table: "ItemModifier",
                column: "ModifierID",
                principalTable: "Modifiers",
                principalColumn: "ModifierID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemModifier_ModifierCategory_ModifierCategoryID",
                table: "ItemModifier");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemModifier_Modifiers_ModifierID",
                table: "ItemModifier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemModifier",
                table: "ItemModifier");

            migrationBuilder.DropIndex(
                name: "IX_ItemModifier_ModifierCategoryID",
                table: "ItemModifier");

            migrationBuilder.DropColumn(
                name: "ModifierCategoryID",
                table: "ItemModifier");

            migrationBuilder.RenameColumn(
                name: "ModifierID",
                table: "ItemModifier",
                newName: "ModifierId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemModifier_ModifierID",
                table: "ItemModifier",
                newName: "IX_ItemModifier_ModifierId");

            migrationBuilder.AlterColumn<int>(
                name: "ModifierId",
                table: "ItemModifier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemModifier",
                table: "ItemModifier",
                columns: new[] { "ItemId", "ModifierId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ItemModifier_Modifiers_ModifierId",
                table: "ItemModifier",
                column: "ModifierId",
                principalTable: "Modifiers",
                principalColumn: "ModifierID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
