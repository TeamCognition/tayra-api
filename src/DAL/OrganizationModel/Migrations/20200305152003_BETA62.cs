using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA62 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AssigneeProfileId",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AssigneeExternalId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalProjectId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemsBoughtTotal",
                table: "SegmentReportsDaily",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TaskSyncs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ExternalProjectId = table.Column<string>(nullable: true),
                    IntegrationType = table.Column<int>(nullable: false),
                    DateId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSyncs", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_TaskSyncs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskSyncs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskSyncs_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_OrganizationId",
                table: "TaskSyncs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_SegmentId",
                table: "TaskSyncs",
                column: "SegmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskSyncs");

            migrationBuilder.DropColumn(
                name: "AssigneeExternalId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ExternalProjectId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ItemsBoughtTotal",
                table: "SegmentReportsDaily");

            migrationBuilder.AlterColumn<int>(
                name: "AssigneeProfileId",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
