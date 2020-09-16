using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class ChangeClaimBundleRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionPoints_Organizations_OrganizationId",
                table: "ActionPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionPointSettings_Organizations_OrganizationId",
                table: "ActionPointSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Blobs_Organizations_OrganizationId",
                table: "Blobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBundleItems_Organizations_OrganizationId",
                table: "ClaimBundleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBundleItems_ProfileInventoryItems_ProfileInventoryItemId",
                table: "ClaimBundleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBundles_Organizations_OrganizationId",
                table: "ClaimBundles");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBundleTokenTxns_Organizations_OrganizationId",
                table: "ClaimBundleTokenTxns");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionLogs_Organizations_OrganizationId",
                table: "CompetitionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionRewards_Organizations_OrganizationId",
                table: "CompetitionRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Organizations_OrganizationId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitors_Organizations_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitorScores_Organizations_OrganizationId",
                table: "CompetitorScores");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityChangeLogs_Organizations_OrganizationId",
                table: "EntityChangeLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GitCommits_Organizations_OrganizationId",
                table: "GitCommits");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationFields_Organizations_OrganizationId",
                table: "IntegrationFields");

            migrationBuilder.DropForeignKey(
                name: "FK_Integrations_Organizations_OrganizationId",
                table: "Integrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Organizations_OrganizationId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDisenchants_Organizations_OrganizationId",
                table: "ItemDisenchants");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemGifts_Organizations_OrganizationId",
                table: "ItemGifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Organizations_OrganizationId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_LogDevices_Organizations_OrganizationId",
                table: "LogDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_LoginLogs_Organizations_OrganizationId",
                table: "LoginLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Organizations_OrganizationId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_LogSettings_Organizations_OrganizationId",
                table: "LogSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileAssignments_Organizations_OrganizationId",
                table: "ProfileAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                table: "ProfileExternalIds");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileInventoryItems_Organizations_OrganizationId",
                table: "ProfileInventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileLogs_Organizations_OrganizationId",
                table: "ProfileLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileMetrics_Organizations_OrganizationId",
                table: "ProfileMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePraises_Organizations_OrganizationId",
                table: "ProfilePraises");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsDaily_Organizations_OrganizationId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsWeekly_Organizations_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Organizations_OrganizationId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestCommits_Organizations_OrganizationId",
                table: "QuestCommits");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestCompletions_Organizations_OrganizationId",
                table: "QuestCompletions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestGoalCompletions_Organizations_OrganizationId",
                table: "QuestGoalCompletions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestGoals_Organizations_OrganizationId",
                table: "QuestGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestRewards_Organizations_OrganizationId",
                table: "QuestRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Organizations_OrganizationId",
                table: "Quests");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestSegments_Organizations_OrganizationId",
                table: "QuestSegments");

            migrationBuilder.DropForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentAreas_Organizations_OrganizationId",
                table: "SegmentAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentMetrics_Organizations_OrganizationId",
                table: "SegmentMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_Segments_Organizations_OrganizationId",
                table: "Segments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItems_Organizations_OrganizationId",
                table: "ShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemSegments_Organizations_OrganizationId",
                table: "ShopItemSegments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopLogs_Organizations_OrganizationId",
                table: "ShopLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopPurchases_Organizations_OrganizationId",
                table: "ShopPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Organizations_OrganizationId",
                table: "Shops");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_Organizations_OrganizationId",
                table: "TaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Organizations_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskSyncs_Organizations_OrganizationId",
                table: "TaskSyncs");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsDaily_Organizations_OrganizationId",
                table: "TeamReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsWeekly_Organizations_OrganizationId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Organizations_OrganizationId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Organizations_OrganizationId",
                table: "Tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_TokenTransactions_Organizations_OrganizationId",
                table: "TokenTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WebhookEventLogs_Organizations_OrganizationId",
                table: "WebhookEventLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPoints_Organizations_OrganizationId",
                table: "ActionPoints",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPointSettings_Organizations_OrganizationId",
                table: "ActionPointSettings",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blobs_Organizations_OrganizationId",
                table: "Blobs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBundleItems_Organizations_OrganizationId",
                table: "ClaimBundleItems",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBundleItems_ProfileInventoryItems_ProfileInventoryItemId",
                table: "ClaimBundleItems",
                column: "ProfileInventoryItemId",
                principalTable: "ProfileInventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBundles_Organizations_OrganizationId",
                table: "ClaimBundles",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBundleTokenTxns_Organizations_OrganizationId",
                table: "ClaimBundleTokenTxns",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionLogs_Organizations_OrganizationId",
                table: "CompetitionLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionRewards_Organizations_OrganizationId",
                table: "CompetitionRewards",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Organizations_OrganizationId",
                table: "Competitions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitors_Organizations_OrganizationId",
                table: "Competitors",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitorScores_Organizations_OrganizationId",
                table: "CompetitorScores",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityChangeLogs_Organizations_OrganizationId",
                table: "EntityChangeLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GitCommits_Organizations_OrganizationId",
                table: "GitCommits",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationFields_Organizations_OrganizationId",
                table: "IntegrationFields",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Integrations_Organizations_OrganizationId",
                table: "Integrations",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Organizations_OrganizationId",
                table: "Invitations",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDisenchants_Organizations_OrganizationId",
                table: "ItemDisenchants",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemGifts_Organizations_OrganizationId",
                table: "ItemGifts",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Organizations_OrganizationId",
                table: "Items",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LogDevices_Organizations_OrganizationId",
                table: "LogDevices",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoginLogs_Organizations_OrganizationId",
                table: "LoginLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Organizations_OrganizationId",
                table: "Logs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LogSettings_Organizations_OrganizationId",
                table: "LogSettings",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileAssignments_Organizations_OrganizationId",
                table: "ProfileAssignments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                table: "ProfileExternalIds",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInventoryItems_Organizations_OrganizationId",
                table: "ProfileInventoryItems",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileLogs_Organizations_OrganizationId",
                table: "ProfileLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileMetrics_Organizations_OrganizationId",
                table: "ProfileMetrics",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePraises_Organizations_OrganizationId",
                table: "ProfilePraises",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsDaily_Organizations_OrganizationId",
                table: "ProfileReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsWeekly_Organizations_OrganizationId",
                table: "ProfileReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Organizations_OrganizationId",
                table: "Profiles",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestCommits_Organizations_OrganizationId",
                table: "QuestCommits",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestCompletions_Organizations_OrganizationId",
                table: "QuestCompletions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestGoalCompletions_Organizations_OrganizationId",
                table: "QuestGoalCompletions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestGoals_Organizations_OrganizationId",
                table: "QuestGoals",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestRewards_Organizations_OrganizationId",
                table: "QuestRewards",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Organizations_OrganizationId",
                table: "Quests",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestSegments_Organizations_OrganizationId",
                table: "QuestSegments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentAreas_Organizations_OrganizationId",
                table: "SegmentAreas",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentMetrics_Organizations_OrganizationId",
                table: "SegmentMetrics",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                table: "SegmentReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                table: "SegmentReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
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
                name: "FK_ShopItems_Organizations_OrganizationId",
                table: "ShopItems",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemSegments_Organizations_OrganizationId",
                table: "ShopItemSegments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopLogs_Organizations_OrganizationId",
                table: "ShopLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopPurchases_Organizations_OrganizationId",
                table: "ShopPurchases",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Organizations_OrganizationId",
                table: "Shops",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_Organizations_OrganizationId",
                table: "TaskLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Organizations_OrganizationId",
                table: "Tasks",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskSyncs_Organizations_OrganizationId",
                table: "TaskSyncs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamReportsDaily_Organizations_OrganizationId",
                table: "TeamReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamReportsWeekly_Organizations_OrganizationId",
                table: "TeamReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Organizations_OrganizationId",
                table: "Teams",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Organizations_OrganizationId",
                table: "Tokens",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TokenTransactions_Organizations_OrganizationId",
                table: "TokenTransactions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebhookEventLogs_Organizations_OrganizationId",
                table: "WebhookEventLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionPoints_Organizations_OrganizationId",
                table: "ActionPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionPointSettings_Organizations_OrganizationId",
                table: "ActionPointSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Blobs_Organizations_OrganizationId",
                table: "Blobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBundleItems_Organizations_OrganizationId",
                table: "ClaimBundleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBundleItems_ProfileInventoryItems_ProfileInventoryItemId",
                table: "ClaimBundleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBundles_Organizations_OrganizationId",
                table: "ClaimBundles");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBundleTokenTxns_Organizations_OrganizationId",
                table: "ClaimBundleTokenTxns");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionLogs_Organizations_OrganizationId",
                table: "CompetitionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionRewards_Organizations_OrganizationId",
                table: "CompetitionRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Organizations_OrganizationId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitors_Organizations_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitorScores_Organizations_OrganizationId",
                table: "CompetitorScores");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityChangeLogs_Organizations_OrganizationId",
                table: "EntityChangeLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_GitCommits_Organizations_OrganizationId",
                table: "GitCommits");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegrationFields_Organizations_OrganizationId",
                table: "IntegrationFields");

            migrationBuilder.DropForeignKey(
                name: "FK_Integrations_Organizations_OrganizationId",
                table: "Integrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Organizations_OrganizationId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDisenchants_Organizations_OrganizationId",
                table: "ItemDisenchants");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemGifts_Organizations_OrganizationId",
                table: "ItemGifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Organizations_OrganizationId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_LogDevices_Organizations_OrganizationId",
                table: "LogDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_LoginLogs_Organizations_OrganizationId",
                table: "LoginLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Organizations_OrganizationId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_LogSettings_Organizations_OrganizationId",
                table: "LogSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileAssignments_Organizations_OrganizationId",
                table: "ProfileAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                table: "ProfileExternalIds");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileInventoryItems_Organizations_OrganizationId",
                table: "ProfileInventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileLogs_Organizations_OrganizationId",
                table: "ProfileLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileMetrics_Organizations_OrganizationId",
                table: "ProfileMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePraises_Organizations_OrganizationId",
                table: "ProfilePraises");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsDaily_Organizations_OrganizationId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsWeekly_Organizations_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Organizations_OrganizationId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestCommits_Organizations_OrganizationId",
                table: "QuestCommits");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestCompletions_Organizations_OrganizationId",
                table: "QuestCompletions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestGoalCompletions_Organizations_OrganizationId",
                table: "QuestGoalCompletions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestGoals_Organizations_OrganizationId",
                table: "QuestGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestRewards_Organizations_OrganizationId",
                table: "QuestRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Organizations_OrganizationId",
                table: "Quests");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestSegments_Organizations_OrganizationId",
                table: "QuestSegments");

            migrationBuilder.DropForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentAreas_Organizations_OrganizationId",
                table: "SegmentAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentMetrics_Organizations_OrganizationId",
                table: "SegmentMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_Segments_Organizations_OrganizationId",
                table: "Segments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItems_Organizations_OrganizationId",
                table: "ShopItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemSegments_Organizations_OrganizationId",
                table: "ShopItemSegments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopLogs_Organizations_OrganizationId",
                table: "ShopLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopPurchases_Organizations_OrganizationId",
                table: "ShopPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Organizations_OrganizationId",
                table: "Shops");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLogs_Organizations_OrganizationId",
                table: "TaskLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Organizations_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskSyncs_Organizations_OrganizationId",
                table: "TaskSyncs");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsDaily_Organizations_OrganizationId",
                table: "TeamReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsWeekly_Organizations_OrganizationId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Organizations_OrganizationId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Organizations_OrganizationId",
                table: "Tokens");

            migrationBuilder.DropForeignKey(
                name: "FK_TokenTransactions_Organizations_OrganizationId",
                table: "TokenTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WebhookEventLogs_Organizations_OrganizationId",
                table: "WebhookEventLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPoints_Organizations_OrganizationId",
                table: "ActionPoints",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionPointSettings_Organizations_OrganizationId",
                table: "ActionPointSettings",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Blobs_Organizations_OrganizationId",
                table: "Blobs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBundleItems_Organizations_OrganizationId",
                table: "ClaimBundleItems",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBundleItems_ProfileInventoryItems_ProfileInventoryItemId",
                table: "ClaimBundleItems",
                column: "ProfileInventoryItemId",
                principalTable: "ProfileInventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBundles_Organizations_OrganizationId",
                table: "ClaimBundles",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBundleTokenTxns_Organizations_OrganizationId",
                table: "ClaimBundleTokenTxns",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionLogs_Organizations_OrganizationId",
                table: "CompetitionLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionRewards_Organizations_OrganizationId",
                table: "CompetitionRewards",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Organizations_OrganizationId",
                table: "Competitions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitors_Organizations_OrganizationId",
                table: "Competitors",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitorScores_Organizations_OrganizationId",
                table: "CompetitorScores",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityChangeLogs_Organizations_OrganizationId",
                table: "EntityChangeLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GitCommits_Organizations_OrganizationId",
                table: "GitCommits",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IntegrationFields_Organizations_OrganizationId",
                table: "IntegrationFields",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Integrations_Organizations_OrganizationId",
                table: "Integrations",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Organizations_OrganizationId",
                table: "Invitations",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDisenchants_Organizations_OrganizationId",
                table: "ItemDisenchants",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemGifts_Organizations_OrganizationId",
                table: "ItemGifts",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Organizations_OrganizationId",
                table: "Items",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LogDevices_Organizations_OrganizationId",
                table: "LogDevices",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoginLogs_Organizations_OrganizationId",
                table: "LoginLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Organizations_OrganizationId",
                table: "Logs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LogSettings_Organizations_OrganizationId",
                table: "LogSettings",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileAssignments_Organizations_OrganizationId",
                table: "ProfileAssignments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                table: "ProfileExternalIds",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInventoryItems_Organizations_OrganizationId",
                table: "ProfileInventoryItems",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileLogs_Organizations_OrganizationId",
                table: "ProfileLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileMetrics_Organizations_OrganizationId",
                table: "ProfileMetrics",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePraises_Organizations_OrganizationId",
                table: "ProfilePraises",
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
                name: "FK_Profiles_Organizations_OrganizationId",
                table: "Profiles",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestCommits_Organizations_OrganizationId",
                table: "QuestCommits",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestCompletions_Organizations_OrganizationId",
                table: "QuestCompletions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestGoalCompletions_Organizations_OrganizationId",
                table: "QuestGoalCompletions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestGoals_Organizations_OrganizationId",
                table: "QuestGoals",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestRewards_Organizations_OrganizationId",
                table: "QuestRewards",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Organizations_OrganizationId",
                table: "Quests",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestSegments_Organizations_OrganizationId",
                table: "QuestSegments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentAreas_Organizations_OrganizationId",
                table: "SegmentAreas",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentMetrics_Organizations_OrganizationId",
                table: "SegmentMetrics",
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
                name: "FK_Segments_Organizations_OrganizationId",
                table: "Segments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItems_Organizations_OrganizationId",
                table: "ShopItems",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemSegments_Organizations_OrganizationId",
                table: "ShopItemSegments",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopLogs_Organizations_OrganizationId",
                table: "ShopLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopPurchases_Organizations_OrganizationId",
                table: "ShopPurchases",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Organizations_OrganizationId",
                table: "Shops",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLogs_Organizations_OrganizationId",
                table: "TaskLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Organizations_OrganizationId",
                table: "Tasks",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskSyncs_Organizations_OrganizationId",
                table: "TaskSyncs",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Organizations_OrganizationId",
                table: "Teams",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Organizations_OrganizationId",
                table: "Tokens",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TokenTransactions_Organizations_OrganizationId",
                table: "TokenTransactions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WebhookEventLogs_Organizations_OrganizationId",
                table: "WebhookEventLogs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
