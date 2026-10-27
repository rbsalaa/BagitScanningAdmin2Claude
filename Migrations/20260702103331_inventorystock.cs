using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class inventorystock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchStock",
                columns: table => new
                {
                    BranchStockID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchID = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    UnitID = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MinThreshold = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    LastTransferDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchStock", x => x.BranchStockID);
                    table.ForeignKey(
                        name: "FK_BranchStock_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchStock_Units_UnitID",
                        column: x => x.UnitID,
                        principalTable: "Units",
                        principalColumn: "UnitID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchStock_BranchID_ItemID",
                table: "BranchStock",
                columns: new[] { "BranchID", "ItemID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BranchStock_ItemID",
                table: "BranchStock",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_BranchStock_UnitID",
                table: "BranchStock",
                column: "UnitID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchStock");
        }
    }
}
