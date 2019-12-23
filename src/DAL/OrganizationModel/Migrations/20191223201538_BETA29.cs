using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_Nickname",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "Profiles",
                newName: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Username",
                table: "Profiles",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_Username",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Profiles",
                newName: "Nickname");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Nickname",
                table: "Profiles",
                column: "Nickname",
                unique: true,
                filter: "[Nickname] IS NOT NULL");
        }
    }
}
