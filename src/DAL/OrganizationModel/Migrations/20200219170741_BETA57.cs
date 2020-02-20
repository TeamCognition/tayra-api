using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA57 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShopItems");

            migrationBuilder.RenameColumn(
                name: "WorthValue",
                table: "Items",
                newName: "Price");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Items",
                newName: "WorthValue");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "ShopItems",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
