using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Data.Migrations
{
    public partial class changemessagestatusflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageStatus",
                table: "SellerOrder");

            migrationBuilder.AddColumn<bool>(
                name: "AfterSellerMessageStatus",
                table: "SellerOrder",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveredMessageStatus",
                table: "SellerOrder",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShippingMessageStatus",
                table: "SellerOrder",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfterSellerMessageStatus",
                table: "SellerOrder");

            migrationBuilder.DropColumn(
                name: "DeliveredMessageStatus",
                table: "SellerOrder");

            migrationBuilder.DropColumn(
                name: "ShippingMessageStatus",
                table: "SellerOrder");

            migrationBuilder.AddColumn<int>(
                name: "MessageStatus",
                table: "SellerOrder",
                type: "int",
                nullable: true);
        }
    }
}
