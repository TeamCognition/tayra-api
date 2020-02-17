using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                table: "ProfileExternalIds");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProfileExternalIds_ExternalId_IntegrationType",
                table: "ProfileExternalIds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds");

            migrationBuilder.DropIndex(
                name: "IX_ProfileExternalIds_OrganizationId",
                table: "ProfileExternalIds");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds",
                columns: new[] { "OrganizationId", "ExternalId", "IntegrationType" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_ExternalId_IntegrationType",
                table: "ProfileExternalIds",
                columns: new[] { "ExternalId", "IntegrationType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds");

            migrationBuilder.DropIndex(
                name: "IX_ProfileExternalIds_ExternalId_IntegrationType",
                table: "ProfileExternalIds");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileExternalIds_ExternalId_IntegrationType",
                table: "ProfileExternalIds",
                columns: new[] { "ExternalId", "IntegrationType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds",
                columns: new[] { "ExternalId", "IntegrationType", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_OrganizationId",
                table: "ProfileExternalIds",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                table: "ProfileExternalIds",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
