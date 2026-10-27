using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class UpdateCompID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "Branch",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Branch_BrandID",
                table: "Branch",
                column: "BrandID");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Brand_BrandID",
                table: "Branch",
                column: "BrandID",
                principalTable: "Brand",
                principalColumn: "BrandID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Brand_BrandID",
                table: "Branch");

            migrationBuilder.DropIndex(
                name: "IX_Branch_BrandID",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "Branch");
        }
    }
}
