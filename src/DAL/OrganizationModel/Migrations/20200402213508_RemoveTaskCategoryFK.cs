using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class RemoveTaskCategoryFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsDaily_TaskCategories_TaskCategoryId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsWeekly_TaskCategories_TaskCategoryId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsDaily_TaskCategories_TaskCategoryId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsWeekly_TaskCategories_TaskCategoryId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsDaily_TaskCategories_TaskCategoryId",
                table: "TeamReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsWeekly_TaskCategories_TaskCategoryId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsWeekly_TaskCategoryId_OrganizationId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsDaily_TaskCategoryId_OrganizationId",
                table: "TeamReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsWeekly_TaskCategoryId_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsDaily_TaskCategoryId_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsWeekly_TaskCategoryId_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsDaily_TaskCategoryId_OrganizationId",
                table: "ProfileReportsDaily");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TaskCategoryId_OrganizationId",
                table: "TeamReportsWeekly",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TaskCategoryId_OrganizationId",
                table: "TeamReportsDaily",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_TaskCategoryId_OrganizationId",
                table: "SegmentReportsWeekly",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_TaskCategoryId_OrganizationId",
                table: "SegmentReportsDaily",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_TaskCategoryId_OrganizationId",
                table: "ProfileReportsWeekly",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_TaskCategoryId_OrganizationId",
                table: "ProfileReportsDaily",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsDaily_TaskCategories_TaskCategoryId",
                table: "ProfileReportsDaily",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsWeekly_TaskCategories_TaskCategoryId",
                table: "ProfileReportsWeekly",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsDaily_TaskCategories_TaskCategoryId",
                table: "SegmentReportsDaily",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsWeekly_TaskCategories_TaskCategoryId",
                table: "SegmentReportsWeekly",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamReportsDaily_TaskCategories_TaskCategoryId",
                table: "TeamReportsDaily",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamReportsWeekly_TaskCategories_TaskCategoryId",
                table: "TeamReportsWeekly",
                column: "TaskCategoryId",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
