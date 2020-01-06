using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeGoalCompletions_Challenges_ChallengeId",
                table: "ChallengeGoalCompletions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ChallengeGoalCompletions_ChallengeId_ProfileId",
                table: "ChallengeGoalCompletions");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ChallengeId",
                table: "ChallengeGoalCompletions",
                newName: "GoalId");

            migrationBuilder.AddColumn<bool>(
                name: "IsQuantityLimited",
                table: "Items",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ChallengeGoalCompletions_GoalId_ProfileId",
                table: "ChallengeGoalCompletions",
                columns: new[] { "GoalId", "ProfileId" });

            migrationBuilder.CreateTable(
                name: "ItemReservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    QuantityChange = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReservations", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ItemReservations_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemReservations_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemReservations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemReservations_ItemId",
                table: "ItemReservations",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemReservations_OrganizationId",
                table: "ItemReservations",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeGoalCompletions_ChallengeGoals_GoalId",
                table: "ChallengeGoalCompletions",
                column: "GoalId",
                principalTable: "ChallengeGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeGoalCompletions_ChallengeGoals_GoalId",
                table: "ChallengeGoalCompletions");

            migrationBuilder.DropTable(
                name: "ItemReservations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ChallengeGoalCompletions_GoalId_ProfileId",
                table: "ChallengeGoalCompletions");

            migrationBuilder.DropColumn(
                name: "IsQuantityLimited",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "GoalId",
                table: "ChallengeGoalCompletions",
                newName: "ChallengeId");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Items",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ChallengeGoalCompletions_ChallengeId_ProfileId",
                table: "ChallengeGoalCompletions",
                columns: new[] { "ChallengeId", "ProfileId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeGoalCompletions_Challenges_ChallengeId",
                table: "ChallengeGoalCompletions",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
