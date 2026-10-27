using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class AddInventorySystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryWarehouse",
                columns: table => new
                {
                    InvID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InvName_AR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InvLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    CompID = table.Column<int>(type: "int", nullable: true),
                    BrandID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryWarehouse", x => x.InvID);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitName_AR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitID);
                });

            migrationBuilder.CreateTable(
                name: "ItemInventory",
                columns: table => new
                {
                    InventoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    InvID = table.Column<int>(type: "int", nullable: false),
                    UnitID = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MinThreshold = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MaxThreshold = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    IsNearToFinish = table.Column<bool>(type: "bit", nullable: false, computedColumnSql: "CASE WHEN [Count] <= [MinThreshold] THEN 1 ELSE 0 END", stored: true),
                    LastRestockDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInventory", x => x.InventoryID);
                    table.ForeignKey(
                        name: "FK_ItemInventory_InventoryWarehouse_InvID",
                        column: x => x.InvID,
                        principalTable: "InventoryWarehouse",
                        principalColumn: "InvID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemInventory_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemInventory_Units_UnitID",
                        column: x => x.UnitID,
                        principalTable: "Units",
                        principalColumn: "UnitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemUnitConversion",
                columns: table => new
                {
                    ConversionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    FromUnitID = table.Column<int>(type: "int", nullable: false),
                    ToUnitID = table.Column<int>(type: "int", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(10,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnitConversion", x => x.ConversionID);
                    table.ForeignKey(
                        name: "FK_ItemUnitConversion_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemUnitConversion_Units_FromUnitID",
                        column: x => x.FromUnitID,
                        principalTable: "Units",
                        principalColumn: "UnitID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemUnitConversion_Units_ToUnitID",
                        column: x => x.ToUnitID,
                        principalTable: "Units",
                        principalColumn: "UnitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemInventory_InvID",
                table: "ItemInventory",
                column: "InvID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemInventory_ItemID_InvID",
                table: "ItemInventory",
                columns: new[] { "ItemID", "InvID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemInventory_UnitID",
                table: "ItemInventory",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnitConversion_FromUnitID",
                table: "ItemUnitConversion",
                column: "FromUnitID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnitConversion_ItemID",
                table: "ItemUnitConversion",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnitConversion_ToUnitID",
                table: "ItemUnitConversion",
                column: "ToUnitID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemInventory");

            migrationBuilder.DropTable(
                name: "ItemUnitConversion");

            migrationBuilder.DropTable(
                name: "InventoryWarehouse");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
