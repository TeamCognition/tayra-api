using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeams_Organizations_OrganizationId",
                table: "ProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeams_Segments_SegmentId",
                table: "ProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeams_Teams_TeamId",
                table: "ProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_Segments_Organizations_OrganizationId",
                table: "Segments");

            migrationBuilder.DropTable(
                name: "SegmentMembers");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_Key_ArchivedAt",
                table: "Teams");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectTeams_SegmentId_TeamId",
                table: "ProjectTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTeams",
                table: "ProjectTeams");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "ProjectTeams",
                newName: "SegmentTeams");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTeams_TeamId",
                table: "SegmentTeams",
                newName: "IX_SegmentTeams_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTeams_OrganizationId",
                table: "SegmentTeams",
                newName: "IX_SegmentTeams_OrganizationId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarColor",
                table: "Teams",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SegmentTeams_SegmentId_TeamId",
                table: "SegmentTeams",
                columns: new[] { "SegmentId", "TeamId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentTeams",
                table: "SegmentTeams",
                columns: new[] { "SegmentId", "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchivedAt" },
                unique: true,
                filter: "[Key] IS NOT NULL AND [ArchivedAt] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Segments_Organizations_OrganizationId",
                table: "Segments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentTeams_Organizations_OrganizationId",
                table: "SegmentTeams",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentTeams_Segments_SegmentId",
                table: "SegmentTeams",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentTeams_Teams_TeamId",
                table: "SegmentTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Segments_Organizations_OrganizationId",
                table: "Segments");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentTeams_Organizations_OrganizationId",
                table: "SegmentTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentTeams_Segments_SegmentId",
                table: "SegmentTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentTeams_Teams_TeamId",
                table: "SegmentTeams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt",
                table: "Teams");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SegmentTeams_SegmentId_TeamId",
                table: "SegmentTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentTeams",
                table: "SegmentTeams");

            migrationBuilder.DropColumn(
                name: "AvatarColor",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "SegmentTeams",
                newName: "ProjectTeams");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentTeams_TeamId",
                table: "ProjectTeams",
                newName: "IX_ProjectTeams_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentTeams_OrganizationId",
                table: "ProjectTeams",
                newName: "IX_ProjectTeams_OrganizationId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Teams",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectTeams_SegmentId_TeamId",
                table: "ProjectTeams",
                columns: new[] { "SegmentId", "TeamId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTeams",
                table: "ProjectTeams",
                columns: new[] { "SegmentId", "TeamId", "OrganizationId" });

            migrationBuilder.CreateTable(
                name: "SegmentMembers",
                columns: table => new
                {
                    SegmentId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentMembers", x => new { x.SegmentId, x.ProfileId, x.OrganizationId });
                    table.UniqueConstraint("AK_SegmentMembers_SegmentId_ProfileId", x => new { x.SegmentId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_SegmentMembers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegmentMembers_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SegmentMembers_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId",
                table: "Teams",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Key_ArchivedAt",
                table: "Teams",
                columns: new[] { "Key", "ArchivedAt" },
                unique: true,
                filter: "[Key] IS NOT NULL AND [ArchivedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentMembers_OrganizationId",
                table: "SegmentMembers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentMembers_ProfileId",
                table: "SegmentMembers",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeams_Organizations_OrganizationId",
                table: "ProjectTeams",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeams_Segments_SegmentId",
                table: "ProjectTeams",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeams_Teams_TeamId",
                table: "ProjectTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Segments_Organizations_OrganizationId",
                table: "Segments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
