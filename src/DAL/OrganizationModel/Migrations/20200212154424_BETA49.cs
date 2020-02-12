using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProfileAssignments_SegmentId_TeamId_ProfileId",
                table: "ProfileAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileAssignments",
                table: "ProfileAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "ProfileAssignments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProfileAssignments",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileAssignments_Id",
                table: "ProfileAssignments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileAssignments",
                table: "ProfileAssignments",
                columns: new[] { "Id", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProfileAssignments_Id",
                table: "ProfileAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileAssignments",
                table: "ProfileAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId",
                table: "ProfileAssignments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProfileAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "ProfileAssignments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProfileAssignments_SegmentId_TeamId_ProfileId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileAssignments",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId", "OrganizationId" });
        }
    }
}
