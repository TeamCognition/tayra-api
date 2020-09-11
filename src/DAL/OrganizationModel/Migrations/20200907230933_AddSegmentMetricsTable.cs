using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class AddSegmentMetricsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SegmentMetrics",
                columns: table => new
                {
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
                    table.PrimaryKey("PK_SegmentMetrics", x => new { x.SegmentId, x.Type, x.DateId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_SegmentMetrics_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegmentMetrics_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentMetrics_OrganizationId",
                table: "SegmentMetrics",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SegmentMetrics");
        }
    }
}
