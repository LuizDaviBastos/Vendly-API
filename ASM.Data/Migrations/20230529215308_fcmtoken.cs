using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Data.Migrations
{
    public partial class fcmtoken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FcmToken",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FcmToken",
                table: "Sellers");
        }
    }
}
