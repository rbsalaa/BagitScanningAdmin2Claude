using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class subsscriptionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandID",
                table: "SubscrubtionInfo");

            migrationBuilder.DropColumn(
                name: "SupscrubtionEndDate",
                table: "SubscrubtionInfo");

            migrationBuilder.DropColumn(
                name: "SupscrubtionStartDate",
                table: "SubscrubtionInfo");

            migrationBuilder.CreateTable(
                name: "SubscrubtionInfoDetails",
                columns: table => new
                {
                    SupscrubtionDetailID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupscrubtionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupscrubtionStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupscrubtionEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrandID = table.Column<int>(type: "int", nullable: true),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    KiosikID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscrubtionInfoDetails", x => x.SupscrubtionDetailID);
                    table.ForeignKey(
                        name: "FK_SubscrubtionInfoDetails_Branch_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branch",
                        principalColumn: "BranchID");
                    table.ForeignKey(
                        name: "FK_SubscrubtionInfoDetails_Brand_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brand",
                        principalColumn: "BrandID");
                    table.ForeignKey(
                        name: "FK_SubscrubtionInfoDetails_Kiosik_KiosikID",
                        column: x => x.KiosikID,
                        principalTable: "Kiosik",
                        principalColumn: "KiosikID");
                    table.ForeignKey(
                        name: "FK_SubscrubtionInfoDetails_SubscrubtionInfo_SupscrubtionID",
                        column: x => x.SupscrubtionID,
                        principalTable: "SubscrubtionInfo",
                        principalColumn: "SupscrubtionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscrubtionInfoDetails_BranchID",
                table: "SubscrubtionInfoDetails",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_SubscrubtionInfoDetails_BrandID",
                table: "SubscrubtionInfoDetails",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_SubscrubtionInfoDetails_KiosikID",
                table: "SubscrubtionInfoDetails",
                column: "KiosikID");

            migrationBuilder.CreateIndex(
                name: "IX_SubscrubtionInfoDetails_SupscrubtionID",
                table: "SubscrubtionInfoDetails",
                column: "SupscrubtionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscrubtionInfoDetails");

            migrationBuilder.AddColumn<int>(
                name: "BrandID",
                table: "SubscrubtionInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SupscrubtionEndDate",
                table: "SubscrubtionInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SupscrubtionStartDate",
                table: "SubscrubtionInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
