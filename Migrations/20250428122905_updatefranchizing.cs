using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class updatefranchizing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FranchID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRBSUser",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Franchising",
                columns: table => new
                {
                    FranchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FranchLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompID = table.Column<int>(type: "int", nullable: false),
                    BrandID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franchising", x => x.FranchID);
                    table.ForeignKey(
                        name: "FK_Franchising_Brand_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brand",
                        principalColumn: "BrandID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Franchising_Company_CompID",
                        column: x => x.CompID,
                        principalTable: "Company",
                        principalColumn: "CompID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BrandID",
                table: "AspNetUsers",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompID",
                table: "AspNetUsers",
                column: "CompID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FranchID",
                table: "AspNetUsers",
                column: "FranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Franchising_BrandID",
                table: "Franchising",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Franchising_CompID",
                table: "Franchising",
                column: "CompID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Brand_BrandID",
                table: "AspNetUsers",
                column: "BrandID",
                principalTable: "Brand",
                principalColumn: "BrandID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Company_CompID",
                table: "AspNetUsers",
                column: "CompID",
                principalTable: "Company",
                principalColumn: "CompID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Franchising_FranchID",
                table: "AspNetUsers",
                column: "FranchID",
                principalTable: "Franchising",
                principalColumn: "FranchID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Brand_BrandID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Company_CompID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Franchising_FranchID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Franchising");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BrandID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FranchID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FranchID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsRBSUser",
                table: "AspNetUsers");
        }
    }
}
