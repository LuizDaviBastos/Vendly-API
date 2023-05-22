using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Data.Migrations
{
    public partial class addcascate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeliAccounts_Sellers_SellerId",
                table: "MeliAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInformations_Sellers_SellerId",
                table: "PaymentInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerMessages_MeliAccounts_MeliAccountId",
                table: "SellerMessages");

            migrationBuilder.AlterColumn<Guid>(
                name: "MeliAccountId",
                table: "SellerMessages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SellerId",
                table: "MeliAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MeliAccounts_Sellers_SellerId",
                table: "MeliAccounts",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInformations_Sellers_SellerId",
                table: "PaymentInformations",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerMessages_MeliAccounts_MeliAccountId",
                table: "SellerMessages",
                column: "MeliAccountId",
                principalTable: "MeliAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeliAccounts_Sellers_SellerId",
                table: "MeliAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInformations_Sellers_SellerId",
                table: "PaymentInformations");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerMessages_MeliAccounts_MeliAccountId",
                table: "SellerMessages");

            migrationBuilder.AlterColumn<Guid>(
                name: "MeliAccountId",
                table: "SellerMessages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "SellerId",
                table: "MeliAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_MeliAccounts_Sellers_SellerId",
                table: "MeliAccounts",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInformations_Sellers_SellerId",
                table: "PaymentInformations",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerMessages_MeliAccounts_MeliAccountId",
                table: "SellerMessages",
                column: "MeliAccountId",
                principalTable: "MeliAccounts",
                principalColumn: "Id");
        }
    }
}
