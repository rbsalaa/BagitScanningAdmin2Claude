using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class HardwareLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KiosikHardwareLog",
                columns: table => new
                {
                    KiosikHardwareID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HardWareName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HardWareCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stauts = table.Column<bool>(type: "bit", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    CompID = table.Column<int>(type: "int", nullable: true),
                    BrandID = table.Column<int>(type: "int", nullable: true),
                    FranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KiosikHardwareLog", x => x.KiosikHardwareID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KiosikHardwareLog");
        }
    }
}
