using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class UpdateLogMeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Logs",
                newName: "ExternalUrl");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Logs",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternalUrl",
                table: "Logs",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Logs",
                newName: "Message");
        }
    }
}
