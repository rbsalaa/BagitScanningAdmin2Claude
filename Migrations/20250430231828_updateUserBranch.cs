using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updateUserBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "UserPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BranchID",
                table: "AspNetUsers",
                column: "BranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Branch_BranchID",
                table: "AspNetUsers",
                column: "BranchID",
                principalTable: "Branch",
                principalColumn: "BranchID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Branch_BranchID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BranchID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "AspNetUsers");
        }
    }
}
