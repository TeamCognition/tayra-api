﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Catalog.Migrations
{
    public partial class BETA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Identities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(maxLength: 200, nullable: false),
                    Password = table.Column<byte[]>(nullable: false),
                    Salt = table.Column<byte[]>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandingPageContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    EmailAddresss = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandingPageContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<byte[]>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Timezone = table.Column<string>(maxLength: 50, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    ServicePlan = table.Column<string>(type: "char(10)", nullable: false, defaultValueSql: "'standard'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityEmails",
                columns: table => new
                {
                    IdentityId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 200, nullable: false),
                    IsPrimary = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityExternalIds",
                columns: table => new
                {
                    IdentityId = table.Column<int>(nullable: false),
                    IntegrationType = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
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

            migrationBuilder.CreateTable(
                name: "TenantIdentities",
                columns: table => new
                {
                    TenantId = table.Column<byte[]>(nullable: false),
                    IdentityId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantIdentities", x => new { x.TenantId, x.IdentityId });
                    table.ForeignKey(
                        name: "FK_TenantIdentities_Identities_IdentityId",
                        column: x => x.IdentityId,
                        principalTable: "Identities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantIdentities_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_TenantIdentities_IdentityId",
                table: "TenantIdentities",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityEmails");

            migrationBuilder.DropTable(
                name: "IdentityExternalIds");

            migrationBuilder.DropTable(
                name: "LandingPageContacts");

            migrationBuilder.DropTable(
                name: "TenantIdentities");

            migrationBuilder.DropTable(
                name: "Identities");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}