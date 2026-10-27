using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class Banners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    BannerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdsStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdsEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BannerURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    CompID = table.Column<int>(type: "int", nullable: true),
                    BrandID = table.Column<int>(type: "int", nullable: true),
                    FranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.BannerID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner");
        }
    }
}
