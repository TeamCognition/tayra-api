using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionPointSettings_Projects_ProjectId",
                table: "ActionPointSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Projects_ProjectId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Projects_ProjectId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Integrations_Projects_ProjectId",
                table: "Integrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Projects_ProjectId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileExternalIds_Projects_ProjectId",
                table: "ProfileExternalIds");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsDaily_Projects_ProjectId",
                table: "ProjectReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsWeekly_Projects_ProjectId",
                table: "ProjectReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeams_Projects_ProjectId",
                table: "ProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopPurchases_Projects_ProjectId",
                table: "ShopPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_Projects_ProjectId",
                table: "TaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ProjectAreas_ProjectAreaId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Projects_ProjectId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "ActionPointProjects");

            migrationBuilder.DropTable(
                name: "ShopItemProjects");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectTeams_ProjectId_TeamId",
                table: "ProjectTeams");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectReportsWeekly_DateId_ProjectId_TaskCategoryId",
                table: "ProjectReportsWeekly");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectReportsDaily_DateId_ProjectId_TaskCategoryId",
                table: "ProjectReportsDaily");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectMembers_ProjectId_ProfileId",
                table: "ProjectMembers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProfileExternalIds_ProfileId_ProjectId_IntegrationType",
                table: "ProfileExternalIds");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Teams",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_ProjectId",
                table: "Teams",
                newName: "IX_Teams_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Tasks",
                newName: "SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectAreaId",
                table: "Tasks",
                newName: "SegmentAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectAreaId",
                table: "Tasks",
                newName: "IX_Tasks_SegmentAreaId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "TaskLogs",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLogs_ProjectId",
                table: "TaskLogs",
                newName: "IX_TaskLogs_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ShopPurchases",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopPurchases_ProjectId",
                table: "ShopPurchases",
                newName: "IX_ShopPurchases_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ProjectTeams",
                newName: "SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ProjectReportsWeekly",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsWeekly_ProjectId",
                table: "ProjectReportsWeekly",
                newName: "IX_ProjectReportsWeekly_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ProjectReportsDaily",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsDaily_ProjectId",
                table: "ProjectReportsDaily",
                newName: "IX_ProjectReportsDaily_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ProjectMembers",
                newName: "SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ProfileExternalIds",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileExternalIds_ProjectId",
                table: "ProfileExternalIds",
                newName: "IX_ProfileExternalIds_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Invitations",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_ProjectId",
                table: "Invitations",
                newName: "IX_Invitations_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Integrations",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Integrations_ProfileId_ProjectId",
                table: "Integrations",
                newName: "IX_Integrations_ProfileId_SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Integrations_ProjectId",
                table: "Integrations",
                newName: "IX_Integrations_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Competitions",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Competitions_ProjectId",
                table: "Competitions",
                newName: "IX_Competitions_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Challenges",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Challenges_ProjectId",
                table: "Challenges",
                newName: "IX_Challenges_SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ActionPointSettings",
                newName: "SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ActionPointSettings_ProjectId",
                table: "ActionPointSettings",
                newName: "IX_ActionPointSettings_SegmentId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectTeams_SegmentId_TeamId",
                table: "ProjectTeams",
                columns: new[] { "SegmentId", "TeamId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectReportsWeekly_DateId_SegmentId_TaskCategoryId",
                table: "ProjectReportsWeekly",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectReportsDaily_DateId_SegmentId_TaskCategoryId",
                table: "ProjectReportsDaily",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectMembers_SegmentId_ProfileId",
                table: "ProjectMembers",
                columns: new[] { "SegmentId", "ProfileId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileExternalIds_ProfileId_SegmentId_IntegrationType",
                table: "ProfileExternalIds",
                columns: new[] { "ProfileId", "SegmentId", "IntegrationType" });

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
                    table.UniqueConstraint("AK_ActionPointSegments_ActionPointId_SegmentId", x => new { x.ActionPointId, x.SegmentId });
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
                        name: "FK_ActionPointSegments_Projects_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopItemSegments",
                columns: table => new
                {
                    ShopItemId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    DiscountPrice = table.Column<float>(nullable: true),
                    DiscountEndsAt = table.Column<DateTime>(nullable: true),
                    HiddenAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemSegments", x => new { x.ShopItemId, x.SegmentId, x.OrganizationId });
                    table.UniqueConstraint("AK_ShopItemSegments_ShopItemId_SegmentId", x => new { x.ShopItemId, x.SegmentId });
                    table.ForeignKey(
                        name: "FK_ShopItemSegments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopItemSegments_Projects_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopItemSegments_ShopItems_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSegments_OrganizationId",
                table: "ActionPointSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSegments_SegmentId",
                table: "ActionPointSegments",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemSegments_OrganizationId",
                table: "ShopItemSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemSegments_SegmentId",
                table: "ShopItemSegments",
                column: "SegmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPointSettings_Projects_SegmentId",
                table: "ActionPointSettings",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_Projects_SegmentId",
                table: "Challenges",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Projects_SegmentId",
                table: "Competitions",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Integrations_Projects_SegmentId",
                table: "Integrations",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Projects_SegmentId",
                table: "Invitations",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileExternalIds_Projects_SegmentId",
                table: "ProfileExternalIds",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_SegmentId",
                table: "ProjectMembers",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReportsDaily_Projects_SegmentId",
                table: "ProjectReportsDaily",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReportsWeekly_Projects_SegmentId",
                table: "ProjectReportsWeekly",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeams_Projects_SegmentId",
                table: "ProjectTeams",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopPurchases_Projects_SegmentId",
                table: "ShopPurchases",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_Projects_SegmentId",
                table: "TaskLogs",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ProjectAreas_SegmentAreaId",
                table: "Tasks",
                column: "SegmentAreaId",
                principalTable: "ProjectAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_SegmentId",
                table: "Tasks",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Projects_SegmentId",
                table: "Teams",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionPointSettings_Projects_SegmentId",
                table: "ActionPointSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Projects_SegmentId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Projects_SegmentId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Integrations_Projects_SegmentId",
                table: "Integrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Projects_SegmentId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileExternalIds_Projects_SegmentId",
                table: "ProfileExternalIds");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_SegmentId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsDaily_Projects_SegmentId",
                table: "ProjectReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsWeekly_Projects_SegmentId",
                table: "ProjectReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeams_Projects_SegmentId",
                table: "ProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopPurchases_Projects_SegmentId",
                table: "ShopPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_Projects_SegmentId",
                table: "TaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ProjectAreas_SegmentAreaId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_SegmentId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Projects_SegmentId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "ActionPointSegments");

            migrationBuilder.DropTable(
                name: "ShopItemSegments");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectTeams_SegmentId_TeamId",
                table: "ProjectTeams");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectReportsWeekly_DateId_SegmentId_TaskCategoryId",
                table: "ProjectReportsWeekly");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectReportsDaily_DateId_SegmentId_TaskCategoryId",
                table: "ProjectReportsDaily");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectMembers_SegmentId_ProfileId",
                table: "ProjectMembers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProfileExternalIds_ProfileId_SegmentId_IntegrationType",
                table: "ProfileExternalIds");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "Teams",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_SegmentId",
                table: "Teams",
                newName: "IX_Teams_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "Tasks",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentAreaId",
                table: "Tasks",
                newName: "ProjectAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SegmentId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SegmentAreaId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectAreaId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "TaskLogs",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLogs_SegmentId",
                table: "TaskLogs",
                newName: "IX_TaskLogs_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "ShopPurchases",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopPurchases_SegmentId",
                table: "ShopPurchases",
                newName: "IX_ShopPurchases_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "ProjectTeams",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "ProjectReportsWeekly",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsWeekly_SegmentId",
                table: "ProjectReportsWeekly",
                newName: "IX_ProjectReportsWeekly_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "ProjectReportsDaily",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsDaily_SegmentId",
                table: "ProjectReportsDaily",
                newName: "IX_ProjectReportsDaily_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "ProjectMembers",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "ProfileExternalIds",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileExternalIds_SegmentId",
                table: "ProfileExternalIds",
                newName: "IX_ProfileExternalIds_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "Invitations",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_SegmentId",
                table: "Invitations",
                newName: "IX_Invitations_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "Integrations",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Integrations_ProfileId_SegmentId",
                table: "Integrations",
                newName: "IX_Integrations_ProfileId_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Integrations_SegmentId",
                table: "Integrations",
                newName: "IX_Integrations_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "Competitions",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Competitions_SegmentId",
                table: "Competitions",
                newName: "IX_Competitions_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "Challenges",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Challenges_SegmentId",
                table: "Challenges",
                newName: "IX_Challenges_ProjectId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "ActionPointSettings",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ActionPointSettings_SegmentId",
                table: "ActionPointSettings",
                newName: "IX_ActionPointSettings_ProjectId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectTeams_ProjectId_TeamId",
                table: "ProjectTeams",
                columns: new[] { "ProjectId", "TeamId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectReportsWeekly_DateId_ProjectId_TaskCategoryId",
                table: "ProjectReportsWeekly",
                columns: new[] { "DateId", "ProjectId", "TaskCategoryId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectReportsDaily_DateId_ProjectId_TaskCategoryId",
                table: "ProjectReportsDaily",
                columns: new[] { "DateId", "ProjectId", "TaskCategoryId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectMembers_ProjectId_ProfileId",
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "ProfileId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileExternalIds_ProfileId_ProjectId_IntegrationType",
                table: "ProfileExternalIds",
                columns: new[] { "ProfileId", "ProjectId", "IntegrationType" });

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

            migrationBuilder.CreateTable(
                name: "ShopItemProjects",
                columns: table => new
                {
                    ShopItemId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    DiscountEndsAt = table.Column<DateTime>(nullable: true),
                    DiscountPrice = table.Column<float>(nullable: true),
                    HiddenAt = table.Column<DateTime>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemProjects", x => new { x.ShopItemId, x.ProjectId, x.OrganizationId });
                    table.UniqueConstraint("AK_ShopItemProjects_ShopItemId_ProjectId", x => new { x.ShopItemId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ShopItemProjects_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopItemProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopItemProjects_ShopItems_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItems",
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

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProjects_OrganizationId",
                table: "ShopItemProjects",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProjects_ProjectId",
                table: "ShopItemProjects",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPointSettings_Projects_ProjectId",
                table: "ActionPointSettings",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_Projects_ProjectId",
                table: "Challenges",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Projects_ProjectId",
                table: "Competitions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Integrations_Projects_ProjectId",
                table: "Integrations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Projects_ProjectId",
                table: "Invitations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileExternalIds_Projects_ProjectId",
                table: "ProfileExternalIds",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReportsDaily_Projects_ProjectId",
                table: "ProjectReportsDaily",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReportsWeekly_Projects_ProjectId",
                table: "ProjectReportsWeekly",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeams_Projects_ProjectId",
                table: "ProjectTeams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopPurchases_Projects_ProjectId",
                table: "ShopPurchases",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_Projects_ProjectId",
                table: "TaskLogs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ProjectAreas_ProjectAreaId",
                table: "Tasks",
                column: "ProjectAreaId",
                principalTable: "ProjectAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Projects_ProjectId",
                table: "Teams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
