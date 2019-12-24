using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionPointSegments_Projects_SegmentId",
                table: "ActionPointSegments");

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
                name: "FK_ProjectAreas_Organizations_OrganizationId",
                table: "ProjectAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Organizations_OrganizationId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Profiles_ProfileId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_SegmentId",
                table: "ProjectMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsDaily_Organizations_OrganizationId",
                table: "ProjectReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsDaily_Projects_SegmentId",
                table: "ProjectReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsDaily_TaskCategories_TaskCategoryId",
                table: "ProjectReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsWeekly_Organizations_OrganizationId",
                table: "ProjectReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsWeekly_Projects_SegmentId",
                table: "ProjectReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectReportsWeekly_TaskCategories_TaskCategoryId",
                table: "ProjectReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Organizations_OrganizationId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeams_Projects_SegmentId",
                table: "ProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemSegments_Projects_SegmentId",
                table: "ShopItemSegments");

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

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Projects_Id",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectReportsWeekly_DateId_SegmentId_TaskCategoryId",
                table: "ProjectReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectReportsWeekly",
                table: "ProjectReportsWeekly");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectReportsDaily_DateId_SegmentId_TaskCategoryId",
                table: "ProjectReportsDaily");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectReportsDaily",
                table: "ProjectReportsDaily");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectMembers_SegmentId_ProfileId",
                table: "ProjectMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectAreas_Id",
                table: "ProjectAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectAreas",
                table: "ProjectAreas");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Segments");

            migrationBuilder.RenameTable(
                name: "ProjectReportsWeekly",
                newName: "SegmentReportsWeekly");

            migrationBuilder.RenameTable(
                name: "ProjectReportsDaily",
                newName: "SegmentReportsDaily");

            migrationBuilder.RenameTable(
                name: "ProjectMembers",
                newName: "SegmentMembers");

            migrationBuilder.RenameTable(
                name: "ProjectAreas",
                newName: "SegmentAreas");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_OrganizationId",
                table: "Segments",
                newName: "IX_Segments_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_Key",
                table: "Segments",
                newName: "IX_Segments_Key");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsWeekly_TaskCategoryId",
                table: "SegmentReportsWeekly",
                newName: "IX_SegmentReportsWeekly_TaskCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsWeekly_SegmentId",
                table: "SegmentReportsWeekly",
                newName: "IX_SegmentReportsWeekly_SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsWeekly_OrganizationId",
                table: "SegmentReportsWeekly",
                newName: "IX_SegmentReportsWeekly_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsDaily_TaskCategoryId",
                table: "SegmentReportsDaily",
                newName: "IX_SegmentReportsDaily_TaskCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsDaily_SegmentId",
                table: "SegmentReportsDaily",
                newName: "IX_SegmentReportsDaily_SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectReportsDaily_OrganizationId",
                table: "SegmentReportsDaily",
                newName: "IX_SegmentReportsDaily_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_ProfileId",
                table: "SegmentMembers",
                newName: "IX_SegmentMembers_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_OrganizationId",
                table: "SegmentMembers",
                newName: "IX_SegmentMembers_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectAreas_OrganizationId",
                table: "SegmentAreas",
                newName: "IX_SegmentAreas_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectAreas_Name",
                table: "SegmentAreas",
                newName: "IX_SegmentAreas_Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Segments_Id",
                table: "Segments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Segments",
                table: "Segments",
                columns: new[] { "Id", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SegmentReportsWeekly_DateId_SegmentId_TaskCategoryId",
                table: "SegmentReportsWeekly",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SegmentReportsDaily_DateId_SegmentId_TaskCategoryId",
                table: "SegmentReportsDaily",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SegmentMembers_SegmentId_ProfileId",
                table: "SegmentMembers",
                columns: new[] { "SegmentId", "ProfileId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentMembers",
                table: "SegmentMembers",
                columns: new[] { "SegmentId", "ProfileId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SegmentAreas_Id",
                table: "SegmentAreas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentAreas",
                table: "SegmentAreas",
                columns: new[] { "Id", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPointSegments_Segments_SegmentId",
                table: "ActionPointSegments",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPointSettings_Segments_SegmentId",
                table: "ActionPointSettings",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_Segments_SegmentId",
                table: "Challenges",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Segments_SegmentId",
                table: "Competitions",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Integrations_Segments_SegmentId",
                table: "Integrations",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Segments_SegmentId",
                table: "Invitations",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileExternalIds_Segments_SegmentId",
                table: "ProfileExternalIds",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeams_Segments_SegmentId",
                table: "ProjectTeams",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentAreas_Organizations_OrganizationId",
                table: "SegmentAreas",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentMembers_Organizations_OrganizationId",
                table: "SegmentMembers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentMembers_Profiles_ProfileId",
                table: "SegmentMembers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentMembers_Segments_SegmentId",
                table: "SegmentMembers",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                table: "SegmentReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsDaily_Segments_SegmentId",
                table: "SegmentReportsDaily",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsDaily_TaskCategories_TaskCategoryId",
                table: "SegmentReportsDaily",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                table: "SegmentReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsWeekly_Segments_SegmentId",
                table: "SegmentReportsWeekly",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsWeekly_TaskCategories_TaskCategoryId",
                table: "SegmentReportsWeekly",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Segments_Organizations_OrganizationId",
                table: "Segments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemSegments_Segments_SegmentId",
                table: "ShopItemSegments",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopPurchases_Segments_SegmentId",
                table: "ShopPurchases",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_Segments_SegmentId",
                table: "TaskLogs",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_SegmentAreas_SegmentAreaId",
                table: "Tasks",
                column: "SegmentAreaId",
                principalTable: "SegmentAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Segments_SegmentId",
                table: "Tasks",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Segments_SegmentId",
                table: "Teams",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionPointSegments_Segments_SegmentId",
                table: "ActionPointSegments");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionPointSettings_Segments_SegmentId",
                table: "ActionPointSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Segments_SegmentId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Segments_SegmentId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Integrations_Segments_SegmentId",
                table: "Integrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Segments_SegmentId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileExternalIds_Segments_SegmentId",
                table: "ProfileExternalIds");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeams_Segments_SegmentId",
                table: "ProjectTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentAreas_Organizations_OrganizationId",
                table: "SegmentAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentMembers_Organizations_OrganizationId",
                table: "SegmentMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentMembers_Profiles_ProfileId",
                table: "SegmentMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentMembers_Segments_SegmentId",
                table: "SegmentMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsDaily_Segments_SegmentId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsDaily_TaskCategories_TaskCategoryId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsWeekly_Segments_SegmentId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsWeekly_TaskCategories_TaskCategoryId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_Segments_Organizations_OrganizationId",
                table: "Segments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemSegments_Segments_SegmentId",
                table: "ShopItemSegments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopPurchases_Segments_SegmentId",
                table: "ShopPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_Segments_SegmentId",
                table: "TaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_SegmentAreas_SegmentAreaId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Segments_SegmentId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Segments_SegmentId",
                table: "Teams");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Segments_Id",
                table: "Segments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Segments",
                table: "Segments");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SegmentReportsWeekly_DateId_SegmentId_TaskCategoryId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SegmentReportsDaily_DateId_SegmentId_TaskCategoryId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SegmentMembers_SegmentId_ProfileId",
                table: "SegmentMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentMembers",
                table: "SegmentMembers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SegmentAreas_Id",
                table: "SegmentAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentAreas",
                table: "SegmentAreas");

            migrationBuilder.RenameTable(
                name: "Segments",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "SegmentReportsWeekly",
                newName: "ProjectReportsWeekly");

            migrationBuilder.RenameTable(
                name: "SegmentReportsDaily",
                newName: "ProjectReportsDaily");

            migrationBuilder.RenameTable(
                name: "SegmentMembers",
                newName: "ProjectMembers");

            migrationBuilder.RenameTable(
                name: "SegmentAreas",
                newName: "ProjectAreas");

            migrationBuilder.RenameIndex(
                name: "IX_Segments_OrganizationId",
                table: "Projects",
                newName: "IX_Projects_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Segments_Key",
                table: "Projects",
                newName: "IX_Projects_Key");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentReportsWeekly_TaskCategoryId",
                table: "ProjectReportsWeekly",
                newName: "IX_ProjectReportsWeekly_TaskCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentReportsWeekly_SegmentId",
                table: "ProjectReportsWeekly",
                newName: "IX_ProjectReportsWeekly_SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentReportsWeekly_OrganizationId",
                table: "ProjectReportsWeekly",
                newName: "IX_ProjectReportsWeekly_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentReportsDaily_TaskCategoryId",
                table: "ProjectReportsDaily",
                newName: "IX_ProjectReportsDaily_TaskCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentReportsDaily_SegmentId",
                table: "ProjectReportsDaily",
                newName: "IX_ProjectReportsDaily_SegmentId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentReportsDaily_OrganizationId",
                table: "ProjectReportsDaily",
                newName: "IX_ProjectReportsDaily_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentMembers_ProfileId",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentMembers_OrganizationId",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentAreas_OrganizationId",
                table: "ProjectAreas",
                newName: "IX_ProjectAreas_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_SegmentAreas_Name",
                table: "ProjectAreas",
                newName: "IX_ProjectAreas_Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Projects_Id",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                columns: new[] { "Id", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectReportsWeekly_DateId_SegmentId_TaskCategoryId",
                table: "ProjectReportsWeekly",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectReportsWeekly",
                table: "ProjectReportsWeekly",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectReportsDaily_DateId_SegmentId_TaskCategoryId",
                table: "ProjectReportsDaily",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectReportsDaily",
                table: "ProjectReportsDaily",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectMembers_SegmentId_ProfileId",
                table: "ProjectMembers",
                columns: new[] { "SegmentId", "ProfileId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers",
                columns: new[] { "SegmentId", "ProfileId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectAreas_Id",
                table: "ProjectAreas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectAreas",
                table: "ProjectAreas",
                columns: new[] { "Id", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPointSegments_Projects_SegmentId",
                table: "ActionPointSegments",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_ProjectAreas_Organizations_OrganizationId",
                table: "ProjectAreas",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Organizations_OrganizationId",
                table: "ProjectMembers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Profiles_ProfileId",
                table: "ProjectMembers",
                column: "ProfileId",
                principalTable: "Profiles",
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
                name: "FK_ProjectReportsDaily_Organizations_OrganizationId",
                table: "ProjectReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReportsDaily_Projects_SegmentId",
                table: "ProjectReportsDaily",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReportsDaily_TaskCategories_TaskCategoryId",
                table: "ProjectReportsDaily",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReportsWeekly_Organizations_OrganizationId",
                table: "ProjectReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReportsWeekly_Projects_SegmentId",
                table: "ProjectReportsWeekly",
                column: "SegmentId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReportsWeekly_TaskCategories_TaskCategoryId",
                table: "ProjectReportsWeekly",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Organizations_OrganizationId",
                table: "Projects",
                column: "OrganizationId",
                principalTable: "Organizations",
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
                name: "FK_ShopItemSegments_Projects_SegmentId",
                table: "ShopItemSegments",
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
    }
}
