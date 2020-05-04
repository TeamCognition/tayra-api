using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class RefactorItemQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemReservations");

            migrationBuilder.DropColumn(
                name: "QuantityReservedRemaining",
                table: "ShopItems");

            migrationBuilder.DropColumn(
                name: "IsQuantityLimited",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "ChallengesQuantityRemaining",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GiveawayQuantityRemaining",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShopQuantityRemaining",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChallengesQuantityRemaining",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "GiveawayQuantityRemaining",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ShopQuantityRemaining",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "QuantityReservedRemaining",
                table: "ShopItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQuantityLimited",
                table: "Items",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ItemReservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    QuantityChange = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReservations", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ItemReservations_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemReservations_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemReservations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemReservations_OrganizationId",
                table: "ItemReservations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemReservations_ItemId_OrganizationId",
                table: "ItemReservations",
                columns: new[] { "ItemId", "OrganizationId" });
        }
    }
}
