using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_TokenTransactions_ProfileId",
            //    table: "TokenTransactions");

            //migrationBuilder.DropIndex(
            //    name: "IX_TokenTransactions_TokenId",
            //    table: "TokenTransactions");

            //migrationBuilder.DropIndex(
            //    name: "IX_Teams_SegmentId_Key_ArchievedAt",
            //    table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamReportsWeekly",
                table: "TeamReportsWeekly");

            //migrationBuilder.DropIndex(
            //    name: "IX_TeamReportsWeekly_TaskCategoryId",
            //    table: "TeamReportsWeekly");

            //migrationBuilder.DropIndex(
            //    name: "IX_TeamReportsWeekly_TeamId",
            //    table: "TeamReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamReportsDaily",
                table: "TeamReportsDaily");

            //migrationBuilder.DropIndex(
            //    name: "IX_TeamReportsDaily_TaskCategoryId",
            //    table: "TeamReportsDaily");

            //migrationBuilder.DropIndex(
            //    name: "IX_TeamReportsDaily_TeamId",
            //    table: "TeamReportsDaily");

            //migrationBuilder.DropIndex(
            //    name: "IX_TaskSyncs_SegmentId",
            //    table: "TaskSyncs");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tasks_AssigneeProfileId",
            //    table: "Tasks");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tasks_SegmentAreaId",
            //    table: "Tasks");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tasks_SegmentId",
            //    table: "Tasks");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tasks_TaskCategoryId",
            //    table: "Tasks");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tasks_TeamId",
            //    table: "Tasks");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tasks_ExternalId_IntegrationType",
            //    table: "Tasks");

            //migrationBuilder.DropIndex(
            //    name: "IX_TaskLogs_AssigneeProfileId",
            //    table: "TaskLogs");

            //migrationBuilder.DropIndex(
            //    name: "IX_TaskLogs_SegmentId",
            //    table: "TaskLogs");

            //migrationBuilder.DropIndex(
            //    name: "IX_TaskLogs_TeamId",
            //    table: "TaskLogs");

            //migrationBuilder.DropIndex(
            //    name: "IX_TaskCategories_Name",
            //    table: "TaskCategories");

            //migrationBuilder.DropIndex(
            //    name: "IX_ShopPurchases_ItemId",
            //    table: "ShopPurchases");

            //migrationBuilder.DropIndex(
            //    name: "IX_ShopPurchases_SegmentId",
            //    table: "ShopPurchases");

            //migrationBuilder.DropIndex(
            //    name: "IX_ShopPurchases_ProfileId_Status",
            //    table: "ShopPurchases");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ShopLogs_ShopId_LogId",
            //    table: "ShopLogs");

            //migrationBuilder.DropIndex(
            //    name: "IX_ShopLogs_LogId",
            //    table: "ShopLogs");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ShopItemSegments_ShopItemId_SegmentId",
            //    table: "ShopItemSegments");

            //migrationBuilder.DropIndex(
            //    name: "IX_ShopItemSegments_SegmentId",
            //    table: "ShopItemSegments");

            //migrationBuilder.DropIndex(
            //    name: "IX_ShopItems_ItemId",
            //    table: "ShopItems");

            //migrationBuilder.DropIndex(
            //    name: "IX_Segments_Key_ArchievedAt",
            //    table: "Segments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly");

            //migrationBuilder.DropIndex(
            //    name: "IX_SegmentReportsWeekly_SegmentId",
            //    table: "SegmentReportsWeekly");

            //migrationBuilder.DropIndex(
            //    name: "IX_SegmentReportsWeekly_TaskCategoryId",
            //    table: "SegmentReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily");

            //migrationBuilder.DropIndex(
            //    name: "IX_SegmentReportsDaily_SegmentId",
            //    table: "SegmentReportsDaily");

            //migrationBuilder.DropIndex(
            //    name: "IX_SegmentReportsDaily_TaskCategoryId",
            //    table: "SegmentReportsDaily");

            //migrationBuilder.DropIndex(
            //    name: "IX_SegmentAreas_Name",
            //    table: "SegmentAreas");

            //migrationBuilder.DropIndex(
            //    name: "IX_Profiles_IdentityId",
            //    table: "Profiles");

            //migrationBuilder.DropIndex(
            //    name: "IX_Profiles_Username",
            //    table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileReportsWeekly_ProfileId",
            //    table: "ProfileReportsWeekly");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileReportsWeekly_TaskCategoryId",
            //    table: "ProfileReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileReportsDaily_ProfileId",
            //    table: "ProfileReportsDaily");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileReportsDaily_TaskCategoryId",
            //    table: "ProfileReportsDaily");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ProfileOneUps_DateId_UppedProfileId_CreatedBy",
            //    table: "ProfileOneUps");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileOneUps_UppedProfileId",
            //    table: "ProfileOneUps");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ProfileLogs_ProfileId_LogId",
            //    table: "ProfileLogs");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileLogs_LogId",
            //    table: "ProfileLogs");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileLogs_ProfileId_Event",
            //    table: "ProfileLogs");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileInventoryItems_ProfileId_IsActive",
            //    table: "ProfileInventoryItems");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileInventoryItems_ItemId_ProfileId_IsActive",
            //    table: "ProfileInventoryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileExternalIds_ProfileId",
            //    table: "ProfileExternalIds");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileExternalIds_SegmentId",
            //    table: "ProfileExternalIds");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileAssignments_ProfileId",
            //    table: "ProfileAssignments");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileAssignments_SegmentId_ProfileId",
            //    table: "ProfileAssignments");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileAssignments_TeamId_ProfileId",
            //    table: "ProfileAssignments");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId",
            //    table: "ProfileAssignments");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_LogSettings_LogDeviceId_LogEvent",
            //    table: "LogSettings");

            //migrationBuilder.DropIndex(
            //    name: "IX_LogSettings_ProfileId",
            //    table: "LogSettings");

            //migrationBuilder.DropIndex(
            //    name: "IX_LogDevices_ProfileId",
            //    table: "LogDevices");

            //migrationBuilder.DropIndex(
            //    name: "IX_ItemReservations_ItemId",
            //    table: "ItemReservations");

            //migrationBuilder.DropIndex(
            //    name: "IX_ItemGifts_ItemId",
            //    table: "ItemGifts");

            //migrationBuilder.DropIndex(
            //    name: "IX_ItemGifts_ReceiverId",
            //    table: "ItemGifts");

            //migrationBuilder.DropIndex(
            //    name: "IX_ItemGifts_SenderId",
            //    table: "ItemGifts");

            //migrationBuilder.DropIndex(
            //    name: "IX_ItemDisenchants_ItemId",
            //    table: "ItemDisenchants");

            //migrationBuilder.DropIndex(
            //    name: "IX_ItemDisenchants_ProfileId",
            //    table: "ItemDisenchants");

            //migrationBuilder.DropIndex(
            //    name: "IX_Invitations_SegmentId",
            //    table: "Invitations");

            //migrationBuilder.DropIndex(
            //    name: "IX_Invitations_TeamId",
            //    table: "Invitations");

            //migrationBuilder.DropIndex(
            //    name: "IX_Integrations_SegmentId",
            //    table: "Integrations");

            //migrationBuilder.DropIndex(
            //    name: "IX_Integrations_ProfileId_SegmentId",
            //    table: "Integrations");

            //migrationBuilder.DropIndex(
            //    name: "IX_IntegrationFields_IntegrationId",
            //    table: "IntegrationFields");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitorScores_CompetitionId",
            //    table: "CompetitorScores");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitorScores_ProfileId",
            //    table: "CompetitorScores");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitorScores_TeamId",
            //    table: "CompetitorScores");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitorScores_CompetitorId_ProfileId",
            //    table: "CompetitorScores");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitorScores_CompetitorId_TeamId",
            //    table: "CompetitorScores");

            //migrationBuilder.DropIndex(
            //    name: "IX_Competitors_ProfileId",
            //    table: "Competitors");

            //migrationBuilder.DropIndex(
            //    name: "IX_Competitors_TeamId",
            //    table: "Competitors");

            //migrationBuilder.DropIndex(
            //    name: "IX_Competitors_CompetitionId_ProfileId",
            //    table: "Competitors");

            //migrationBuilder.DropIndex(
            //    name: "IX_Competitors_CompetitionId_TeamId",
            //    table: "Competitors");

            //migrationBuilder.DropIndex(
            //    name: "IX_Competitors_CompetitionId_ProfileId_TeamId",
            //    table: "Competitors");

            //migrationBuilder.DropIndex(
            //    name: "IX_Competitions_PreviousCompetitionId",
            //    table: "Competitions");

            //migrationBuilder.DropIndex(
            //    name: "IX_Competitions_SegmentId",
            //    table: "Competitions");

            //migrationBuilder.DropIndex(
            //    name: "IX_Competitions_TokenId",
            //    table: "Competitions");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitionRewards_CompetitionId",
            //    table: "CompetitionRewards");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitionRewards_ItemId",
            //    table: "CompetitionRewards");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitionRewards_TokenId",
            //    table: "CompetitionRewards");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_CompetitionLogs_CompetitionId_LogId",
            //    table: "CompetitionLogs");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitionLogs_LogId",
            //    table: "CompetitionLogs");

            //migrationBuilder.DropIndex(
            //    name: "IX_CompetitionLogs_CompetitionId_Event",
            //    table: "CompetitionLogs");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ClaimBundleTokenTxns_ClaimBundleId_TokenTransactionId",
            //    table: "ClaimBundleTokenTxns");

            //migrationBuilder.DropIndex(
            //    name: "IX_ClaimBundleTokenTxns_TokenTransactionId",
            //    table: "ClaimBundleTokenTxns");

            //migrationBuilder.DropIndex(
            //    name: "IX_ClaimBundles_ProfileId",
            //    table: "ClaimBundles");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ClaimBundleItems_ClaimBundleId_ProfileInventoryItemId",
            //    table: "ClaimBundleItems");

            //migrationBuilder.DropIndex(
            //    name: "IX_ClaimBundleItems_ProfileInventoryItemId",
            //    table: "ClaimBundleItems");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ChallengeSegments_ChallengeId_SegmentId",
            //    table: "ChallengeSegments");

            //migrationBuilder.DropIndex(
            //    name: "IX_ChallengeSegments_SegmentId",
            //    table: "ChallengeSegments");

            //migrationBuilder.DropIndex(
            //    name: "IX_Challenges_SegmentId",
            //    table: "Challenges");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ChallengeRewards_ChallengeId_ItemId",
            //    table: "ChallengeRewards");

            //migrationBuilder.DropIndex(
            //    name: "IX_ChallengeRewards_ItemId",
            //    table: "ChallengeRewards");

            //migrationBuilder.DropIndex(
            //    name: "IX_ChallengeGoals_ChallengeId",
            //    table: "ChallengeGoals");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ChallengeGoalCompletions_GoalId_ProfileId",
            //    table: "ChallengeGoalCompletions");

            //migrationBuilder.DropIndex(
            //    name: "IX_ChallengeGoalCompletions_ProfileId",
            //    table: "ChallengeGoalCompletions");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ChallengeCompletions_ChallengeId_ProfileId",
            //    table: "ChallengeCompletions");

            //migrationBuilder.DropIndex(
            //    name: "IX_ChallengeCompletions_ProfileId",
            //    table: "ChallengeCompletions");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ChallengeCommits_ChallengeId_ProfileId",
            //    table: "ChallengeCommits");

            //migrationBuilder.DropIndex(
            //    name: "IX_ChallengeCommits_ProfileId",
            //    table: "ChallengeCommits");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ActionPointSettings_Type",
            //    table: "ActionPointSettings");

            //migrationBuilder.DropIndex(
            //    name: "IX_ActionPointSettings_SegmentId",
            //    table: "ActionPointSettings");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ActionPointSegments_ActionPointId_SegmentId",
            //    table: "ActionPointSegments");

            //migrationBuilder.DropIndex(
            //    name: "IX_ActionPointSegments_SegmentId",
            //    table: "ActionPointSegments");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_ActionPointProfiles_ActionPointId_ProfileId",
            //    table: "ActionPointProfiles");

            //migrationBuilder.DropIndex(
            //    name: "IX_ActionPointProfiles_ProfileId",
            //    table: "ActionPointProfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamReportsWeekly",
                table: "TeamReportsWeekly",
                columns: new[] { "DateId", "TeamId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamReportsDaily",
                table: "TeamReportsDaily",
                columns: new[] { "DateId", "TeamId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly",
                columns: new[] { "DateId", "ProfileId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily",
                columns: new[] { "DateId", "ProfileId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds",
                columns: new[] { "ExternalId", "IntegrationType", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TokenTransactions_ProfileId_OrganizationId",
            //    table: "TokenTransactions",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TokenTransactions_TokenId_OrganizationId",
            //    table: "TokenTransactions",
            //    columns: new[] { "TokenId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Teams_SegmentId_Key_ArchievedAt_OrganizationId",
            //    table: "Teams",
            //    columns: new[] { "SegmentId", "Key", "ArchievedAt", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamReportsWeekly_OrganizationId",
            //    table: "TeamReportsWeekly",
            //    column: "OrganizationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamReportsWeekly_TaskCategoryId_OrganizationId",
            //    table: "TeamReportsWeekly",
            //    columns: new[] { "TaskCategoryId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamReportsWeekly_TeamId_OrganizationId",
            //    table: "TeamReportsWeekly",
            //    columns: new[] { "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamReportsDaily_OrganizationId",
            //    table: "TeamReportsDaily",
            //    column: "OrganizationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamReportsDaily_TaskCategoryId_OrganizationId",
            //    table: "TeamReportsDaily",
            //    columns: new[] { "TaskCategoryId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamReportsDaily_TeamId_OrganizationId",
            //    table: "TeamReportsDaily",
            //    columns: new[] { "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TaskSyncs_SegmentId_OrganizationId",
            //    table: "TaskSyncs",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tasks_AssigneeProfileId_OrganizationId",
            //    table: "Tasks",
            //    columns: new[] { "AssigneeProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tasks_SegmentAreaId_OrganizationId",
            //    table: "Tasks",
            //    columns: new[] { "SegmentAreaId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tasks_SegmentId_OrganizationId",
            //    table: "Tasks",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tasks_TaskCategoryId_OrganizationId",
            //    table: "Tasks",
            //    columns: new[] { "TaskCategoryId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tasks_TeamId_OrganizationId",
            //    table: "Tasks",
            //    columns: new[] { "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tasks_ExternalId_IntegrationType_OrganizationId",
            //    table: "Tasks",
            //    columns: new[] { "ExternalId", "IntegrationType", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tasks_ExternalId_IntegrationType_SegmentId_OrganizationId",
            //    table: "Tasks",
            //    columns: new[] { "ExternalId", "IntegrationType", "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TaskLogs_AssigneeProfileId_OrganizationId",
            //    table: "TaskLogs",
            //    columns: new[] { "AssigneeProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TaskLogs_SegmentId_OrganizationId",
            //    table: "TaskLogs",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TaskLogs_TeamId_OrganizationId",
            //    table: "TaskLogs",
            //    columns: new[] { "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TaskCategories_Name_OrganizationId",
            //    table: "TaskCategories",
            //    columns: new[] { "Name", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShopPurchases_ItemId_OrganizationId",
            //    table: "ShopPurchases",
            //    columns: new[] { "ItemId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShopPurchases_SegmentId_OrganizationId",
            //    table: "ShopPurchases",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShopPurchases_ProfileId_Status_OrganizationId",
            //    table: "ShopPurchases",
            //    columns: new[] { "ProfileId", "Status", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShopLogs_LogId_OrganizationId",
            //    table: "ShopLogs",
            //    columns: new[] { "LogId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShopItemSegments_SegmentId_OrganizationId",
            //    table: "ShopItemSegments",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShopItems_ItemId_OrganizationId",
            //    table: "ShopItems",
            //    columns: new[] { "ItemId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Segments_Key_ArchievedAt_OrganizationId",
            //    table: "Segments",
            //    columns: new[] { "Key", "ArchievedAt", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_SegmentReportsWeekly_OrganizationId",
            //    table: "SegmentReportsWeekly",
            //    column: "OrganizationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SegmentReportsWeekly_SegmentId_OrganizationId",
            //    table: "SegmentReportsWeekly",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_SegmentReportsWeekly_TaskCategoryId_OrganizationId",
            //    table: "SegmentReportsWeekly",
            //    columns: new[] { "TaskCategoryId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_SegmentReportsDaily_OrganizationId",
            //    table: "SegmentReportsDaily",
            //    column: "OrganizationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SegmentReportsDaily_SegmentId_OrganizationId",
            //    table: "SegmentReportsDaily",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_SegmentReportsDaily_TaskCategoryId_OrganizationId",
            //    table: "SegmentReportsDaily",
            //    columns: new[] { "TaskCategoryId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_SegmentAreas_Name_OrganizationId",
            //    table: "SegmentAreas",
            //    columns: new[] { "Name", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Profiles_IdentityId_OrganizationId",
            //    table: "Profiles",
            //    columns: new[] { "IdentityId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Profiles_Username_OrganizationId",
            //    table: "Profiles",
            //    columns: new[] { "Username", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileReportsWeekly_OrganizationId",
            //    table: "ProfileReportsWeekly",
            //    column: "OrganizationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileReportsWeekly_ProfileId_OrganizationId",
            //    table: "ProfileReportsWeekly",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileReportsWeekly_TaskCategoryId_OrganizationId",
            //    table: "ProfileReportsWeekly",
            //    columns: new[] { "TaskCategoryId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileReportsDaily_OrganizationId",
            //    table: "ProfileReportsDaily",
            //    column: "OrganizationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileReportsDaily_ProfileId_OrganizationId",
            //    table: "ProfileReportsDaily",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileReportsDaily_TaskCategoryId_OrganizationId",
            //    table: "ProfileReportsDaily",
            //    columns: new[] { "TaskCategoryId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileOneUps_UppedProfileId_OrganizationId",
            //    table: "ProfileOneUps",
            //    columns: new[] { "UppedProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileLogs_LogId_OrganizationId",
            //    table: "ProfileLogs",
            //    columns: new[] { "LogId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileLogs_ProfileId_Event_OrganizationId",
            //    table: "ProfileLogs",
            //    columns: new[] { "ProfileId", "Event", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileInventoryItems_ProfileId_IsActive_OrganizationId",
            //    table: "ProfileInventoryItems",
            //    columns: new[] { "ProfileId", "IsActive", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileInventoryItems_ItemId_ProfileId_IsActive_OrganizationId",
            //    table: "ProfileInventoryItems",
            //    columns: new[] { "ItemId", "ProfileId", "IsActive", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileExternalIds_OrganizationId",
            //    table: "ProfileExternalIds",
            //    column: "OrganizationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileExternalIds_ProfileId_OrganizationId",
            //    table: "ProfileExternalIds",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileExternalIds_SegmentId_OrganizationId",
            //    table: "ProfileExternalIds",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileAssignments_ProfileId_OrganizationId",
            //    table: "ProfileAssignments",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileAssignments_SegmentId_ProfileId_OrganizationId",
            //    table: "ProfileAssignments",
            //    columns: new[] { "SegmentId", "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileAssignments_TeamId_ProfileId_OrganizationId",
            //    table: "ProfileAssignments",
            //    columns: new[] { "TeamId", "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_OrganizationId",
            //    table: "ProfileAssignments",
            //    columns: new[] { "SegmentId", "TeamId", "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_LogSettings_ProfileId_OrganizationId",
            //    table: "LogSettings",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_LogDevices_ProfileId_OrganizationId",
            //    table: "LogDevices",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItemReservations_ItemId_OrganizationId",
            //    table: "ItemReservations",
            //    columns: new[] { "ItemId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItemGifts_ItemId_OrganizationId",
            //    table: "ItemGifts",
            //    columns: new[] { "ItemId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItemGifts_ReceiverId_OrganizationId",
            //    table: "ItemGifts",
            //    columns: new[] { "ReceiverId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItemGifts_SenderId_OrganizationId",
            //    table: "ItemGifts",
            //    columns: new[] { "SenderId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItemDisenchants_ItemId_OrganizationId",
            //    table: "ItemDisenchants",
            //    columns: new[] { "ItemId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItemDisenchants_ProfileId_OrganizationId",
            //    table: "ItemDisenchants",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Invitations_SegmentId_OrganizationId",
            //    table: "Invitations",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Invitations_TeamId_OrganizationId",
            //    table: "Invitations",
            //    columns: new[] { "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Integrations_SegmentId_OrganizationId",
            //    table: "Integrations",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Integrations_ProfileId_SegmentId_OrganizationId",
            //    table: "Integrations",
            //    columns: new[] { "ProfileId", "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_IntegrationFields_IntegrationId_OrganizationId",
            //    table: "IntegrationFields",
            //    columns: new[] { "IntegrationId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitorScores_CompetitionId_OrganizationId",
            //    table: "CompetitorScores",
            //    columns: new[] { "CompetitionId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitorScores_ProfileId_OrganizationId",
            //    table: "CompetitorScores",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitorScores_TeamId_OrganizationId",
            //    table: "CompetitorScores",
            //    columns: new[] { "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitorScores_CompetitorId_ProfileId_OrganizationId",
            //    table: "CompetitorScores",
            //    columns: new[] { "CompetitorId", "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitorScores_CompetitorId_TeamId_OrganizationId",
            //    table: "CompetitorScores",
            //    columns: new[] { "CompetitorId", "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Competitors_ProfileId_OrganizationId",
            //    table: "Competitors",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Competitors_TeamId_OrganizationId",
            //    table: "Competitors",
            //    columns: new[] { "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Competitors_CompetitionId_ProfileId_OrganizationId",
            //    table: "Competitors",
            //    columns: new[] { "CompetitionId", "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Competitors_CompetitionId_TeamId_OrganizationId",
            //    table: "Competitors",
            //    columns: new[] { "CompetitionId", "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Competitors_CompetitionId_ProfileId_TeamId_OrganizationId",
            //    table: "Competitors",
            //    columns: new[] { "CompetitionId", "ProfileId", "TeamId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Competitions_PreviousCompetitionId_OrganizationId",
            //    table: "Competitions",
            //    columns: new[] { "PreviousCompetitionId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Competitions_SegmentId_OrganizationId",
            //    table: "Competitions",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Competitions_TokenId_OrganizationId",
            //    table: "Competitions",
            //    columns: new[] { "TokenId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitionRewards_CompetitionId_OrganizationId",
            //    table: "CompetitionRewards",
            //    columns: new[] { "CompetitionId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitionRewards_ItemId_OrganizationId",
            //    table: "CompetitionRewards",
            //    columns: new[] { "ItemId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitionRewards_TokenId_OrganizationId",
            //    table: "CompetitionRewards",
            //    columns: new[] { "TokenId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitionLogs_LogId_OrganizationId",
            //    table: "CompetitionLogs",
            //    columns: new[] { "LogId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CompetitionLogs_CompetitionId_Event_OrganizationId",
            //    table: "CompetitionLogs",
            //    columns: new[] { "CompetitionId", "Event", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClaimBundleTokenTxns_TokenTransactionId_OrganizationId",
            //    table: "ClaimBundleTokenTxns",
            //    columns: new[] { "TokenTransactionId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClaimBundles_ProfileId_OrganizationId",
            //    table: "ClaimBundles",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClaimBundleItems_ProfileInventoryItemId_OrganizationId",
            //    table: "ClaimBundleItems",
            //    columns: new[] { "ProfileInventoryItemId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChallengeSegments_SegmentId_OrganizationId",
            //    table: "ChallengeSegments",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Challenges_SegmentId_OrganizationId",
            //    table: "Challenges",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChallengeRewards_ItemId_OrganizationId",
            //    table: "ChallengeRewards",
            //    columns: new[] { "ItemId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChallengeGoals_ChallengeId_OrganizationId",
            //    table: "ChallengeGoals",
            //    columns: new[] { "ChallengeId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChallengeGoalCompletions_ProfileId_OrganizationId",
            //    table: "ChallengeGoalCompletions",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChallengeCompletions_ProfileId_OrganizationId",
            //    table: "ChallengeCompletions",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ChallengeCommits_ProfileId_OrganizationId",
            //    table: "ChallengeCommits",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ActionPointSettings_SegmentId_OrganizationId",
            //    table: "ActionPointSettings",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ActionPointSegments_SegmentId_OrganizationId",
            //    table: "ActionPointSegments",
            //    columns: new[] { "SegmentId", "OrganizationId" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ActionPointProfiles_ProfileId_OrganizationId",
            //    table: "ActionPointProfiles",
            //    columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                table: "ProfileExternalIds",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsDaily_Organizations_OrganizationId",
                table: "ProfileReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsWeekly_Organizations_OrganizationId",
                table: "ProfileReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                table: "SegmentReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                table: "SegmentReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamReportsDaily_Organizations_OrganizationId",
                table: "TeamReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamReportsWeekly_Organizations_OrganizationId",
                table: "TeamReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                table: "ProfileExternalIds");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsDaily_Organizations_OrganizationId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsWeekly_Organizations_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsDaily_Organizations_OrganizationId",
                table: "TeamReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsWeekly_Organizations_OrganizationId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_TokenTransactions_ProfileId_OrganizationId",
                table: "TokenTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TokenTransactions_TokenId_OrganizationId",
                table: "TokenTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchievedAt_OrganizationId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamReportsWeekly",
                table: "TeamReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsWeekly_OrganizationId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsWeekly_TaskCategoryId_OrganizationId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsWeekly_TeamId_OrganizationId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamReportsDaily",
                table: "TeamReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsDaily_OrganizationId",
                table: "TeamReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsDaily_TaskCategoryId_OrganizationId",
                table: "TeamReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsDaily_TeamId_OrganizationId",
                table: "TeamReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_TaskSyncs_SegmentId_OrganizationId",
                table: "TaskSyncs");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssigneeProfileId_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SegmentAreaId_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SegmentId_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskCategoryId_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TeamId_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_SegmentId_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskLogs_AssigneeProfileId_OrganizationId",
                table: "TaskLogs");

            migrationBuilder.DropIndex(
                name: "IX_TaskLogs_SegmentId_OrganizationId",
                table: "TaskLogs");

            migrationBuilder.DropIndex(
                name: "IX_TaskLogs_TeamId_OrganizationId",
                table: "TaskLogs");

            migrationBuilder.DropIndex(
                name: "IX_TaskCategories_Name_OrganizationId",
                table: "TaskCategories");

            migrationBuilder.DropIndex(
                name: "IX_ShopPurchases_ItemId_OrganizationId",
                table: "ShopPurchases");

            migrationBuilder.DropIndex(
                name: "IX_ShopPurchases_SegmentId_OrganizationId",
                table: "ShopPurchases");

            migrationBuilder.DropIndex(
                name: "IX_ShopPurchases_ProfileId_Status_OrganizationId",
                table: "ShopPurchases");

            migrationBuilder.DropIndex(
                name: "IX_ShopLogs_LogId_OrganizationId",
                table: "ShopLogs");

            migrationBuilder.DropIndex(
                name: "IX_ShopItemSegments_SegmentId_OrganizationId",
                table: "ShopItemSegments");

            migrationBuilder.DropIndex(
                name: "IX_ShopItems_ItemId_OrganizationId",
                table: "ShopItems");

            migrationBuilder.DropIndex(
                name: "IX_Segments_Key_ArchievedAt_OrganizationId",
                table: "Segments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsWeekly_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsWeekly_SegmentId_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsWeekly_TaskCategoryId_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsDaily_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsDaily_SegmentId_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsDaily_TaskCategoryId_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_SegmentAreas_Name_OrganizationId",
                table: "SegmentAreas");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_IdentityId_OrganizationId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_Username_OrganizationId",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsWeekly_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsWeekly_ProfileId_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsWeekly_TaskCategoryId_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsDaily_OrganizationId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsDaily_ProfileId_OrganizationId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsDaily_TaskCategoryId_OrganizationId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_ProfileOneUps_UppedProfileId_OrganizationId",
                table: "ProfileOneUps");

            migrationBuilder.DropIndex(
                name: "IX_ProfileLogs_LogId_OrganizationId",
                table: "ProfileLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProfileLogs_ProfileId_Event_OrganizationId",
                table: "ProfileLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProfileInventoryItems_ProfileId_IsActive_OrganizationId",
                table: "ProfileInventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_ProfileInventoryItems_ItemId_ProfileId_IsActive_OrganizationId",
                table: "ProfileInventoryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds");

            migrationBuilder.DropIndex(
                name: "IX_ProfileExternalIds_OrganizationId",
                table: "ProfileExternalIds");

            migrationBuilder.DropIndex(
                name: "IX_ProfileExternalIds_ProfileId_OrganizationId",
                table: "ProfileExternalIds");

            migrationBuilder.DropIndex(
                name: "IX_ProfileExternalIds_SegmentId_OrganizationId",
                table: "ProfileExternalIds");

            migrationBuilder.DropIndex(
                name: "IX_ProfileAssignments_ProfileId_OrganizationId",
                table: "ProfileAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ProfileAssignments_SegmentId_ProfileId_OrganizationId",
                table: "ProfileAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ProfileAssignments_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments");

            migrationBuilder.DropIndex(
                name: "IX_LogSettings_ProfileId_OrganizationId",
                table: "LogSettings");

            migrationBuilder.DropIndex(
                name: "IX_LogDevices_ProfileId_OrganizationId",
                table: "LogDevices");

            migrationBuilder.DropIndex(
                name: "IX_ItemReservations_ItemId_OrganizationId",
                table: "ItemReservations");

            migrationBuilder.DropIndex(
                name: "IX_ItemGifts_ItemId_OrganizationId",
                table: "ItemGifts");

            migrationBuilder.DropIndex(
                name: "IX_ItemGifts_ReceiverId_OrganizationId",
                table: "ItemGifts");

            migrationBuilder.DropIndex(
                name: "IX_ItemGifts_SenderId_OrganizationId",
                table: "ItemGifts");

            migrationBuilder.DropIndex(
                name: "IX_ItemDisenchants_ItemId_OrganizationId",
                table: "ItemDisenchants");

            migrationBuilder.DropIndex(
                name: "IX_ItemDisenchants_ProfileId_OrganizationId",
                table: "ItemDisenchants");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_SegmentId_OrganizationId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_TeamId_OrganizationId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Integrations_SegmentId_OrganizationId",
                table: "Integrations");

            migrationBuilder.DropIndex(
                name: "IX_Integrations_ProfileId_SegmentId_OrganizationId",
                table: "Integrations");

            migrationBuilder.DropIndex(
                name: "IX_IntegrationFields_IntegrationId_OrganizationId",
                table: "IntegrationFields");

            migrationBuilder.DropIndex(
                name: "IX_CompetitorScores_CompetitionId_OrganizationId",
                table: "CompetitorScores");

            migrationBuilder.DropIndex(
                name: "IX_CompetitorScores_ProfileId_OrganizationId",
                table: "CompetitorScores");

            migrationBuilder.DropIndex(
                name: "IX_CompetitorScores_TeamId_OrganizationId",
                table: "CompetitorScores");

            migrationBuilder.DropIndex(
                name: "IX_CompetitorScores_CompetitorId_ProfileId_OrganizationId",
                table: "CompetitorScores");

            migrationBuilder.DropIndex(
                name: "IX_CompetitorScores_CompetitorId_TeamId_OrganizationId",
                table: "CompetitorScores");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_ProfileId_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_TeamId_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_CompetitionId_TeamId_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_TeamId_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_PreviousCompetitionId_OrganizationId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_SegmentId_OrganizationId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_TokenId_OrganizationId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionRewards_CompetitionId_OrganizationId",
                table: "CompetitionRewards");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionRewards_ItemId_OrganizationId",
                table: "CompetitionRewards");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionRewards_TokenId_OrganizationId",
                table: "CompetitionRewards");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionLogs_LogId_OrganizationId",
                table: "CompetitionLogs");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionLogs_CompetitionId_Event_OrganizationId",
                table: "CompetitionLogs");

            migrationBuilder.DropIndex(
                name: "IX_ClaimBundleTokenTxns_TokenTransactionId_OrganizationId",
                table: "ClaimBundleTokenTxns");

            migrationBuilder.DropIndex(
                name: "IX_ClaimBundles_ProfileId_OrganizationId",
                table: "ClaimBundles");

            migrationBuilder.DropIndex(
                name: "IX_ClaimBundleItems_ProfileInventoryItemId_OrganizationId",
                table: "ClaimBundleItems");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeSegments_SegmentId_OrganizationId",
                table: "ChallengeSegments");

            migrationBuilder.DropIndex(
                name: "IX_Challenges_SegmentId_OrganizationId",
                table: "Challenges");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeRewards_ItemId_OrganizationId",
                table: "ChallengeRewards");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeGoals_ChallengeId_OrganizationId",
                table: "ChallengeGoals");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeGoalCompletions_ProfileId_OrganizationId",
                table: "ChallengeGoalCompletions");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeCompletions_ProfileId_OrganizationId",
                table: "ChallengeCompletions");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeCommits_ProfileId_OrganizationId",
                table: "ChallengeCommits");

            migrationBuilder.DropIndex(
                name: "IX_ActionPointSettings_SegmentId_OrganizationId",
                table: "ActionPointSettings");

            migrationBuilder.DropIndex(
                name: "IX_ActionPointSegments_SegmentId_OrganizationId",
                table: "ActionPointSegments");

            migrationBuilder.DropIndex(
                name: "IX_ActionPointProfiles_ProfileId_OrganizationId",
                table: "ActionPointProfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamReportsWeekly",
                table: "TeamReportsWeekly",
                columns: new[] { "OrganizationId", "DateId", "TeamId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamReportsDaily",
                table: "TeamReportsDaily",
                columns: new[] { "OrganizationId", "DateId", "TeamId", "TaskCategoryId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ShopLogs_ShopId_LogId",
                table: "ShopLogs",
                columns: new[] { "ShopId", "LogId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ShopItemSegments_ShopItemId_SegmentId",
                table: "ShopItemSegments",
                columns: new[] { "ShopItemId", "SegmentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly",
                columns: new[] { "OrganizationId", "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily",
                columns: new[] { "OrganizationId", "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly",
                columns: new[] { "OrganizationId", "DateId", "ProfileId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily",
                columns: new[] { "OrganizationId", "DateId", "ProfileId", "TaskCategoryId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileOneUps_DateId_UppedProfileId_CreatedBy",
                table: "ProfileOneUps",
                columns: new[] { "DateId", "UppedProfileId", "CreatedBy" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileLogs_ProfileId_LogId",
                table: "ProfileLogs",
                columns: new[] { "ProfileId", "LogId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileExternalIds",
                table: "ProfileExternalIds",
                columns: new[] { "OrganizationId", "ExternalId", "IntegrationType" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_LogSettings_LogDeviceId_LogEvent",
                table: "LogSettings",
                columns: new[] { "LogDeviceId", "LogEvent" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompetitionLogs_CompetitionId_LogId",
                table: "CompetitionLogs",
                columns: new[] { "CompetitionId", "LogId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ClaimBundleTokenTxns_ClaimBundleId_TokenTransactionId",
                table: "ClaimBundleTokenTxns",
                columns: new[] { "ClaimBundleId", "TokenTransactionId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ClaimBundleItems_ClaimBundleId_ProfileInventoryItemId",
                table: "ClaimBundleItems",
                columns: new[] { "ClaimBundleId", "ProfileInventoryItemId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ChallengeSegments_ChallengeId_SegmentId",
                table: "ChallengeSegments",
                columns: new[] { "ChallengeId", "SegmentId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ChallengeRewards_ChallengeId_ItemId",
                table: "ChallengeRewards",
                columns: new[] { "ChallengeId", "ItemId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ChallengeGoalCompletions_GoalId_ProfileId",
                table: "ChallengeGoalCompletions",
                columns: new[] { "GoalId", "ProfileId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ChallengeCompletions_ChallengeId_ProfileId",
                table: "ChallengeCompletions",
                columns: new[] { "ChallengeId", "ProfileId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ChallengeCommits_ChallengeId_ProfileId",
                table: "ChallengeCommits",
                columns: new[] { "ChallengeId", "ProfileId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ActionPointSettings_Type",
                table: "ActionPointSettings",
                column: "Type");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ActionPointSegments_ActionPointId_SegmentId",
                table: "ActionPointSegments",
                columns: new[] { "ActionPointId", "SegmentId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ActionPointProfiles_ActionPointId_ProfileId",
                table: "ActionPointProfiles",
                columns: new[] { "ActionPointId", "ProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_ProfileId",
                table: "TokenTransactions",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_TokenId",
                table: "TokenTransactions",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchievedAt",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchievedAt" },
                unique: true,
                filter: "[Key] IS NOT NULL AND [ArchievedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TaskCategoryId",
                table: "TeamReportsWeekly",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TeamId",
                table: "TeamReportsWeekly",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TaskCategoryId",
                table: "TeamReportsDaily",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TeamId",
                table: "TeamReportsDaily",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_SegmentId",
                table: "TaskSyncs",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneeProfileId",
                table: "Tasks",
                column: "AssigneeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SegmentAreaId",
                table: "Tasks",
                column: "SegmentAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SegmentId",
                table: "Tasks",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskCategoryId",
                table: "Tasks",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TeamId",
                table: "Tasks",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType" },
                unique: true,
                filter: "[ExternalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_AssigneeProfileId",
                table: "TaskLogs",
                column: "AssigneeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_SegmentId",
                table: "TaskLogs",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_TeamId",
                table: "TaskLogs",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_Name",
                table: "TaskCategories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ItemId",
                table: "ShopPurchases",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_SegmentId",
                table: "ShopPurchases",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ProfileId_Status",
                table: "ShopPurchases",
                columns: new[] { "ProfileId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopLogs_LogId",
                table: "ShopLogs",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemSegments_SegmentId",
                table: "ShopItemSegments",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ItemId",
                table: "ShopItems",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key_ArchievedAt",
                table: "Segments",
                columns: new[] { "Key", "ArchievedAt" },
                unique: true,
                filter: "[ArchievedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_SegmentId",
                table: "SegmentReportsWeekly",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_TaskCategoryId",
                table: "SegmentReportsWeekly",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_SegmentId",
                table: "SegmentReportsDaily",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_TaskCategoryId",
                table: "SegmentReportsDaily",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentAreas_Name",
                table: "SegmentAreas",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_IdentityId",
                table: "Profiles",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Username",
                table: "Profiles",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_ProfileId",
                table: "ProfileReportsWeekly",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_TaskCategoryId",
                table: "ProfileReportsWeekly",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_ProfileId",
                table: "ProfileReportsDaily",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_TaskCategoryId",
                table: "ProfileReportsDaily",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileOneUps_UppedProfileId",
                table: "ProfileOneUps",
                column: "UppedProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_LogId",
                table: "ProfileLogs",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_ProfileId_Event",
                table: "ProfileLogs",
                columns: new[] { "ProfileId", "Event" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ProfileId_IsActive",
                table: "ProfileInventoryItems",
                columns: new[] { "ProfileId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ItemId_ProfileId_IsActive",
                table: "ProfileInventoryItems",
                columns: new[] { "ItemId", "ProfileId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_ProfileId",
                table: "ProfileExternalIds",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_SegmentId",
                table: "ProfileExternalIds",
                column: "SegmentId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LogSettings_ProfileId",
                table: "LogSettings",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_LogDevices_ProfileId",
                table: "LogDevices",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemReservations_ItemId",
                table: "ItemReservations",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ItemId",
                table: "ItemGifts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ReceiverId",
                table: "ItemGifts",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_SenderId",
                table: "ItemGifts",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ItemId",
                table: "ItemDisenchants",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ProfileId",
                table: "ItemDisenchants",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_SegmentId",
                table: "Invitations",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_TeamId",
                table: "Invitations",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_SegmentId",
                table: "Integrations",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_ProfileId_SegmentId",
                table: "Integrations",
                columns: new[] { "ProfileId", "SegmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationFields_IntegrationId",
                table: "IntegrationFields",
                column: "IntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_CompetitionId",
                table: "CompetitorScores",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_ProfileId",
                table: "CompetitorScores",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_TeamId",
                table: "CompetitorScores",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_CompetitorId_ProfileId",
                table: "CompetitorScores",
                columns: new[] { "CompetitorId", "ProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_CompetitorId_TeamId",
                table: "CompetitorScores",
                columns: new[] { "CompetitorId", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_ProfileId",
                table: "Competitors",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_TeamId",
                table: "Competitors",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId" },
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_TeamId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "TeamId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_TeamId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId", "TeamId" },
                unique: true,
                filter: "[ProfileId] IS NOT NULL AND [TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_PreviousCompetitionId",
                table: "Competitions",
                column: "PreviousCompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_SegmentId",
                table: "Competitions",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_TokenId",
                table: "Competitions",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_CompetitionId",
                table: "CompetitionRewards",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_ItemId",
                table: "CompetitionRewards",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_TokenId",
                table: "CompetitionRewards",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionLogs_LogId",
                table: "CompetitionLogs",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionLogs_CompetitionId_Event",
                table: "CompetitionLogs",
                columns: new[] { "CompetitionId", "Event" });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleTokenTxns_TokenTransactionId",
                table: "ClaimBundleTokenTxns",
                column: "TokenTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundles_ProfileId",
                table: "ClaimBundles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleItems_ProfileInventoryItemId",
                table: "ClaimBundleItems",
                column: "ProfileInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeSegments_SegmentId",
                table: "ChallengeSegments",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_SegmentId",
                table: "Challenges",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRewards_ItemId",
                table: "ChallengeRewards",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoals_ChallengeId",
                table: "ChallengeGoals",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoalCompletions_ProfileId",
                table: "ChallengeGoalCompletions",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCompletions_ProfileId",
                table: "ChallengeCompletions",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCommits_ProfileId",
                table: "ChallengeCommits",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSettings_SegmentId",
                table: "ActionPointSettings",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSegments_SegmentId",
                table: "ActionPointSegments",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointProfiles_ProfileId",
                table: "ActionPointProfiles",
                column: "ProfileId");
        }
    }
}
