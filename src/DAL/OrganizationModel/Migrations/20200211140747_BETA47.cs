using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemsBoughtChange",
                table: "SegmentReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsCreatedChange",
                table: "SegmentReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsDisenchantedChange",
                table: "SegmentReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsGiftedChange",
                table: "SegmentReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsBoughtChange",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsCreatedChange",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsDisenchantedChange",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsGiftedChange",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InventoryCountTotal",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "InventoryValueTotal",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ItemsBoughtChange",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsCreatedChange",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsDisenchantedChange",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsGiftedChange",
                table: "ProfileReportsWeekly",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InventoryCountTotal",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "InventoryValueTotal",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ItemsBoughtChange",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsBoughtTotal",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsCreatedChange",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsCreatedTotal",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsDisenchantedChange",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsDisenchantedTotal",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsGiftedChange",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemsGiftedTotal",
                table: "ProfileReportsDaily",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemsBoughtChange",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropColumn(
                name: "ItemsCreatedChange",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropColumn(
                name: "ItemsDisenchantedChange",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropColumn(
                name: "ItemsGiftedChange",
                table: "SegmentReportsWeekly");

            migrationBuilder.DropColumn(
                name: "ItemsBoughtChange",
                table: "SegmentReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsCreatedChange",
                table: "SegmentReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsDisenchantedChange",
                table: "SegmentReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsGiftedChange",
                table: "SegmentReportsDaily");

            migrationBuilder.DropColumn(
                name: "InventoryCountTotal",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "InventoryValueTotal",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "ItemsBoughtChange",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "ItemsCreatedChange",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "ItemsDisenchantedChange",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "ItemsGiftedChange",
                table: "ProfileReportsWeekly");

            migrationBuilder.DropColumn(
                name: "InventoryCountTotal",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "InventoryValueTotal",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsBoughtChange",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsBoughtTotal",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsCreatedChange",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsCreatedTotal",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsDisenchantedChange",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsDisenchantedTotal",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsGiftedChange",
                table: "ProfileReportsDaily");

            migrationBuilder.DropColumn(
                name: "ItemsGiftedTotal",
                table: "ProfileReportsDaily");
        }
    }
}
