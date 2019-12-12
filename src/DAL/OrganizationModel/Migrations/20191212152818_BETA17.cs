using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    DateId = table.Column<int>(nullable: false),
                    ConcludedOn = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPoints", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ActionPoints_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionPoints_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionPointSettings",
                columns: table => new
                {
                    Type = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    NotifyByEmail = table.Column<bool>(nullable: false),
                    NotifyByPush = table.Column<bool>(nullable: false),
                    NotifyByNotification = table.Column<bool>(nullable: false),
                    MuteUntil = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPointSettings", x => new { x.Type, x.OrganizationId });
                    table.UniqueConstraint("AK_ActionPointSettings_Type", x => x.Type);
                    table.ForeignKey(
                        name: "FK_ActionPointSettings_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionPointSettings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Purpose = table.Column<int>(nullable: false),
                    Filename = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Filesize = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blobs", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Blobs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blobs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionPointProfiles",
                columns: table => new
                {
                    ActionPointId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IsProfileOnly = table.Column<bool>(nullable: false),
                    ConcludedOn = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPointProfiles", x => new { x.ActionPointId, x.ProfileId, x.OrganizationId });
                    table.UniqueConstraint("AK_ActionPointProfiles_ActionPointId_ProfileId", x => new { x.ActionPointId, x.ProfileId });
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

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointProfiles_OrganizationId",
                table: "ActionPointProfiles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointProfiles_ProfileId",
                table: "ActionPointProfiles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPoints_OrganizationId",
                table: "ActionPoints",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSettings_OrganizationId",
                table: "ActionPointSettings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSettings_ProjectId",
                table: "ActionPointSettings",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_OrganizationId",
                table: "Blobs",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionPointProfiles");

            migrationBuilder.DropTable(
                name: "ActionPointSettings");

            migrationBuilder.DropTable(
                name: "Blobs");

            migrationBuilder.DropTable(
                name: "ActionPoints");
        }
    }
}
