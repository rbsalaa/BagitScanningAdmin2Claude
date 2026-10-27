using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class inventoryBranchRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryBranch_InventoryWarehouse_InventoryInvID",
                table: "InventoryBranch");

            migrationBuilder.DropIndex(
                name: "IX_InventoryBranch_InventoryInvID",
                table: "InventoryBranch");

            migrationBuilder.DropColumn(
                name: "InventoryInvID",
                table: "InventoryBranch");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryBranch_InventoryWarehouse_InvID",
                table: "InventoryBranch",
                column: "InvID",
                principalTable: "InventoryWarehouse",
                principalColumn: "InvID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryBranch_InventoryWarehouse_InvID",
                table: "InventoryBranch");

            migrationBuilder.AddColumn<int>(
                name: "InventoryInvID",
                table: "InventoryBranch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBranch_InventoryInvID",
                table: "InventoryBranch",
                column: "InventoryInvID");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryBranch_InventoryWarehouse_InventoryInvID",
                table: "InventoryBranch",
                column: "InventoryInvID",
                principalTable: "InventoryWarehouse",
                principalColumn: "InvID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
