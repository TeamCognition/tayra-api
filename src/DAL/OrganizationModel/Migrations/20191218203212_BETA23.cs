using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Profiles_CreatedProfileId",
                table: "Invitations");

            migrationBuilder.RenameColumn(
                name: "CreatedProfileId",
                table: "Invitations",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_CreatedProfileId",
                table: "Invitations",
                newName: "IX_Invitations_TeamId");

            migrationBuilder.AddColumn<int>(
                name: "AutoTimeSpentInMinutes",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Profiles",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Profiles",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Invitations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ProjectId",
                table: "Invitations",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Projects_ProjectId",
                table: "Invitations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Teams_TeamId",
                table: "Invitations",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Projects_ProjectId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Teams_TeamId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_ProjectId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "AutoTimeSpentInMinutes",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Invitations");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Invitations",
                newName: "CreatedProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_TeamId",
                table: "Invitations",
                newName: "IX_Invitations_CreatedProfileId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Profiles",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Profiles",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Profiles_CreatedProfileId",
                table: "Invitations",
                column: "CreatedProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
