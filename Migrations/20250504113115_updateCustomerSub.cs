using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updateCustomerSub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_SubscrubtionInfo_SupscrubtionID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_SupscrubtionID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "SupscrubtionID",
                table: "Customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupscrubtionID",
                table: "Customer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_SupscrubtionID",
                table: "Customer",
                column: "SupscrubtionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_SubscrubtionInfo_SupscrubtionID",
                table: "Customer",
                column: "SupscrubtionID",
                principalTable: "SubscrubtionInfo",
                principalColumn: "SupscrubtionID");
        }
    }
}
