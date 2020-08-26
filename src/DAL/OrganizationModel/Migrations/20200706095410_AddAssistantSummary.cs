using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class AddAssistantSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssistantSummary",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssistantSummary",
                table: "Segments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssistantSummary",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssistantSummary",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AssistantSummary",
                table: "Segments");

            migrationBuilder.DropColumn(
                name: "AssistantSummary",
                table: "Profiles");
        }
    }
}
