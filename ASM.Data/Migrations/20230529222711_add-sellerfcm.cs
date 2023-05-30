using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Data.Migrations
{
    public partial class addsellerfcm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FcmToken",
                table: "Sellers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FcmToken",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
