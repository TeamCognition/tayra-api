using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SegmentTeams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SegmentTeams",
                columns: table => new
                {
                    SegmentId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentTeams", x => new { x.SegmentId, x.TeamId, x.OrganizationId });
                    table.UniqueConstraint("AK_SegmentTeams_SegmentId_TeamId", x => new { x.SegmentId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_SegmentTeams_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegmentTeams_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SegmentTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentTeams_OrganizationId",
                table: "SegmentTeams",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentTeams_TeamId",
                table: "SegmentTeams",
                column: "TeamId");
        }
    }
}
