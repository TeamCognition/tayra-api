using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class TenantOnboarding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_TenantId",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "isCreateProfileOnboarding",
                table: "Profiles",
                newName: "IsProfileOnboardingCompleted");

            migrationBuilder.RenameColumn(
                name: "IsInviteUsersOnboarding",
                table: "LocalTenants",
                newName: "IsSegmentOnboardingCompleted");

            migrationBuilder.RenameColumn(
                name: "IsCreateSegmentOnboarding",
                table: "LocalTenants",
                newName: "IsMembersOnboardingCompleted");

            migrationBuilder.RenameColumn(
                name: "IsAddSourcesOnboarding",
                table: "LocalTenants",
                newName: "IsAppsOnboardingCompleted");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Teams",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_TenantId",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchivedAt", "TenantId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_TenantId",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "IsProfileOnboardingCompleted",
                table: "Profiles",
                newName: "isCreateProfileOnboarding");

            migrationBuilder.RenameColumn(
                name: "IsSegmentOnboardingCompleted",
                table: "LocalTenants",
                newName: "IsInviteUsersOnboarding");

            migrationBuilder.RenameColumn(
                name: "IsMembersOnboardingCompleted",
                table: "LocalTenants",
                newName: "IsCreateSegmentOnboarding");

            migrationBuilder.RenameColumn(
                name: "IsAppsOnboardingCompleted",
                table: "LocalTenants",
                newName: "IsAddSourcesOnboarding");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Teams",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_TenantId",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchivedAt", "TenantId" },
                unique: true,
                filter: "[Key] IS NOT NULL");
        }
    }
}
