using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeAverage",
                table: "TeamReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeChange",
                table: "TeamReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeChange",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeTotal",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeAverage",
                table: "SegmentReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeChange",
                table: "SegmentReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeChange",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeTotal",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeAverage",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeChange",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeChange",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksCompletionTimeTotal",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeAverage",
                table: "TeamReportsWeekly");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeChange",
                table: "TeamReportsWeekly");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeChange",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeTotal",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeAverage",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeChange",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeChange",
                table: "SegmentReportsDaily");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeTotal",
                table: "SegmentReportsDaily");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeAverage",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeChange",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeChange",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "TasksCompletionTimeTotal",
                table: "ProfileReportsDaily");
        }
    }
}
