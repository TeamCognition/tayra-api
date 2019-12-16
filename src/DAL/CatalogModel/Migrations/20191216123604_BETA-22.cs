using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Catalog.Migrations
{
    public partial class BETA22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Identities_Username",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Identities");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Identities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Identities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityEmails_Email_IsPrimary",
                table: "IdentityEmails",
                columns: new[] { "Email", "IsPrimary" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityEmails_Email_IsPrimary",
                table: "IdentityEmails");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Identities");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Identities",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Identities_Username",
                table: "Identities",
                column: "Username",
                unique: true);
        }
    }
}
