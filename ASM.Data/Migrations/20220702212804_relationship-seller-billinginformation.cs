using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASM.Data.Migrations
{
    public partial class relationshipsellerbillinginformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BillingInformationId",
                table: "Sellers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PaymentInformation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: true),
                    ExpireIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SellerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentInformation_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInformation_SellerId",
                table: "PaymentInformation",
                column: "SellerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentInformation");

            migrationBuilder.DropColumn(
                name: "BillingInformationId",
                table: "Sellers");
        }
    }
}
