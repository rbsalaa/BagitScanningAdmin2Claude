using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class Suggestionsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuggestionCat",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuggestionCat",
                table: "Category");
        }
    }
}
