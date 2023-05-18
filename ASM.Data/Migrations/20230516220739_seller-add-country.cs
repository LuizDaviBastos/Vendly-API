using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Data.Migrations
{
    public partial class selleraddcountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Sellers",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationCode",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Sellers");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Sellers",
                newName: "LastName");
        }
    }
}
