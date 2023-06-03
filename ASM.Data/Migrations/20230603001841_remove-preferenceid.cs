using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Data.Migrations
{
    public partial class removepreferenceid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferenceId",
                table: "Sellers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferenceId",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
