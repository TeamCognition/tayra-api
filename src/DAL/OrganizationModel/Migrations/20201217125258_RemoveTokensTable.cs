using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class RemoveTokensTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TokenTransactions_Tokens_TokenId",
                table: "TokenTransactions");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_TokenTransactions_TokenId_OrganizationId",
                table: "TokenTransactions");

            migrationBuilder.DropColumn(
                name: "FinalBalance",
                table: "TokenTransactions");

            migrationBuilder.DropColumn(
                name: "TokenId",
                table: "TokenTransactions");

            migrationBuilder.AddColumn<int>(
                name: "TokenType",
                table: "TokenTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenType",
                table: "TokenTransactions");

            migrationBuilder.AddColumn<double>(
                name: "FinalBalance",
                table: "TokenTransactions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "TokenId",
                table: "TokenTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Tokens_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_TokenId_OrganizationId",
                table: "TokenTransactions",
                columns: new[] { "TokenId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_OrganizationId",
                table: "Tokens",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TokenTransactions_Tokens_TokenId",
                table: "TokenTransactions",
                column: "TokenId",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
