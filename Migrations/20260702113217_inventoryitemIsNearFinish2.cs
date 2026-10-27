using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class inventoryitemIsNearFinish2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsNearToFinish",
                table: "ItemInventory",
                type: "bit",
                nullable: false,
                computedColumnSql: "CAST(CASE WHEN [Count] <= [MinThreshold] THEN 1 ELSE 0 END AS bit)",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComputedColumnSql: "CASE WHEN [Count] <= [MinThreshold] THEN 1 ELSE 0 END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsNearToFinish",
                table: "ItemInventory",
                type: "bit",
                nullable: false,
                computedColumnSql: "CASE WHEN [Count] <= [MinThreshold] THEN 1 ELSE 0 END",
                stored: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComputedColumnSql: "CAST(CASE WHEN [Count] <= [MinThreshold] THEN 1 ELSE 0 END AS bit)",
                oldStored: null);
        }
    }
}
