using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class Modifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "Modifiers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "Modifiers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "Modifiers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "Modifiers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "ModifierCategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "ModifierCategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "ModifierCategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "ModifierCategory",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "ModifierCategory");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "ModifierCategory");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "ModifierCategory");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "ModifierCategory");
        }
    }
}
