using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Catalog.Migrations
{
    public partial class BETA31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tenants",
                newName: "Key");

            migrationBuilder.RenameIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                newName: "IX_Tenants_Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Tenants",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Tenants_Key",
                table: "Tenants",
                newName: "IX_Tenants_Name");
        }
    }
}
