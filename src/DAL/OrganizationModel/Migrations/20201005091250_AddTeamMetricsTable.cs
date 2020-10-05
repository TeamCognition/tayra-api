using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class AddTeamMetricsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamMetrics",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    DateId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Value = table.Column<float>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMetrics", x => new { x.TeamId, x.Type, x.DateId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_TeamMetrics_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMetrics_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMetrics_OrganizationId",
                table: "TeamMetrics",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMetrics");
        }
    }
}
