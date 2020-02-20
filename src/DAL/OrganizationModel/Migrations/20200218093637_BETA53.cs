using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA53 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "ChallengeCommits",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LogDevices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDevices", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_LogDevices_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogDevices_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogDevices_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogSettings",
                columns: table => new
                {
                    LogDeviceId = table.Column<int>(nullable: false),
                    LogEvent = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSettings", x => new { x.LogDeviceId, x.LogEvent, x.OrganizationId });
                    table.UniqueConstraint("AK_LogSettings_LogDeviceId_LogEvent", x => new { x.LogDeviceId, x.LogEvent });
                    table.ForeignKey(
                        name: "FK_LogSettings_LogDevices_LogDeviceId",
                        column: x => x.LogDeviceId,
                        principalTable: "LogDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogSettings_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogSettings_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogDevices_OrganizationId",
                table: "LogDevices",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_LogDevices_ProfileId",
                table: "LogDevices",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_LogSettings_OrganizationId",
                table: "LogSettings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_LogSettings_ProfileId",
                table: "LogSettings",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogSettings");

            migrationBuilder.DropTable(
                name: "LogDevices");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "ChallengeCommits");

            migrationBuilder.CreateTable(
                name: "StatTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    IntegrationType = table.Column<int>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatTypes", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_StatTypes_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatTypes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatTypes_OrganizationId",
                table: "StatTypes",
                column: "OrganizationId");
        }
    }
}
