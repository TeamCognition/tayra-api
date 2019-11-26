using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class MVP2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Identities_IdentityId",
                table: "Profiles");

            migrationBuilder.DropTable(
                name: "IdentityEmails");

            migrationBuilder.DropTable(
                name: "IdentityExternalIds");

            migrationBuilder.DropTable(
                name: "Identities");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_IdentityId",
                table: "Profiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Identities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    Password = table.Column<byte[]>(nullable: false),
                    Salt = table.Column<byte[]>(nullable: false),
                    Username = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityEmails",
                columns: table => new
                {
                    IdentityId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 200, nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsPrimary = table.Column<bool>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityEmails", x => new { x.IdentityId, x.Email });
                    table.ForeignKey(
                        name: "FK_IdentityEmails_Identities_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_IdentityId",
                table: "Profiles",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Identities_Username",
                table: "Identities",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityEmails_Email",
                table: "IdentityEmails",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityEmails_IdentityId_IsPrimary",
                table: "IdentityEmails",
                columns: new[] { "IdentityId", "IsPrimary" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityExternalIds_ExternalId",
                table: "IdentityExternalIds",
                column: "ExternalId",
                unique: true,
                filter: "[ExternalId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Identities_IdentityId",
                table: "Profiles",
                column: "IdentityId",
                principalTable: "Identities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
