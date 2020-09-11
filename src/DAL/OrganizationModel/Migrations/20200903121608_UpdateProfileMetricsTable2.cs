using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class UpdateProfileMetricsTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileMetrics",
                table: "ProfileMetrics");

            migrationBuilder.AlterColumn<int>(
                name: "SegmentId",
                table: "ProfileMetrics",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileMetrics",
                table: "ProfileMetrics",
                columns: new[] { "ProfileId", "Type", "DateId", "OrganizationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileMetrics",
                table: "ProfileMetrics");

            migrationBuilder.AlterColumn<int>(
                name: "SegmentId",
                table: "ProfileMetrics",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileMetrics",
                table: "ProfileMetrics",
                columns: new[] { "ProfileId", "SegmentId", "Type", "DateId", "OrganizationId" });
        }
    }
}
