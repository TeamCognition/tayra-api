using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class UpdateGithubTablesAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SHA",
                table: "GitCommits",
                newName: "Sha");

            migrationBuilder.AddColumn<string>(
                name: "ExternalRepositoryId",
                table: "PullRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalRepositoryId",
                table: "GitCommits",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalRepositoryId",
                table: "PullRequests");

            migrationBuilder.DropColumn(
                name: "ExternalRepositoryId",
                table: "GitCommits");

            migrationBuilder.RenameColumn(
                name: "Sha",
                table: "GitCommits",
                newName: "SHA");
        }
    }
}
