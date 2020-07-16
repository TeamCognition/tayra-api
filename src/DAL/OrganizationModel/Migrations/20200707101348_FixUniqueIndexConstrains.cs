using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class FixUniqueIndexConstrains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchievedAt_OrganizationId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_SegmentId_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_ShopItems_ItemId_OrganizationId",
                table: "ShopItems");

            migrationBuilder.DropIndex(
                name: "IX_Segments_Key_ArchievedAt_OrganizationId",
                table: "Segments");

            migrationBuilder.DropIndex(
                name: "IX_SegmentAreas_Name_OrganizationId",
                table: "SegmentAreas");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_Username_OrganizationId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_CompetitionId_TeamId_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_TeamId_OrganizationId",
                table: "Competitors");

            migrationBuilder.RenameColumn(
                name: "ArchievedAt",
                table: "Teams",
                newName: "ArchivedAt");

            migrationBuilder.RenameColumn(
                name: "ArchievedAt",
                table: "ShopItems",
                newName: "ArchivedAt");

            migrationBuilder.RenameColumn(
                name: "ArchievedAt",
                table: "Segments",
                newName: "ArchivedAt");

            migrationBuilder.RenameColumn(
                name: "ArchievedAt",
                table: "Profiles",
                newName: "ArchivedAt");

            migrationBuilder.RenameColumn(
                name: "ArchievedAt",
                table: "Items",
                newName: "ArchivedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_OrganizationId",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchivedAt", "OrganizationId" },
                unique: true,
                filter: "[Key] IS NOT NULL AND [ArchivedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_SegmentId_OrganizationId",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType", "SegmentId", "OrganizationId" },
                unique: true,
                filter: "[ExternalId] IS NOT NULL AND [SegmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ItemId_OrganizationId",
                table: "ShopItems",
                columns: new[] { "ItemId", "OrganizationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key_ArchivedAt_OrganizationId",
                table: "Segments",
                columns: new[] { "Key", "ArchivedAt", "OrganizationId" },
                unique: true,
                filter: "[ArchivedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentAreas_Name_OrganizationId",
                table: "SegmentAreas",
                columns: new[] { "Name", "OrganizationId" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Username_OrganizationId",
                table: "Profiles",
                columns: new[] { "Username", "OrganizationId" },
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId", "OrganizationId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_OrganizationId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId", "OrganizationId" },
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_TeamId_OrganizationId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "TeamId", "OrganizationId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_TeamId_OrganizationId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId", "TeamId", "OrganizationId" },
                unique: true,
                filter: "[ProfileId] IS NOT NULL AND [TeamId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_OrganizationId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_SegmentId_OrganizationId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_ShopItems_ItemId_OrganizationId",
                table: "ShopItems");

            migrationBuilder.DropIndex(
                name: "IX_Segments_Key_ArchivedAt_OrganizationId",
                table: "Segments");

            migrationBuilder.DropIndex(
                name: "IX_SegmentAreas_Name_OrganizationId",
                table: "SegmentAreas");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_Username_OrganizationId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_CompetitionId_TeamId_OrganizationId",
                table: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_TeamId_OrganizationId",
                table: "Competitors");

            migrationBuilder.RenameColumn(
                name: "ArchivedAt",
                table: "Teams",
                newName: "ArchievedAt");

            migrationBuilder.RenameColumn(
                name: "ArchivedAt",
                table: "ShopItems",
                newName: "ArchievedAt");

            migrationBuilder.RenameColumn(
                name: "ArchivedAt",
                table: "Segments",
                newName: "ArchievedAt");

            migrationBuilder.RenameColumn(
                name: "ArchivedAt",
                table: "Profiles",
                newName: "ArchievedAt");

            migrationBuilder.RenameColumn(
                name: "ArchivedAt",
                table: "Items",
                newName: "ArchievedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchievedAt_OrganizationId",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchievedAt", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_SegmentId_OrganizationId",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType", "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ItemId_OrganizationId",
                table: "ShopItems",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key_ArchievedAt_OrganizationId",
                table: "Segments",
                columns: new[] { "Key", "ArchievedAt", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentAreas_Name_OrganizationId",
                table: "SegmentAreas",
                columns: new[] { "Name", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Username_OrganizationId",
                table: "Profiles",
                columns: new[] { "Username", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_OrganizationId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_TeamId_OrganizationId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_TeamId_OrganizationId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId", "TeamId", "OrganizationId" });
        }
    }
}
