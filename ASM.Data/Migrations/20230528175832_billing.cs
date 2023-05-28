using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Data.Migrations
{
    public partial class billing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentInformations_SellerId",
                table: "PaymentInformations");

            migrationBuilder.AlterColumn<Guid>(
                name: "SellerId",
                table: "PaymentInformations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPayment",
                table: "PaymentInformations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MetaData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInformations_SellerId",
                table: "PaymentInformations",
                column: "SellerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_SellerId",
                table: "PaymentHistory",
                column: "SellerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInformations_SellerId",
                table: "PaymentInformations");

            migrationBuilder.DropColumn(
                name: "LastPayment",
                table: "PaymentInformations");

            migrationBuilder.AlterColumn<Guid>(
                name: "SellerId",
                table: "PaymentInformations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInformations_SellerId",
                table: "PaymentInformations",
                column: "SellerId",
                unique: true,
                filter: "[SellerId] IS NOT NULL");
        }
    }
}
