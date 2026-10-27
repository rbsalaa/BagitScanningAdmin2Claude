using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class ItemCatGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "Category",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "Category");
        }
    }
}
