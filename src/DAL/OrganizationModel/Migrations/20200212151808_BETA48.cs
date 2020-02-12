using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA48 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.CreateTable(
                name: "ProfileAssignments",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileAssignments", x => new { x.SegmentId, x.TeamId, x.ProfileId, x.OrganizationId });
                    table.UniqueConstraint("AK_ProfileAssignments_SegmentId_TeamId_ProfileId", x => new { x.SegmentId, x.TeamId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_ProfileAssignments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileAssignments_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileAssignments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileAssignments_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_OrganizationId",
                table: "ProfileAssignments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_ProfileId",
                table: "ProfileAssignments",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_ProfileId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "ProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_TeamId_ProfileId",
                table: "ProfileAssignments",
                columns: new[] { "TeamId", "ProfileId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileAssignments");

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => new { x.TeamId, x.ProfileId, x.OrganizationId });
                    table.UniqueConstraint("AK_TeamMembers_TeamId_ProfileId", x => new { x.TeamId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_TeamMembers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_OrganizationId",
                table: "TeamMembers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_ProfileId",
                table: "TeamMembers",
                column: "ProfileId");
        }
    }
}
