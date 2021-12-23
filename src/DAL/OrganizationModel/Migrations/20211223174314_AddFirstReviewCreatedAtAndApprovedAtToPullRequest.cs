using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class AddFirstReviewCreatedAtAndApprovedAtToPullRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "PullRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstReviewCreatedAt",
                table: "PullRequests",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "PullRequests");

            migrationBuilder.DropColumn(
                name: "FirstReviewCreatedAt",
                table: "PullRequests");
        }
    }
}
