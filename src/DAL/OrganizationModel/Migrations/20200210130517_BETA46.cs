using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyTokensChange",
                table: "TeamReportsWeekly",
                newName: "CompanyTokensSpentChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensAverage",
                table: "TeamReportsWeekly",
                newName: "CompanyTokensSpentAverage");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensTotal",
                table: "TeamReportsDaily",
                newName: "CompanyTokensSpentTotal");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensChange",
                table: "TeamReportsDaily",
                newName: "CompanyTokensSpentChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensChange",
                table: "SegmentReportsWeekly",
                newName: "CompanyTokensSpentChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensAverage",
                table: "SegmentReportsWeekly",
                newName: "CompanyTokensSpentAverage");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensTotal",
                table: "SegmentReportsDaily",
                newName: "CompanyTokensSpentTotal");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensChange",
                table: "SegmentReportsDaily",
                newName: "CompanyTokensSpentChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensTotalAverage",
                table: "ProfileReportsWeekly",
                newName: "CompanyTokensSpentTotalAverage");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensChange",
                table: "ProfileReportsWeekly",
                newName: "CompanyTokensSpentChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensTotal",
                table: "ProfileReportsDaily",
                newName: "CompanyTokensSpentTotal");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensChange",
                table: "ProfileReportsDaily",
                newName: "CompanyTokensSpentChange");

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedAverage",
                table: "TeamReportsWeekly",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedChange",
                table: "TeamReportsWeekly",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedChange",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedTotal",
                table: "TeamReportsDaily",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedAverage",
                table: "SegmentReportsWeekly",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedChange",
                table: "SegmentReportsWeekly",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedChange",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedTotal",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedChange",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedTotalAverage",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedChange",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CompanyTokensEarnedTotal",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedAverage",
                table: "TeamReportsWeekly");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedChange",
                table: "TeamReportsWeekly");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedChange",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedTotal",
                table: "TeamReportsDaily");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedAverage",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedChange",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedChange",
                table: "SegmentReportsDaily");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedTotal",
                table: "SegmentReportsDaily");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedChange",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedTotalAverage",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedChange",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "CompanyTokensEarnedTotal",
                table: "ProfileReportsDaily");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentChange",
                table: "TeamReportsWeekly",
                newName: "CompanyTokensChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentAverage",
                table: "TeamReportsWeekly",
                newName: "CompanyTokensAverage");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentTotal",
                table: "TeamReportsDaily",
                newName: "CompanyTokensTotal");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentChange",
                table: "TeamReportsDaily",
                newName: "CompanyTokensChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentChange",
                table: "SegmentReportsWeekly",
                newName: "CompanyTokensChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentAverage",
                table: "SegmentReportsWeekly",
                newName: "CompanyTokensAverage");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentTotal",
                table: "SegmentReportsDaily",
                newName: "CompanyTokensTotal");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentChange",
                table: "SegmentReportsDaily",
                newName: "CompanyTokensChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentTotalAverage",
                table: "ProfileReportsWeekly",
                newName: "CompanyTokensTotalAverage");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentChange",
                table: "ProfileReportsWeekly",
                newName: "CompanyTokensChange");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentTotal",
                table: "ProfileReportsDaily",
                newName: "CompanyTokensTotal");

            migrationBuilder.RenameColumn(
                name: "CompanyTokensSpentChange",
                table: "ProfileReportsDaily",
                newName: "CompanyTokensChange");
        }
    }
}
