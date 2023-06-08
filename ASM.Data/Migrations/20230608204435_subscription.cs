using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Data.Migrations
{
    public partial class subscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPlan",
                table: "PaymentInformations");

            migrationBuilder.DropColumn(
                name: "IsFreePeriod",
                table: "PaymentInformations");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "PaymentHistory");

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionPlanId",
                table: "PaymentInformations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PaymentHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionPlanId",
                table: "PaymentHistory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserPaymentId",
                table: "PaymentHistory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubscriptionPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    ValidateValue = table.Column<int>(type: "int", nullable: false),
                    ValidateType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlan", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInformations_SubscriptionPlanId",
                table: "PaymentInformations",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_SubscriptionPlanId",
                table: "PaymentHistory",
                column: "SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_SubscriptionPlan_SubscriptionPlanId",
                table: "PaymentHistory",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInformations_SubscriptionPlan_SubscriptionPlanId",
                table: "PaymentInformations",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlan",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_SubscriptionPlan_SubscriptionPlanId",
                table: "PaymentHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInformations_SubscriptionPlan_SubscriptionPlanId",
                table: "PaymentInformations");

            migrationBuilder.DropTable(
                name: "SubscriptionPlan");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInformations_SubscriptionPlanId",
                table: "PaymentInformations");

            migrationBuilder.DropIndex(
                name: "IX_PaymentHistory_SubscriptionPlanId",
                table: "PaymentHistory");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "PaymentInformations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PaymentHistory");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "PaymentHistory");

            migrationBuilder.DropColumn(
                name: "UserPaymentId",
                table: "PaymentHistory");

            migrationBuilder.AddColumn<string>(
                name: "CurrentPlan",
                table: "PaymentInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFreePeriod",
                table: "PaymentInformations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PaymentId",
                table: "PaymentHistory",
                type: "bigint",
                nullable: true);
        }
    }
}
