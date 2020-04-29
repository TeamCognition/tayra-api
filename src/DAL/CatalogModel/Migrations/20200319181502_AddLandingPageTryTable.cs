using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Catalog.Migrations
{
    public partial class AddLandingPageTryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LandingPageContact",
                table: "LandingPageContact");

            migrationBuilder.RenameTable(
                name: "LandingPageContact",
                newName: "LandingPageContacts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LandingPageContacts",
                table: "LandingPageContacts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LandingPageTry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandingPageTry", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandingPageTry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LandingPageContacts",
                table: "LandingPageContacts");

            migrationBuilder.RenameTable(
                name: "LandingPageContacts",
                newName: "LandingPageContact");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LandingPageContact",
                table: "LandingPageContact",
                column: "Id");
        }
    }
}
