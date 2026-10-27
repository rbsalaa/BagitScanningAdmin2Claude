using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class inventorystock2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("ALTER TABLE ItemInventory DROP COLUMN IsNearToFinish;");
            migrationBuilder.Sql(@"ALTER TABLE ItemInventory ADD IsNearToFinish AS
        CAST(CASE WHEN [Count] <= [MinThreshold] THEN 1 ELSE 0 END AS bit);");
        

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
