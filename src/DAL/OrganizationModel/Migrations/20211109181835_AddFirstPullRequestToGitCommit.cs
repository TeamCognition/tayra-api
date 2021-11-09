using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class AddFirstPullRequestToGitCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FirstPullRequestId",
                table: "GitCommits",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GitCommits_FirstPullRequestId_TenantId",
                table: "GitCommits",
                columns: new[] { "FirstPullRequestId", "TenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GitCommits_PullRequests_FirstPullRequestId",
                table: "GitCommits",
                column: "FirstPullRequestId",
                principalTable: "PullRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GitCommits_PullRequests_FirstPullRequestId",
                table: "GitCommits");

            migrationBuilder.DropIndex(
                name: "IX_GitCommits_FirstPullRequestId_TenantId",
                table: "GitCommits");

            migrationBuilder.DropColumn(
                name: "FirstPullRequestId",
                table: "GitCommits");
        }
    }
}
