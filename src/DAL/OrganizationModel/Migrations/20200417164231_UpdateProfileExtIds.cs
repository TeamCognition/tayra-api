using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class UpdateProfileExtIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds",
                columns: new[] { "ExternalId", "IntegrationType", "SegmentId", "OrganizationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds",
                columns: new[] { "ExternalId", "IntegrationType", "OrganizationId" });
        }
    }
}
