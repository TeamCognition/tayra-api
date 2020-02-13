using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA50 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProfileExternalIds_ProfileId_SegmentId_IntegrationType",
                table: "ProfileExternalIds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                table: "ProfileExternalIds",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileExternalIds_ExternalId_IntegrationType",
                table: "ProfileExternalIds",
                columns: new[] { "ExternalId", "IntegrationType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds",
                columns: new[] { "ExternalId", "IntegrationType", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_ProfileId",
                table: "ProfileExternalIds",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProfileExternalIds_ExternalId_IntegrationType",
                table: "ProfileExternalIds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds");

            migrationBuilder.DropIndex(
                name: "IX_ProfileExternalIds_ProfileId",
                table: "ProfileExternalIds");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                table: "ProfileExternalIds",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileExternalIds_ProfileId_SegmentId_IntegrationType",
                table: "ProfileExternalIds",
                columns: new[] { "ProfileId", "SegmentId", "IntegrationType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds",
                columns: new[] { "ProfileId", "SegmentId", "IntegrationType", "OrganizationId" });
        }
    }
}
