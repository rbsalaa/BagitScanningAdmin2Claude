using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class AddDiscountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DiscountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercintage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.DiscountID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_DiscountID",
                table: "TransactionDetails",
                column: "DiscountID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_Discounts_DiscountID",
                table: "TransactionDetails",
                column: "DiscountID",
                principalTable: "Discounts",
                principalColumn: "DiscountID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_Discounts_DiscountID",
                table: "TransactionDetails");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_TransactionDetails_DiscountID",
                table: "TransactionDetails");
        }
    }
}
