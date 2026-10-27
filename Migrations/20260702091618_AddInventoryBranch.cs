using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class AddInventoryBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "InventoryWarehouse");

            migrationBuilder.CreateTable(
                name: "InventoryBranch",
                columns: table => new
                {
                    InvID = table.Column<int>(type: "int", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: false),
                    InventoryInvID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBranch", x => new { x.InvID, x.BranchID });
                    table.ForeignKey(
                        name: "FK_InventoryBranch_Branch_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branch",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryBranch_InventoryWarehouse_InventoryInvID",
                        column: x => x.InventoryInvID,
                        principalTable: "InventoryWarehouse",
                        principalColumn: "InvID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBranch_BranchID",
                table: "InventoryBranch",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBranch_InventoryInvID",
                table: "InventoryBranch",
                column: "InventoryInvID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryBranch");

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "InventoryWarehouse",
                type: "int",
                nullable: true);
        }
    }
}
