using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class AddGitCommitsAndProfileAnalytics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IntegrationType",
                table: "WebhookEventLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnalyticsEnabled",
                table: "Profiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GitCommits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    SHA = table.Column<string>(nullable: true),
                    ExternalUrl = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    AuthorExternalId = table.Column<string>(nullable: true),
                    AuthorProfileId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitCommits", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_GitCommits_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GitCommits_Profiles_AuthorProfileId",
                        column: x => x.AuthorProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GitCommits_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GitCommits_OrganizationId",
                table: "GitCommits",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_GitCommits_AuthorProfileId_OrganizationId",
                table: "GitCommits",
                columns: new[] { "AuthorProfileId", "OrganizationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GitCommits");

            migrationBuilder.DropColumn(
                name: "IntegrationType",
                table: "WebhookEventLogs");

            migrationBuilder.DropColumn(
                name: "IsAnalyticsEnabled",
                table: "Profiles");
        }
    }
}
