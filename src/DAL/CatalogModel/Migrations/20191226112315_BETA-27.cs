using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Catalog.Migrations
{
    public partial class BETA27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityEmails_IdentityId_IsPrimary",
                table: "IdentityEmails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IdentityEmails_IdentityId_IsPrimary",
                table: "IdentityEmails",
                columns: new[] { "IdentityId", "IsPrimary" },
                unique: true);
        }
    }
}
