using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsProfileOnly",
                table: "ActionPointProfiles",
                newName: "IsMemberOnly");

            migrationBuilder.CreateTable(
                name: "ActionPointProjects",
                columns: table => new
                {
                    ActionPointId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ConcludedOn = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPointProjects", x => new { x.ActionPointId, x.ProjectId, x.OrganizationId });
                    table.UniqueConstraint("AK_ActionPointProjects_ActionPointId_ProjectId", x => new { x.ActionPointId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ActionPointProjects_ActionPoints_ActionPointId",
                        column: x => x.ActionPointId,
                        principalTable: "ActionPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPointProjects_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionPointProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointProjects_OrganizationId",
                table: "ActionPointProjects",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointProjects_ProjectId",
                table: "ActionPointProjects",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionPointProjects");

            migrationBuilder.RenameColumn(
                name: "IsMemberOnly",
                table: "ActionPointProfiles",
                newName: "IsProfileOnly");
        }
    }
}
