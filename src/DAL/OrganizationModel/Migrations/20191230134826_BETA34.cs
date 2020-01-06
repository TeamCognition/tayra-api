using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Segments_Key",
                table: "Segments");

            migrationBuilder.DropColumn(
                name: "ArchivedAt",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ArchivedAt",
                table: "ShopItems");

            migrationBuilder.DropColumn(
                name: "ArchivedAt",
                table: "Segments");

            migrationBuilder.AddColumn<long>(
                name: "ArchievedAt",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ArchievedAt",
                table: "ShopItems",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ArchievedAt",
                table: "Segments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchievedAt",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchievedAt" },
                unique: true,
                filter: "[Key] IS NOT NULL AND [ArchievedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key_ArchievedAt",
                table: "Segments",
                columns: new[] { "Key", "ArchievedAt" },
                unique: true,
                filter: "[ArchievedAt] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_SegmentId_Key_ArchievedAt",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Segments_Key_ArchievedAt",
                table: "Segments");

            migrationBuilder.DropColumn(
                name: "ArchievedAt",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ArchievedAt",
                table: "ShopItems");

            migrationBuilder.DropColumn(
                name: "ArchievedAt",
                table: "Segments");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchivedAt",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchivedAt",
                table: "ShopItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchivedAt",
                table: "Segments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchivedAt" },
                unique: true,
                filter: "[Key] IS NOT NULL AND [ArchivedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key",
                table: "Segments",
                column: "Key",
                unique: true);
        }
    }
}
