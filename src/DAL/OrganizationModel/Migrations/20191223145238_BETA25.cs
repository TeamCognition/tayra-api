using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Integrations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_ProjectId",
                table: "Integrations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_ProfileId_ProjectId",
                table: "Integrations",
                columns: new[] { "ProfileId", "ProjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Integrations_Profiles_ProfileId",
                table: "Integrations",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Integrations_Projects_ProjectId",
                table: "Integrations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Integrations_Profiles_ProfileId",
                table: "Integrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Integrations_Projects_ProjectId",
                table: "Integrations");

            migrationBuilder.DropIndex(
                name: "IX_Integrations_ProjectId",
                table: "Integrations");

            migrationBuilder.DropIndex(
                name: "IX_Integrations_ProfileId_ProjectId",
                table: "Integrations");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Integrations");
        }
    }
}
