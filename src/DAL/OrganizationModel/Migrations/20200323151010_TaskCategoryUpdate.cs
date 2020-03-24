using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class TaskCategoryUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskCategories_Organizations_OrganizationId",
                table: "TaskCategories");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_TaskCategories_Id",
            //    table: "TaskCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskCategories",
                table: "TaskCategories");

            migrationBuilder.DropIndex(
                name: "IX_TaskCategories_OrganizationId",
                table: "TaskCategories");

            migrationBuilder.DropIndex(
                name: "IX_TaskCategories_Name_OrganizationId",
                table: "TaskCategories");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "TaskCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskCategories",
                table: "TaskCategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_Name",
                table: "TaskCategories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskCategories",
                table: "TaskCategories");

            migrationBuilder.DropIndex(
                name: "IX_TaskCategories_Name",
                table: "TaskCategories");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "TaskCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TaskCategories_Id",
                table: "TaskCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskCategories",
                table: "TaskCategories",
                columns: new[] { "Id", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_OrganizationId",
                table: "TaskCategories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_Name_OrganizationId",
                table: "TaskCategories",
                columns: new[] { "Name", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskCategories_Organizations_OrganizationId",
                table: "TaskCategories",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
