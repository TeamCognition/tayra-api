using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA66 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionPointProfiles");

            migrationBuilder.DropTable(
                name: "ActionPointSegments");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "ActionPoints",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "ActionPoints",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SegmentId",
                table: "ActionPoints",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionPoints_ProfileId_OrganizationId",
                table: "ActionPoints",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPoints_SegmentId_OrganizationId",
                table: "ActionPoints",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPoints_Profiles_ProfileId",
                table: "ActionPoints",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPoints_Segments_SegmentId",
                table: "ActionPoints",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionPoints_Profiles_ProfileId",
                table: "ActionPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionPoints_Segments_SegmentId",
                table: "ActionPoints");

            migrationBuilder.DropIndex(
                name: "IX_ActionPoints_ProfileId_OrganizationId",
                table: "ActionPoints");

            migrationBuilder.DropIndex(
                name: "IX_ActionPoints_SegmentId_OrganizationId",
                table: "ActionPoints");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "ActionPoints");

            migrationBuilder.DropColumn(
                name: "SegmentId",
                table: "ActionPoints");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "ActionPoints",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "ActionPointProfiles",
                columns: table => new
                {
                    ActionPointId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ConcludedOn = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    IsMemberOnly = table.Column<bool>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPointProfiles", x => new { x.ActionPointId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ActionPointProfiles_ActionPoints_ActionPointId",
                        column: x => x.ActionPointId,
                        principalTable: "ActionPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPointProfiles_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionPointProfiles_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionPointSegments",
                columns: table => new
                {
                    ActionPointId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ConcludedOn = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPointSegments", x => new { x.ActionPointId, x.SegmentId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ActionPointSegments_ActionPoints_ActionPointId",
                        column: x => x.ActionPointId,
                        principalTable: "ActionPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPointSegments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionPointSegments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointProfiles_OrganizationId",
                table: "ActionPointProfiles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointProfiles_ProfileId_OrganizationId",
                table: "ActionPointProfiles",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSegments_OrganizationId",
                table: "ActionPointSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSegments_SegmentId_OrganizationId",
                table: "ActionPointSegments",
                columns: new[] { "SegmentId", "OrganizationId" });
        }
    }
}
