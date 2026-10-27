using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class ModifyorderKiosk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KiosikID",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_KiosikID",
                table: "Order",
                column: "KiosikID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Kiosik_KiosikID",
                table: "Order",
                column: "KiosikID",
                principalTable: "Kiosik",
                principalColumn: "KiosikID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Kiosik_KiosikID",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_KiosikID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "KiosikID",
                table: "Order");
        }
    }
}
