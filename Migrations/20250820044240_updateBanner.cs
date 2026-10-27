using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updateBanner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdsStartDate",
                table: "Banner",
                newName: "BannerStartDate");

            migrationBuilder.RenameColumn(
                name: "AdsEndDate",
                table: "Banner",
                newName: "BannerEndDate");

            migrationBuilder.CreateTable(
                name: "KiosikBanner",
                columns: table => new
                {
                    KiosikBannerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Banner = table.Column<int>(type: "int", nullable: false),
                    KiosikID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AdsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KiosikBanner", x => x.KiosikBannerID);
                    table.ForeignKey(
                        name: "FK_KiosikBanner_Banner_AdsID",
                        column: x => x.AdsID,
                        principalTable: "Banner",
                        principalColumn: "BannerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KiosikBanner_Kiosik_KiosikID",
                        column: x => x.KiosikID,
                        principalTable: "Kiosik",
                        principalColumn: "KiosikID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KiosikBanner_AdsID",
                table: "KiosikBanner",
                column: "AdsID");

            migrationBuilder.CreateIndex(
                name: "IX_KiosikBanner_KiosikID",
                table: "KiosikBanner",
                column: "KiosikID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KiosikBanner");

            migrationBuilder.RenameColumn(
                name: "BannerStartDate",
                table: "Banner",
                newName: "AdsStartDate");

            migrationBuilder.RenameColumn(
                name: "BannerEndDate",
                table: "Banner",
                newName: "AdsEndDate");
        }
    }
}
