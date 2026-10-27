using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class CompBrandRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Brand_CompID",
                table: "Brand",
                column: "CompID");

            migrationBuilder.AddForeignKey(
                name: "FK_Brand_Company_CompID",
                table: "Brand",
                column: "CompID",
                principalTable: "Company",
                principalColumn: "CompID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brand_Company_CompID",
                table: "Brand");

            migrationBuilder.DropIndex(
                name: "IX_Brand_CompID",
                table: "Brand");
        }
    }
}
