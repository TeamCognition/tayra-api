using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Catalog.Migrations
{
    public partial class BETA28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityEmails_Email_DeletedAt",
                table: "IdentityEmails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IdentityEmails_Email_DeletedAt",
                table: "IdentityEmails",
                columns: new[] { "Email", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");
        }
    }
}
