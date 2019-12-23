using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Catalog.Migrations
{
    public partial class BETA25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityExternalIds");

            migrationBuilder.DropIndex(
                name: "IX_IdentityEmails_Email",
                table: "IdentityEmails");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Identities",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Identities",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityEmails_Email_DeletedAt",
                table: "IdentityEmails",
                columns: new[] { "Email", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityEmails_Email_DeletedAt",
                table: "IdentityEmails");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Identities",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Identities",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityExternalIds",
                columns: table => new
                {
                    IdentityId = table.Column<int>(nullable: false),
                    IntegrationType = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    ExternalId = table.Column<string>(maxLength: 100, nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityExternalIds", x => new { x.IdentityId, x.IntegrationType });
                    table.ForeignKey(
                        name: "FK_IdentityExternalIds_Identities_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityEmails_Email",
                table: "IdentityEmails",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityExternalIds_ExternalId",
                table: "IdentityExternalIds",
                column: "ExternalId",
                unique: true,
                filter: "[ExternalId] IS NOT NULL");
        }
    }
}
