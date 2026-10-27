using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updateFinanicalgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "TAXGroup",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "TAXGroup",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "TAXGroup",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "TAXGroup",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "StoreMenuDiscounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "StoreMenuDiscounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "StoreMenuDiscounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "StoreMenuDiscounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "PaymentMethod",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "PaymentMethod",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "PaymentMethod",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "PaymentMethod",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "Discounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "Discounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "Discounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "Discounts",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "TAXGroup");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "TAXGroup");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "TAXGroup");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "TAXGroup");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "StoreMenuDiscounts");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "StoreMenuDiscounts");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "StoreMenuDiscounts");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "StoreMenuDiscounts");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "Discounts");
        }
    }
}
