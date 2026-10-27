using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updateBanner2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KiosikBanner_Banner_AdsID",
                table: "KiosikBanner");

            migrationBuilder.DropIndex(
                name: "IX_KiosikBanner_AdsID",
                table: "KiosikBanner");

            migrationBuilder.DropColumn(
                name: "AdsID",
                table: "KiosikBanner");

            migrationBuilder.CreateIndex(
                name: "IX_KiosikBanner_Banner",
                table: "KiosikBanner",
                column: "Banner");

            migrationBuilder.AddForeignKey(
                name: "FK_KiosikBanner_Banner_Banner",
                table: "KiosikBanner",
                column: "Banner",
                principalTable: "Banner",
                principalColumn: "BannerID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KiosikBanner_Banner_Banner",
                table: "KiosikBanner");

            migrationBuilder.DropIndex(
                name: "IX_KiosikBanner_Banner",
                table: "KiosikBanner");

            migrationBuilder.AddColumn<int>(
                name: "AdsID",
                table: "KiosikBanner",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KiosikBanner_AdsID",
                table: "KiosikBanner",
                column: "AdsID");

            migrationBuilder.AddForeignKey(
                name: "FK_KiosikBanner_Banner_AdsID",
                table: "KiosikBanner",
                column: "AdsID",
                principalTable: "Banner",
                principalColumn: "BannerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
