using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomReward",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "TokenRewardValue",
                table: "Challenges");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ShopItems",
                newName: "QuantityReserved");

            migrationBuilder.AddColumn<DateTime>(
                name: "BornOn",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmployedOn",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "RewardValue",
                table: "Challenges",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "ChallengeCommits",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    ChallengeId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeCommits", x => new { x.ChallengeId, x.ProfileId, x.OrganizationId });
                    table.UniqueConstraint("AK_ChallengeCommits_ChallengeId_ProfileId", x => new { x.ChallengeId, x.ProfileId });
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
                name: "ChallengeGoalCompletions",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    ChallengeId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeGoalCompletions", x => new { x.ChallengeId, x.ProfileId, x.OrganizationId });
                    table.UniqueConstraint("AK_ChallengeGoalCompletions_ChallengeId_ProfileId", x => new { x.ChallengeId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_ChallengeGoalCompletions_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
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

            migrationBuilder.CreateTable(
                name: "ChallengeGoals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    IsCommentRequired = table.Column<bool>(nullable: false),
                    ChallengeId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                    QuantityReserved = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeRewards", x => new { x.ChallengeId, x.ItemId, x.OrganizationId });
                    table.UniqueConstraint("AK_ChallengeRewards_ChallengeId_ItemId", x => new { x.ChallengeId, x.ItemId });
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

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCommits_OrganizationId",
                table: "ChallengeCommits",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCommits_ProfileId",
                table: "ChallengeCommits",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoalCompletions_OrganizationId",
                table: "ChallengeGoalCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoalCompletions_ProfileId",
                table: "ChallengeGoalCompletions",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoals_ChallengeId",
                table: "ChallengeGoals",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoals_OrganizationId",
                table: "ChallengeGoals",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRewards_ItemId",
                table: "ChallengeRewards",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRewards_OrganizationId",
                table: "ChallengeRewards",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeCommits");

            migrationBuilder.DropTable(
                name: "ChallengeGoalCompletions");

            migrationBuilder.DropTable(
                name: "ChallengeGoals");

            migrationBuilder.DropTable(
                name: "ChallengeRewards");

            migrationBuilder.DropColumn(
                name: "BornOn",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "EmployedOn",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RewardValue",
                table: "Challenges");

            migrationBuilder.RenameColumn(
                name: "QuantityReserved",
                table: "ShopItems",
                newName: "Quantity");

            migrationBuilder.AddColumn<string>(
                name: "CustomReward",
                table: "Challenges",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TokenRewardValue",
                table: "Challenges",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
