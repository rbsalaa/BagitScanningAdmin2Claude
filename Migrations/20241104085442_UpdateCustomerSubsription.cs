using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class UpdateCustomerSubsription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SupscrubtionID",
                table: "Customer",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_SubscrubtionInfo_SupscrubtionID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_SupscrubtionID",
                table: "Customer");

            migrationBuilder.AlterColumn<string>(
                name: "SupscrubtionID",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
