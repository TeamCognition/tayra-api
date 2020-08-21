using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Catalog.Migrations
{
    public partial class AddTenantIntegrationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantIntegrations",
                columns: table => new
                {
                    TenantId = table.Column<byte[]>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    InstallationId = table.Column<string>(maxLength: 125, nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantIntegrations", x => new { x.TenantId, x.Type, x.SegmentId });
                    table.ForeignKey(
                        name: "FK_TenantIntegrations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantIntegrations_InstallationId",
                table: "TenantIntegrations",
                column: "InstallationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantIntegrations");
        }
    }
}
