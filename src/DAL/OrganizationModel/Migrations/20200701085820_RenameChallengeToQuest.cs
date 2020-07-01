using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class RenameChallengeToQuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeCommits");

            migrationBuilder.DropTable(
                name: "ChallengeCompletions");

            migrationBuilder.DropTable(
                name: "ChallengeGoalCompletions");

            migrationBuilder.DropTable(
                name: "ChallengeRewards");

            migrationBuilder.DropTable(
                name: "ChallengeSegments");

            migrationBuilder.DropTable(
                name: "ChallengeGoals");

            migrationBuilder.DropTable(
                name: "Challenges");

            migrationBuilder.RenameColumn(
                name: "ChallengesQuantityRemaining",
                table: "Items",
                newName: "QuestsQuantityRemaining");

            migrationBuilder.AddColumn<int>(
                name: "ItemsBoughtChange",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsBoughtTotal",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestsCompletedChange",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestsCompletedTotal",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestsCompletedChange",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestsCompletedTotal",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CompletionsLimit = table.Column<int>(nullable: true),
                    CompletionsRemaining = table.Column<int>(nullable: true),
                    IsEasterEgg = table.Column<bool>(nullable: false),
                    IsArchived = table.Column<bool>(nullable: false),
                    ActiveUntil = table.Column<DateTime>(nullable: true),
                    EndedAt = table.Column<DateTime>(nullable: true),
                    RewardValue = table.Column<float>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    SegmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Quests_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quests_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quests_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestCommits",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    QuestId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestCommits", x => new { x.QuestId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestCommits_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestCommits_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestCommits_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestCompletions",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    QuestId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestCompletions", x => new { x.QuestId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestCompletions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestCompletions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestCompletions_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestGoals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    IsCommentRequired = table.Column<bool>(nullable: false),
                    QuestId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestGoals", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_QuestGoals_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestGoals_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestGoals_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestRewards",
                columns: table => new
                {
                    QuestId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestRewards", x => new { x.QuestId, x.ItemId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestRewards_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestRewards_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestRewards_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestSegments",
                columns: table => new
                {
                    QuestId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestSegments", x => new { x.QuestId, x.SegmentId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestSegments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestSegments_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestSegments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestGoalCompletions",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    GoalId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestGoalCompletions", x => new { x.GoalId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestGoalCompletions_QuestGoals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "QuestGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestGoalCompletions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestGoalCompletions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestCommits_OrganizationId",
                table: "QuestCommits",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestCommits_ProfileId_OrganizationId",
                table: "QuestCommits",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestCompletions_OrganizationId",
                table: "QuestCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestCompletions_ProfileId_OrganizationId",
                table: "QuestCompletions",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoalCompletions_OrganizationId",
                table: "QuestGoalCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoalCompletions_ProfileId_OrganizationId",
                table: "QuestGoalCompletions",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoals_OrganizationId",
                table: "QuestGoals",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoals_QuestId_OrganizationId",
                table: "QuestGoals",
                columns: new[] { "QuestId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestRewards_OrganizationId",
                table: "QuestRewards",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestRewards_ItemId_OrganizationId",
                table: "QuestRewards",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Quests_OrganizationId",
                table: "Quests",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_SegmentId_OrganizationId",
                table: "Quests",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestSegments_OrganizationId",
                table: "QuestSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestSegments_SegmentId_OrganizationId",
                table: "QuestSegments",
                columns: new[] { "SegmentId", "OrganizationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestCommits");

            migrationBuilder.DropTable(
                name: "QuestCompletions");

            migrationBuilder.DropTable(
                name: "QuestGoalCompletions");

            migrationBuilder.DropTable(
                name: "QuestRewards");

            migrationBuilder.DropTable(
                name: "QuestSegments");

            migrationBuilder.DropTable(
                name: "QuestGoals");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropColumn(
                name: "ItemsBoughtChange",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsBoughtTotal",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "QuestsCompletedChange",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "QuestsCompletedTotal",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "QuestsCompletedChange",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "QuestsCompletedTotal",
                table: "ProfileReportsDaily");

            migrationBuilder.RenameColumn(
                name: "QuestsQuantityRemaining",
                table: "Items",
                newName: "ChallengesQuantityRemaining");

            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ActiveUntil = table.Column<DateTime>(nullable: true),
                    CompletionsLimit = table.Column<int>(nullable: true),
                    CompletionsRemaining = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EndedAt = table.Column<DateTime>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    IsArchived = table.Column<bool>(nullable: false),
                    IsEasterEgg = table.Column<bool>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RewardValue = table.Column<float>(nullable: false),
                    SegmentId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Challenges_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenges_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Challenges_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeCommits",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeCommits", x => new { x.ChallengeId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeCommits_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeCommits_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeCommits_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeCompletions",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeCompletions", x => new { x.ChallengeId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeCompletions_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeCompletions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeCompletions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeGoals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ChallengeId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    IsCommentRequired = table.Column<bool>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeGoals", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ChallengeGoals_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeGoals_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeGoals_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeRewards",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeRewards", x => new { x.ChallengeId, x.ItemId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeRewards_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeRewards_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeRewards_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeSegments",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeSegments", x => new { x.ChallengeId, x.SegmentId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeSegments_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeSegments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeSegments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeGoalCompletions",
                columns: table => new
                {
                    GoalId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeGoalCompletions", x => new { x.GoalId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeGoalCompletions_ChallengeGoals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "ChallengeGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeGoalCompletions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeGoalCompletions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCommits_OrganizationId",
                table: "ChallengeCommits",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCommits_ProfileId_OrganizationId",
                table: "ChallengeCommits",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCompletions_OrganizationId",
                table: "ChallengeCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCompletions_ProfileId_OrganizationId",
                table: "ChallengeCompletions",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoalCompletions_OrganizationId",
                table: "ChallengeGoalCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoalCompletions_ProfileId_OrganizationId",
                table: "ChallengeGoalCompletions",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoals_OrganizationId",
                table: "ChallengeGoals",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoals_ChallengeId_OrganizationId",
                table: "ChallengeGoals",
                columns: new[] { "ChallengeId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRewards_OrganizationId",
                table: "ChallengeRewards",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRewards_ItemId_OrganizationId",
                table: "ChallengeRewards",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_OrganizationId",
                table: "Challenges",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_SegmentId_OrganizationId",
                table: "Challenges",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeSegments_OrganizationId",
                table: "ChallengeSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeSegments_SegmentId_OrganizationId",
                table: "ChallengeSegments",
                columns: new[] { "SegmentId", "OrganizationId" });
        }
    }
}
