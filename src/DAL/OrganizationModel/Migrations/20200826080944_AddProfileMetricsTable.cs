using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class AddProfileMetricsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileMetrics",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    DateId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Value = table.Column<float>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileMetrics", x => new { x.ProfileId, x.SegmentId, x.Type, x.DateId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileMetrics_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileMetrics_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileMetrics_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileMetrics_OrganizationId",
                table: "ProfileMetrics",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileMetrics_SegmentId_OrganizationId",
                table: "ProfileMetrics",
                columns: new[] { "SegmentId", "OrganizationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileMetrics");
        }
    }
}
