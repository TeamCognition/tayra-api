using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class AddGithubPRTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PullRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    ExternalUrl = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ClosedAt = table.Column<DateTime>(nullable: false),
                    MergedAt = table.Column<DateTime>(nullable: false),
                    CommitsCount = table.Column<int>(nullable: false),
                    ReviewCommentsCount = table.Column<int>(nullable: false),
                    ExternalAuthorId = table.Column<string>(nullable: true),
                    AuthorProfileId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PullRequests", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_PullRequests_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequests_Profiles_AuthorProfileId",
                        column: x => x.AuthorProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequests_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PullRequestReviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    CommitId = table.Column<string>(nullable: true),
                    SubmittedAt = table.Column<DateTime>(nullable: false),
                    PullRequestId = table.Column<int>(nullable: false),
                    ReviewerExternalId = table.Column<string>(nullable: true),
                    ReviewerProfileId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PullRequestReviews", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_PullRequestReviews_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequestReviews_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviews_PullRequests_PullRequestId",
                        column: x => x.PullRequestId,
                        principalTable: "PullRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviews_Profiles_ReviewerProfileId",
                        column: x => x.ReviewerProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PullRequestReviewComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    ExternalUrl = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CommentedProfileId = table.Column<int>(nullable: true),
                    UserCommentedPullRequestReviewProfileId = table.Column<int>(nullable: true),
                    PullRequestReviewId = table.Column<int>(nullable: false),
                    PullRequestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PullRequestReviewComments", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_PullRequestReviewComments_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequestReviewComments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviewComments_PullRequests_PullRequestId",
                        column: x => x.PullRequestId,
                        principalTable: "PullRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviewComments_PullRequestReviews_PullRequestReviewId",
                        column: x => x.PullRequestReviewId,
                        principalTable: "PullRequestReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviewComments_Profiles_UserCommentedPullRequestReviewProfileId",
                        column: x => x.UserCommentedPullRequestReviewProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_OrganizationId",
                table: "PullRequestReviewComments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_PullRequestId_OrganizationId",
                table: "PullRequestReviewComments",
                columns: new[] { "PullRequestId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_PullRequestReviewId_OrganizationId",
                table: "PullRequestReviewComments",
                columns: new[] { "PullRequestReviewId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_UserCommentedPullRequestReviewProfileId_OrganizationId",
                table: "PullRequestReviewComments",
                columns: new[] { "UserCommentedPullRequestReviewProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviews_OrganizationId",
                table: "PullRequestReviews",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviews_PullRequestId_OrganizationId",
                table: "PullRequestReviews",
                columns: new[] { "PullRequestId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviews_ReviewerProfileId_OrganizationId",
                table: "PullRequestReviews",
                columns: new[] { "ReviewerProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_OrganizationId",
                table: "PullRequests",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_AuthorProfileId_OrganizationId",
                table: "PullRequests",
                columns: new[] { "AuthorProfileId", "OrganizationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PullRequestReviewComments");

            migrationBuilder.DropTable(
                name: "PullRequestReviews");

            migrationBuilder.DropTable(
                name: "PullRequests");
        }
    }
}
