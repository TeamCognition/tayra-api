using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA61 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsDaily_Organizations_OrganizationId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileReportsWeekly_Organizations_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsDaily_Organizations_OrganizationId",
                table: "TeamReportsDaily");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamReportsWeekly_Organizations_OrganizationId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TeamReportsWeekly_DateId_TeamId_TaskCategoryId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamReportsWeekly",
                table: "TeamReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsWeekly_OrganizationId",
                table: "TeamReportsWeekly");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TeamReportsDaily_DateId_TeamId_TaskCategoryId",
                table: "TeamReportsDaily");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamReportsDaily",
                table: "TeamReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_TeamReportsDaily_OrganizationId",
                table: "TeamReportsDaily");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SegmentReportsWeekly_DateId_SegmentId_TaskCategoryId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsWeekly_OrganizationId",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SegmentReportsDaily_DateId_SegmentId_TaskCategoryId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_SegmentReportsDaily_OrganizationId",
                table: "SegmentReportsDaily");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProfileReportsWeekly_DateId_ProfileId_TaskCategoryId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsWeekly_OrganizationId",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProfileReportsDaily_DateId_ProfileId_TaskCategoryId",
                table: "ProfileReportsDaily");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily");

            migrationBuilder.DropIndex(
                name: "IX_ProfileReportsDaily_OrganizationId",
                table: "ProfileReportsDaily");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamReportsWeekly",
                table: "TeamReportsWeekly",
                columns: new[] { "OrganizationId", "DateId", "TeamId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamReportsDaily",
                table: "TeamReportsDaily",
                columns: new[] { "OrganizationId", "DateId", "TeamId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly",
                columns: new[] { "OrganizationId", "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily",
                columns: new[] { "OrganizationId", "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly",
                columns: new[] { "OrganizationId", "DateId", "ProfileId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily",
                columns: new[] { "OrganizationId", "DateId", "ProfileId", "TaskCategoryId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamReportsWeekly",
                table: "TeamReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamReportsDaily",
                table: "TeamReportsDaily");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TeamReportsWeekly_DateId_TeamId_TaskCategoryId",
                table: "TeamReportsWeekly",
                columns: new[] { "DateId", "TeamId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamReportsWeekly",
                table: "TeamReportsWeekly",
                columns: new[] { "DateId", "TeamId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TeamReportsDaily_DateId_TeamId_TaskCategoryId",
                table: "TeamReportsDaily",
                columns: new[] { "DateId", "TeamId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamReportsDaily",
                table: "TeamReportsDaily",
                columns: new[] { "DateId", "TeamId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SegmentReportsWeekly_DateId_SegmentId_TaskCategoryId",
                table: "SegmentReportsWeekly",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsWeekly",
                table: "SegmentReportsWeekly",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SegmentReportsDaily_DateId_SegmentId_TaskCategoryId",
                table: "SegmentReportsDaily",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SegmentReportsDaily",
                table: "SegmentReportsDaily",
                columns: new[] { "DateId", "SegmentId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileReportsWeekly_DateId_ProfileId_TaskCategoryId",
                table: "ProfileReportsWeekly",
                columns: new[] { "DateId", "ProfileId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsWeekly",
                table: "ProfileReportsWeekly",
                columns: new[] { "DateId", "ProfileId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileReportsDaily_DateId_ProfileId_TaskCategoryId",
                table: "ProfileReportsDaily",
                columns: new[] { "DateId", "ProfileId", "TaskCategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileReportsDaily",
                table: "ProfileReportsDaily",
                columns: new[] { "DateId", "ProfileId", "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_OrganizationId",
                table: "TeamReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_OrganizationId",
                table: "TeamReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_OrganizationId",
                table: "SegmentReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_OrganizationId",
                table: "SegmentReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_OrganizationId",
                table: "ProfileReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_OrganizationId",
                table: "ProfileReportsDaily",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsDaily_Organizations_OrganizationId",
                table: "ProfileReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileReportsWeekly_Organizations_OrganizationId",
                table: "ProfileReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                table: "SegmentReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                table: "SegmentReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamReportsDaily_Organizations_OrganizationId",
                table: "TeamReportsDaily",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamReportsWeekly_Organizations_OrganizationId",
                table: "TeamReportsWeekly",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
