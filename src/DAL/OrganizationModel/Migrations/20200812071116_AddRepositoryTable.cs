using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class AddRepositoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true),
                    IntegrationInstallationId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameWithOwner = table.Column<string>(nullable: true),
                    PrimaryLanguage = table.Column<string>(nullable: true),
                    ExternalUrl = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Repositories_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repositories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Repositories_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_OrganizationId",
                table: "Repositories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_TeamId_OrganizationId",
                table: "Repositories",
                columns: new[] { "TeamId", "OrganizationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Repositories");
        }
    }
}
