using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class RenameCommitedAtToCommittedAtInGitCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommitedAt",
                table: "GitCommits",
                newName: "CommittedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommittedAt",
                table: "GitCommits",
                newName: "CommitedAt");
        }
    }
}
