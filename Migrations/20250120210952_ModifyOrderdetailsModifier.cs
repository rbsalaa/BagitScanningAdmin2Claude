using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class ModifyOrderdetailsModifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_OrderDetail_OrderDetailId",
                table: "Modifiers");

            migrationBuilder.DropIndex(
                name: "IX_Modifiers_OrderDetailId",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Modifiers");

            migrationBuilder.CreateTable(
                name: "ModifierOrderDetail",
                columns: table => new
                {
                    ModifiersModifierID = table.Column<int>(type: "int", nullable: false),
                    OrderDetailsOrderDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModifierOrderDetail", x => new { x.ModifiersModifierID, x.OrderDetailsOrderDetailId });
                    table.ForeignKey(
                        name: "FK_ModifierOrderDetail_Modifiers_ModifiersModifierID",
                        column: x => x.ModifiersModifierID,
                        principalTable: "Modifiers",
                        principalColumn: "ModifierID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModifierOrderDetail_OrderDetail_OrderDetailsOrderDetailId",
                        column: x => x.OrderDetailsOrderDetailId,
                        principalTable: "OrderDetail",
                        principalColumn: "OrderDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModifierOrderDetail_OrderDetailsOrderDetailId",
                table: "ModifierOrderDetail",
                column: "OrderDetailsOrderDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModifierOrderDetail");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "Modifiers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modifiers_OrderDetailId",
                table: "Modifiers",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modifiers_OrderDetail_OrderDetailId",
                table: "Modifiers",
                column: "OrderDetailId",
                principalTable: "OrderDetail",
                principalColumn: "OrderDetailId");
        }
    }
}
