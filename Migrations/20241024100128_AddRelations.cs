using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class AddRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TransactionSummery_KiosikID",
                table: "TransactionSummery",
                column: "KiosikID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSummery_StautsID",
                table: "TransactionSummery",
                column: "StautsID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_ItemID",
                table: "TransactionDetails",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_PaymentMethodID",
                table: "TransactionDetails",
                column: "PaymentMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_TAXID",
                table: "TransactionDetails",
                column: "TAXID");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_TransactionSummeryID",
                table: "TransactionDetails",
                column: "TransactionSummeryID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ItemID",
                table: "MenuItems",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuID",
                table: "MenuItems",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_KiosikAds_AdsID",
                table: "KiosikAds",
                column: "AdsID");

            migrationBuilder.CreateIndex(
                name: "IX_KiosikAds_KiosikID",
                table: "KiosikAds",
                column: "KiosikID");

            migrationBuilder.CreateIndex(
                name: "IX_Kiosik_BranchID",
                table: "Kiosik",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Kiosik_BrandID",
                table: "Kiosik",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Kiosik_CompID",
                table: "Kiosik",
                column: "CompID");

            migrationBuilder.CreateIndex(
                name: "IX_Kiosik_CustomerID",
                table: "Kiosik",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Kiosik_MenuID",
                table: "Kiosik",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsPromotion_PromotionID",
                table: "ItemsPromotion",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsCategory_CategoryID",
                table: "ItemsCategory",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsCategory_ItemID",
                table: "ItemsCategory",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Items_GroupID",
                table: "Items",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsItem_GroupID",
                table: "GroupsItem",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsItem_ItemID",
                table: "GroupsItem",
                column: "ItemID");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsItem_Groups_GroupID",
                table: "GroupsItem",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "GroupID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsItem_Items_ItemID",
                table: "GroupsItem",
                column: "ItemID",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Groups_GroupID",
                table: "Items",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsCategory_Category_CategoryID",
                table: "ItemsCategory",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsCategory_Items_ItemID",
                table: "ItemsCategory",
                column: "ItemID",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsPromotion_Promotion_PromotionID",
                table: "ItemsPromotion",
                column: "PromotionID",
                principalTable: "Promotion",
                principalColumn: "PromotionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosik_Branch_BranchID",
                table: "Kiosik",
                column: "BranchID",
                principalTable: "Branch",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosik_Brand_BrandID",
                table: "Kiosik",
                column: "BrandID",
                principalTable: "Brand",
                principalColumn: "BrandID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosik_Company_CompID",
                table: "Kiosik",
                column: "CompID",
                principalTable: "Company",
                principalColumn: "CompID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosik_Customer_CustomerID",
                table: "Kiosik",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kiosik_Menu_MenuID",
                table: "Kiosik",
                column: "MenuID",
                principalTable: "Menu",
                principalColumn: "MenuID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KiosikAds_Advertisment_AdsID",
                table: "KiosikAds",
                column: "AdsID",
                principalTable: "Advertisment",
                principalColumn: "AdsID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KiosikAds_Kiosik_KiosikID",
                table: "KiosikAds",
                column: "KiosikID",
                principalTable: "Kiosik",
                principalColumn: "KiosikID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Items_ItemID",
                table: "MenuItems",
                column: "ItemID",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Menu_MenuID",
                table: "MenuItems",
                column: "MenuID",
                principalTable: "Menu",
                principalColumn: "MenuID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_Items_ItemID",
                table: "TransactionDetails",
                column: "ItemID",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_PaymentMethod_PaymentMethodID",
                table: "TransactionDetails",
                column: "PaymentMethodID",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_TAXGroup_TAXID",
                table: "TransactionDetails",
                column: "TAXID",
                principalTable: "TAXGroup",
                principalColumn: "TAXID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_TransactionSummery_TransactionSummeryID",
                table: "TransactionDetails",
                column: "TransactionSummeryID",
                principalTable: "TransactionSummery",
                principalColumn: "TransactionSummeryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionSummery_Kiosik_KiosikID",
                table: "TransactionSummery",
                column: "KiosikID",
                principalTable: "Kiosik",
                principalColumn: "KiosikID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionSummery_TransactionStauts_StautsID",
                table: "TransactionSummery",
                column: "StautsID",
                principalTable: "TransactionStauts",
                principalColumn: "StautsID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupsItem_Groups_GroupID",
                table: "GroupsItem");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupsItem_Items_ItemID",
                table: "GroupsItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Groups_GroupID",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsCategory_Category_CategoryID",
                table: "ItemsCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsCategory_Items_ItemID",
                table: "ItemsCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsPromotion_Promotion_PromotionID",
                table: "ItemsPromotion");

            migrationBuilder.DropForeignKey(
                name: "FK_Kiosik_Branch_BranchID",
                table: "Kiosik");

            migrationBuilder.DropForeignKey(
                name: "FK_Kiosik_Brand_BrandID",
                table: "Kiosik");

            migrationBuilder.DropForeignKey(
                name: "FK_Kiosik_Company_CompID",
                table: "Kiosik");

            migrationBuilder.DropForeignKey(
                name: "FK_Kiosik_Customer_CustomerID",
                table: "Kiosik");

            migrationBuilder.DropForeignKey(
                name: "FK_Kiosik_Menu_MenuID",
                table: "Kiosik");

            migrationBuilder.DropForeignKey(
                name: "FK_KiosikAds_Advertisment_AdsID",
                table: "KiosikAds");

            migrationBuilder.DropForeignKey(
                name: "FK_KiosikAds_Kiosik_KiosikID",
                table: "KiosikAds");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Items_ItemID",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Menu_MenuID",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_Items_ItemID",
                table: "TransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_PaymentMethod_PaymentMethodID",
                table: "TransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_TAXGroup_TAXID",
                table: "TransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_TransactionSummery_TransactionSummeryID",
                table: "TransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionSummery_Kiosik_KiosikID",
                table: "TransactionSummery");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionSummery_TransactionStauts_StautsID",
                table: "TransactionSummery");

            migrationBuilder.DropIndex(
                name: "IX_TransactionSummery_KiosikID",
                table: "TransactionSummery");

            migrationBuilder.DropIndex(
                name: "IX_TransactionSummery_StautsID",
                table: "TransactionSummery");

            migrationBuilder.DropIndex(
                name: "IX_TransactionDetails_ItemID",
                table: "TransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_TransactionDetails_PaymentMethodID",
                table: "TransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_TransactionDetails_TAXID",
                table: "TransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_TransactionDetails_TransactionSummeryID",
                table: "TransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_ItemID",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_MenuID",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_KiosikAds_AdsID",
                table: "KiosikAds");

            migrationBuilder.DropIndex(
                name: "IX_KiosikAds_KiosikID",
                table: "KiosikAds");

            migrationBuilder.DropIndex(
                name: "IX_Kiosik_BranchID",
                table: "Kiosik");

            migrationBuilder.DropIndex(
                name: "IX_Kiosik_BrandID",
                table: "Kiosik");

            migrationBuilder.DropIndex(
                name: "IX_Kiosik_CompID",
                table: "Kiosik");

            migrationBuilder.DropIndex(
                name: "IX_Kiosik_CustomerID",
                table: "Kiosik");

            migrationBuilder.DropIndex(
                name: "IX_Kiosik_MenuID",
                table: "Kiosik");

            migrationBuilder.DropIndex(
                name: "IX_ItemsPromotion_PromotionID",
                table: "ItemsPromotion");

            migrationBuilder.DropIndex(
                name: "IX_ItemsCategory_CategoryID",
                table: "ItemsCategory");

            migrationBuilder.DropIndex(
                name: "IX_ItemsCategory_ItemID",
                table: "ItemsCategory");

            migrationBuilder.DropIndex(
                name: "IX_Items_GroupID",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_GroupsItem_GroupID",
                table: "GroupsItem");

            migrationBuilder.DropIndex(
                name: "IX_GroupsItem_ItemID",
                table: "GroupsItem");
        }
    }
}
