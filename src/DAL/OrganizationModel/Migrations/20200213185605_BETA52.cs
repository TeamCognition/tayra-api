using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA52 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProfileExternalIds_ExternalId_IntegrationType",
                table: "ProfileExternalIds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_ExternalId_IntegrationType",
                table: "ProfileExternalIds",
                columns: new[] { "ExternalId", "IntegrationType" });
        }
    }
}
