using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Data.Migrations
{
    public partial class addedpreferenceIdinuser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferenceId",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferenceId",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
