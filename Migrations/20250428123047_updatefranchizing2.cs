using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updatefranchizing2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "Branch",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branch_FranchID",
                table: "Branch",
                column: "FranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Franchising_FranchID",
                table: "Branch",
                column: "FranchID",
                principalTable: "Franchising",
                principalColumn: "FranchID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Franchising_FranchID",
                table: "Branch");

            migrationBuilder.DropIndex(
                name: "IX_Branch_FranchID",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "Branch");
        }
    }
}
