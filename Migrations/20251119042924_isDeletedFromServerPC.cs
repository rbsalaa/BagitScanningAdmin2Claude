using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfKOrderWeb.Migrations
{
    public partial class isDeletedFromServerPC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TransactionSummery",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "TransactionSummery",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TransactionStauts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "TransactionStauts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TransactionDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "TransactionDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TAXGroup",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "TAXGroup",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StoreMenuDiscounts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "StoreMenuDiscounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Promotion",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Promotion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PaymentMethod",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "PaymentMethod",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "OrderDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Modifiers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Modifiers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ModifierModifierCategory",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ModifierModifierCategory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ModifierCategory",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ModifierCategory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MenuSuggestions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "MenuSuggestions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MenuItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "MenuItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Menu",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MDFileUpload",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "MDFileUpload",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KiosikHardwareLog",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "KiosikHardwareLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KiosikBanner",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "KiosikBanner",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KiosikAds",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "KiosikAds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Kiosik",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Kiosik",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemsPromotion",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ItemsPromotion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemsCategory",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ItemsCategory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemModifier",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ItemModifier",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GroupsItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "GroupsItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Groups",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Groups",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Discounts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Discounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Category",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Banner",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Banner",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Advertisment",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Advertisment",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TransactionSummery");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "TransactionSummery");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TransactionStauts");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "TransactionStauts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TAXGroup");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "TAXGroup");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StoreMenuDiscounts");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "StoreMenuDiscounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ModifierModifierCategory");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ModifierModifierCategory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ModifierCategory");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ModifierCategory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MenuSuggestions");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "MenuSuggestions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MDFileUpload");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "MDFileUpload");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "KiosikHardwareLog");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "KiosikHardwareLog");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "KiosikBanner");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "KiosikBanner");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "KiosikAds");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "KiosikAds");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Kiosik");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Kiosik");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemsPromotion");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ItemsPromotion");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemsCategory");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ItemsCategory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemModifier");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ItemModifier");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GroupsItem");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "GroupsItem");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Banner");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Banner");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Advertisment");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Advertisment");
        }
    }
}
