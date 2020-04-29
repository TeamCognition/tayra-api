using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class RenameToPraise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileOneUps");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedChange",
                table: "TeamReportsWeekly",
                newName: "PraisesReceivedChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedAverage",
                table: "TeamReportsWeekly",
                newName: "PraisesReceivedAverage");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenChange",
                table: "TeamReportsWeekly",
                newName: "PraisesGivenChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenAverage",
                table: "TeamReportsWeekly",
                newName: "PraisesGivenAverage");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedTotal",
                table: "TeamReportsDaily",
                newName: "PraisesReceivedTotal");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedChange",
                table: "TeamReportsDaily",
                newName: "PraisesReceivedChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenTotal",
                table: "TeamReportsDaily",
                newName: "PraisesGivenTotal");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenChange",
                table: "TeamReportsDaily",
                newName: "PraisesGivenChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedChange",
                table: "SegmentReportsWeekly",
                newName: "PraisesReceivedChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedAverage",
                table: "SegmentReportsWeekly",
                newName: "PraisesReceivedAverage");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenChange",
                table: "SegmentReportsWeekly",
                newName: "PraisesGivenChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenAverage",
                table: "SegmentReportsWeekly",
                newName: "PraisesGivenAverage");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedTotal",
                table: "SegmentReportsDaily",
                newName: "PraisesReceivedTotal");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedChange",
                table: "SegmentReportsDaily",
                newName: "PraisesReceivedChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenTotal",
                table: "SegmentReportsDaily",
                newName: "PraisesGivenTotal");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenChange",
                table: "SegmentReportsDaily",
                newName: "PraisesGivenChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedTotalAverage",
                table: "ProfileReportsWeekly",
                newName: "PraisesReceivedTotalAverage");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedChange",
                table: "ProfileReportsWeekly",
                newName: "PraisesReceivedChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenTotalAverage",
                table: "ProfileReportsWeekly",
                newName: "PraisesGivenTotalAverage");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenChange",
                table: "ProfileReportsWeekly",
                newName: "PraisesGivenChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedTotal",
                table: "ProfileReportsDaily",
                newName: "PraisesReceivedTotal");

            migrationBuilder.RenameColumn(
                name: "OneUpsReceivedChange",
                table: "ProfileReportsDaily",
                newName: "PraisesReceivedChange");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenTotal",
                table: "ProfileReportsDaily",
                newName: "PraisesGivenTotal");

            migrationBuilder.RenameColumn(
                name: "OneUpsGivenChange",
                table: "ProfileReportsDaily",
                newName: "PraisesGivenChange");

            migrationBuilder.AddColumn<int>(
                name: "DateId",
                table: "TokenTransactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedDateId",
                table: "ShopPurchases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedDateId",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DateId",
                table: "ItemGifts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DateId",
                table: "ItemDisenchants",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProfilePraises",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    PraiserProfileId = table.Column<int>(nullable: false),
                    DateId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 140, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePraises", x => new { x.DateId, x.ProfileId, x.PraiserProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfilePraises_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfilePraises_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePraises_OrganizationId",
                table: "ProfilePraises",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePraises_ProfileId_OrganizationId",
                table: "ProfilePraises",
                columns: new[] { "ProfileId", "OrganizationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilePraises");

            migrationBuilder.DropColumn(
                name: "DateId",
                table: "TokenTransactions");

            migrationBuilder.DropColumn(
                name: "LastModifiedDateId",
                table: "ShopPurchases");

            migrationBuilder.DropColumn(
                name: "CreatedDateId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DateId",
                table: "ItemGifts");

            migrationBuilder.DropColumn(
                name: "DateId",
                table: "ItemDisenchants");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedChange",
                table: "TeamReportsWeekly",
                newName: "OneUpsReceivedChange");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedAverage",
                table: "TeamReportsWeekly",
                newName: "OneUpsReceivedAverage");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenChange",
                table: "TeamReportsWeekly",
                newName: "OneUpsGivenChange");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenAverage",
                table: "TeamReportsWeekly",
                newName: "OneUpsGivenAverage");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedTotal",
                table: "TeamReportsDaily",
                newName: "OneUpsReceivedTotal");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedChange",
                table: "TeamReportsDaily",
                newName: "OneUpsReceivedChange");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenTotal",
                table: "TeamReportsDaily",
                newName: "OneUpsGivenTotal");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenChange",
                table: "TeamReportsDaily",
                newName: "OneUpsGivenChange");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedChange",
                table: "SegmentReportsWeekly",
                newName: "OneUpsReceivedChange");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedAverage",
                table: "SegmentReportsWeekly",
                newName: "OneUpsReceivedAverage");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenChange",
                table: "SegmentReportsWeekly",
                newName: "OneUpsGivenChange");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenAverage",
                table: "SegmentReportsWeekly",
                newName: "OneUpsGivenAverage");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedTotal",
                table: "SegmentReportsDaily",
                newName: "OneUpsReceivedTotal");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedChange",
                table: "SegmentReportsDaily",
                newName: "OneUpsReceivedChange");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenTotal",
                table: "SegmentReportsDaily",
                newName: "OneUpsGivenTotal");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenChange",
                table: "SegmentReportsDaily",
                newName: "OneUpsGivenChange");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedTotalAverage",
                table: "ProfileReportsWeekly",
                newName: "OneUpsReceivedTotalAverage");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedChange",
                table: "ProfileReportsWeekly",
                newName: "OneUpsReceivedChange");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenTotalAverage",
                table: "ProfileReportsWeekly",
                newName: "OneUpsGivenTotalAverage");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenChange",
                table: "ProfileReportsWeekly",
                newName: "OneUpsGivenChange");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedTotal",
                table: "ProfileReportsDaily",
                newName: "OneUpsReceivedTotal");

            migrationBuilder.RenameColumn(
                name: "PraisesReceivedChange",
                table: "ProfileReportsDaily",
                newName: "OneUpsReceivedChange");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenTotal",
                table: "ProfileReportsDaily",
                newName: "OneUpsGivenTotal");

            migrationBuilder.RenameColumn(
                name: "PraisesGivenChange",
                table: "ProfileReportsDaily",
                newName: "OneUpsGivenChange");

            migrationBuilder.CreateTable(
                name: "ProfileOneUps",
                columns: table => new
                {
                    DateId = table.Column<int>(nullable: false),
                    UppedProfileId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileOneUps", x => new { x.DateId, x.UppedProfileId, x.CreatedBy, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileOneUps_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileOneUps_Profiles_UppedProfileId",
                        column: x => x.UppedProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileOneUps_OrganizationId",
                table: "ProfileOneUps",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileOneUps_UppedProfileId_OrganizationId",
                table: "ProfileOneUps",
                columns: new[] { "UppedProfileId", "OrganizationId" });
        }
    }
}
