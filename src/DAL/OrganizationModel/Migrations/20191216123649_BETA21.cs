using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Profiles_ProfileId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_ProfileId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Invitations");

            migrationBuilder.AddColumn<string>(
                name: "JobPosition",
                table: "Profiles",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedProfileId",
                table: "Invitations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Invitations",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Invitations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Invitations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Invitations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_CreatedProfileId",
                table: "Invitations",
                column: "CreatedProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Profiles_CreatedProfileId",
                table: "Invitations",
                column: "CreatedProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Profiles_CreatedProfileId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_CreatedProfileId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "JobPosition",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CreatedProfileId",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Invitations");

            migrationBuilder.AddColumn<string>(
                name: "EmailId",
                table: "Invitations",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Invitations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ProfileId",
                table: "Invitations",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Profiles_ProfileId",
                table: "Invitations",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
