using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updateNourasData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ItemsCategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetID",
                table: "Banner",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetType",
                table: "Banner",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetUrl",
                table: "Banner",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ItemsCategory");

            migrationBuilder.DropColumn(
                name: "TargetID",
                table: "Banner");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "Banner");

            migrationBuilder.DropColumn(
                name: "TargetUrl",
                table: "Banner");
        }
    }
}
