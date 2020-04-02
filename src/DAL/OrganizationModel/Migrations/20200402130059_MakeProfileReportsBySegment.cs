using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class MakeProfileReportsBySegment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily");

            migrationBuilder.AddColumn<int>(
                name: "SegmentId",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SegmentId",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly",
                columns: new[] { "DateId", "ProfileId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily",
                columns: new[] { "DateId", "ProfileId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_SegmentId_OrganizationId",
                table: "ProfileReportsWeekly",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_SegmentId_OrganizationId",
                table: "ProfileReportsDaily",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsDaily_Segments_SegmentId",
                table: "ProfileReportsDaily",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsWeekly_Segments_SegmentId",
                table: "ProfileReportsWeekly",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsDaily_Segments_SegmentId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsWeekly_Segments_SegmentId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsWeekly_SegmentId_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsDaily_SegmentId_OrganizationId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "SegmentId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "SegmentId",
                table: "ProfileReportsDaily");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly",
                columns: new[] { "DateId", "ProfileId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily",
                columns: new[] { "DateId", "ProfileId", "TaskCategoryId", "OrganizationId" });
        }
    }
}
