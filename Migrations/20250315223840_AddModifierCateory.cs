using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class AddModifierCateory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModifierCategoryID",
                table: "Modifiers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ModifierCategory",
                columns: table => new
                {
                    ModifierCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifierCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifierCategoryName_AR = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifierCategory", x => x.ModifierCategoryID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modifiers_ModifierCategoryID",
                table: "Modifiers",
                column: "ModifierCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Modifiers_ModifierCategory_ModifierCategoryID",
                table: "Modifiers",
                column: "ModifierCategoryID",
                principalTable: "ModifierCategory",
                principalColumn: "ModifierCategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_ModifierCategory_ModifierCategoryID",
                table: "Modifiers");

            migrationBuilder.DropTable(
                name: "ModifierCategory");

            migrationBuilder.DropIndex(
                name: "IX_Modifiers_ModifierCategoryID",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "ModifierCategoryID",
                table: "Modifiers");
        }
    }
}
