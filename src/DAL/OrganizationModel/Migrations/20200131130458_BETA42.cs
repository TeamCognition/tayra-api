using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityReserved",
                table: "ShopItems",
                newName: "QuantityReservedRemaining");

            migrationBuilder.RenameColumn(
                name: "QuantityReserved",
                table: "ChallengeRewards",
                newName: "Quantity");

            migrationBuilder.AlterColumn<int>(
                name: "SegmentId",
                table: "Challenges",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "ChallengeSegments",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeSegments", x => new { x.ChallengeId, x.SegmentId, x.OrganizationId });
                    table.UniqueConstraint("AK_ChallengeSegments_ChallengeId_SegmentId", x => new { x.ChallengeId, x.SegmentId });
                    table.ForeignKey(
                        name: "FK_ChallengeSegments_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeSegments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeSegments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeSegments_OrganizationId",
                table: "ChallengeSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeSegments_SegmentId",
                table: "ChallengeSegments",
                column: "SegmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeSegments");

            migrationBuilder.RenameColumn(
                name: "QuantityReservedRemaining",
                table: "ShopItems",
                newName: "QuantityReserved");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ChallengeRewards",
                newName: "QuantityReserved");

            migrationBuilder.AlterColumn<int>(
                name: "SegmentId",
                table: "Challenges",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
