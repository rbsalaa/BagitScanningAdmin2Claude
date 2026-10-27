using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updateCustomerSubID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "SubscrubtionInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscrubtionInfo_CustomerID",
                table: "SubscrubtionInfo",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscrubtionInfo_Customer_CustomerID",
                table: "SubscrubtionInfo",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "CustomerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscrubtionInfo_Customer_CustomerID",
                table: "SubscrubtionInfo");

            migrationBuilder.DropIndex(
                name: "IX_SubscrubtionInfo_CustomerID",
                table: "SubscrubtionInfo");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "SubscrubtionInfo");
        }
    }
}
