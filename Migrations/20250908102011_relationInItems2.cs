using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class relationInItems2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemModifier_Groups_GroupsGroupID",
                table: "ItemModifier");

            migrationBuilder.DropIndex(
                name: "IX_ItemModifier_GroupsGroupID",
                table: "ItemModifier");

            migrationBuilder.DropColumn(
                name: "GroupsGroupID",
                table: "ItemModifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupsGroupID",
                table: "ItemModifier",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemModifier_GroupsGroupID",
                table: "ItemModifier",
                column: "GroupsGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemModifier_Groups_GroupsGroupID",
                table: "ItemModifier",
                column: "GroupsGroupID",
                principalTable: "Groups",
                principalColumn: "GroupID");
        }
    }
}
