using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class FixArchivedIndexConstrains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_OrganizationId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Segments_Key_ArchivedAt_OrganizationId",
                table: "Segments");

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "Teams",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "ShopItems",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "Segments",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "Profiles",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "Items",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_OrganizationId",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchivedAt", "OrganizationId" },
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key_ArchivedAt_OrganizationId",
                table: "Segments",
                columns: new[] { "Key", "ArchivedAt", "OrganizationId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_OrganizationId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Segments_Key_ArchivedAt_OrganizationId",
                table: "Segments");

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "Teams",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "ShopItems",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "Segments",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "Profiles",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "ArchivedAt",
                table: "Items",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_OrganizationId",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchivedAt", "OrganizationId" },
                unique: true,
                filter: "[Key] IS NOT NULL AND [ArchivedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key_ArchivedAt_OrganizationId",
                table: "Segments",
                columns: new[] { "Key", "ArchivedAt", "OrganizationId" },
                unique: true,
                filter: "[ArchivedAt] IS NOT NULL");
        }
    }
}
