using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class UpdateGithubTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternalAuthorId",
                table: "PullRequests",
                newName: "ExternalAuthorUsername");

            migrationBuilder.AddColumn<int>(
                name: "Additions",
                table: "PullRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Deletions",
                table: "PullRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewsCount",
                table: "PullRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Additions",
                table: "PullRequests");

            migrationBuilder.DropColumn(
                name: "Deletions",
                table: "PullRequests");

            migrationBuilder.DropColumn(
                name: "ReviewsCount",
                table: "PullRequests");

            migrationBuilder.RenameColumn(
                name: "ExternalAuthorUsername",
                table: "PullRequests",
                newName: "ExternalAuthorId");
        }
    }
}
