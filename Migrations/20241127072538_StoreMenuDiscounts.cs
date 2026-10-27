using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class StoreMenuDiscounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreMenuDiscounts",
                columns: table => new
                {
                    DiscountMenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercintage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAllMenuDiscount = table.Column<bool>(type: "bit", nullable: false),
                    IsPercintage = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMenuDiscounts", x => x.DiscountMenuID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreMenuDiscounts");
        }
    }
}
