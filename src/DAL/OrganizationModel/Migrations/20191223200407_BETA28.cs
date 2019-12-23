using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileExternalIds",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    IntegrationType = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileExternalIds", x => new { x.ProfileId, x.ProjectId, x.IntegrationType, x.OrganizationId });
                    table.UniqueConstraint("AK_ProfileExternalIds_ProfileId_ProjectId_IntegrationType", x => new { x.ProfileId, x.ProjectId, x.IntegrationType });
                    table.ForeignKey(
                        name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileExternalIds_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileExternalIds_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_OrganizationId",
                table: "ProfileExternalIds",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_ProjectId",
                table: "ProfileExternalIds",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileExternalIds");
        }
    }
}
