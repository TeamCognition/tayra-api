using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NonMembersCountTotal",
                table: "TeamReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SegmentId",
                table: "TeamReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsUnassigned",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NonMembersCountTotal",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SegmentId",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NonMembersCountTotal",
                table: "SegmentReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NonMembersCountTotal",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileRole",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileRole",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NonMembersCountTotal",
                table: "TeamReportsWeekly");

            migrationBuilder.DropColumn(
                name: "SegmentId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropColumn(
                name: "IsUnassigned",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "NonMembersCountTotal",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "SegmentId",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "NonMembersCountTotal",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropColumn(
                name: "NonMembersCountTotal",
                table: "SegmentReportsDaily");

            migrationBuilder.DropColumn(
                name: "ProfileRole",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "ProfileRole",
                table: "ProfileReportsDaily");
        }
    }
}
