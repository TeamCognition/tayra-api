using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isCreateSegmentOnboarding = table.Column<bool>(type: "bit", nullable: false),
                    isAddSourcesOnboarding = table.Column<bool>(type: "bit", nullable: false),
                    isInviteUsersOnboarding = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Purpose = table.Column<int>(type: "int", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Filesize = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blobs", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Blobs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blobs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntityChangeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityState = table.Column<int>(type: "int", nullable: false),
                    AuditType = table.Column<byte>(type: "tinyint", nullable: false),
                    ChangedValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityChangeLogs", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_EntityChangeLogs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityChangeLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    IsActivable = table.Column<bool>(type: "bit", nullable: false),
                    IsDisenchantable = table.Column<bool>(type: "bit", nullable: false),
                    IsGiftable = table.Column<bool>(type: "bit", nullable: false),
                    ShopQuantityRemaining = table.Column<int>(type: "int", nullable: true),
                    QuestsQuantityRemaining = table.Column<int>(type: "int", nullable: true),
                    GiveawayQuantityRemaining = table.Column<int>(type: "int", nullable: true),
                    Rarity = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedDateId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArchivedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Items_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoginLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLogs", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_LoginLogs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Event = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Logs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsAnalyticsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    JobPosition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BornOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssistantSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isCreateProfileOnboarding = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArchivedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Profiles_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SegmentAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentAreas", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_SegmentAreas_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SegmentAreas_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DataStore = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DataWarehouse = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsReportingUnlocked = table.Column<bool>(type: "bit", nullable: false),
                    AssistantSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArchivedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Segments_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Shops_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shops_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SupplyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "WebhookEventLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegrationType = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookEventLogs", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_WebhookEventLogs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookEventLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountPrice = table.Column<float>(type: "real", nullable: true),
                    DiscountEndsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FeaturedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisabledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsGlobal = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArchivedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ShopItems_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopItems_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimBundles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RewardClaimedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBundles", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ClaimBundles_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimBundles_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimBundles_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GitCommits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SHA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Additions = table.Column<int>(type: "int", nullable: false),
                    Deletions = table.Column<int>(type: "int", nullable: false),
                    AuthorProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitCommits", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_GitCommits_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GitCommits_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GitCommits_Profiles_AuthorProfileId",
                        column: x => x.AuthorProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemDisenchants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDisenchants", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ItemDisenchants_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDisenchants_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemDisenchants_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemDisenchants_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemGifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGifts", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ItemGifts_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemGifts_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemGifts_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemGifts_Profiles_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemGifts_Profiles_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogDevices_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileInventoryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    AcquireMethod = table.Column<int>(type: "int", nullable: false),
                    AcquireDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimRequired = table.Column<bool>(type: "bit", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileInventoryItems", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ProfileInventoryItems_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileInventoryItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileInventoryItems_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileInventoryItems_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileLogs",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Event = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileLogs", x => new { x.ProfileId, x.LogId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileLogs_Logs_LogId",
                        column: x => x.LogId,
                        principalTable: "Logs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileLogs_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePraises",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PraiserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePraises", x => new { x.DateId, x.ProfileId, x.PraiserProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfilePraises_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfilePraises_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PullRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalNumber = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    ExternalUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MergedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommitsCount = table.Column<int>(type: "int", nullable: false),
                    ReviewCommentsCount = table.Column<int>(type: "int", nullable: false),
                    ExternalAuthorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PullRequests", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_PullRequests_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequests_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequests_Profiles_AuthorProfileId",
                        column: x => x.AuthorProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    ConcludedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPoints", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ActionPoints_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionPoints_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPoints_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPoints_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionPointSettings",
                columns: table => new
                {
                    Type = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotifyByEmail = table.Column<bool>(type: "bit", nullable: false),
                    NotifyByPush = table.Column<bool>(type: "bit", nullable: false),
                    NotifyByNotification = table.Column<bool>(type: "bit", nullable: false),
                    MuteUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPointSettings", x => new { x.Type, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ActionPointSettings_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionPointSettings_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Integrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integrations", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Integrations_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Integrations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Integrations_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Integrations_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileExternalIds",
                columns: table => new
                {
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntegrationType = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileExternalIds", x => new { x.ExternalId, x.IntegrationType, x.SegmentId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileExternalIds_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileExternalIds_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileMetrics",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileMetrics", x => new { x.ProfileId, x.Type, x.DateId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileMetrics_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileMetrics_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileMetrics_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    ProfileRole = table.Column<int>(type: "int", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityTotal = table.Column<int>(type: "int", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedTotal = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentTotal = table.Column<float>(type: "real", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreTotal = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenTotal = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedTotal = table.Column<int>(type: "int", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverTotal = table.Column<int>(type: "int", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorTotal = table.Column<float>(type: "real", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionTotal = table.Column<float>(type: "real", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesTotal = table.Column<int>(type: "int", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeTotal = table.Column<int>(type: "int", nullable: false),
                    InventoryCountTotal = table.Column<int>(type: "int", nullable: false),
                    InventoryValueTotal = table.Column<float>(type: "real", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsBoughtTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedTotal = table.Column<int>(type: "int", nullable: false),
                    QuestsCompletedChange = table.Column<int>(type: "int", nullable: false),
                    QuestsCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    ActivityChartJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileReportsDaily", x => new { x.DateId, x.ProfileId, x.SegmentId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileReportsDaily_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileReportsDaily_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileReportsDaily_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileReportsWeekly",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    ProfileRole = table.Column<int>(type: "int", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityTotalAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedTotalAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentTotalAverage = table.Column<float>(type: "real", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreTotalAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenTotalAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedTotalAverage = table.Column<float>(type: "real", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsTotalAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedTotalAverage = table.Column<float>(type: "real", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverTotalAverage = table.Column<float>(type: "real", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorTotalAverage = table.Column<float>(type: "real", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionTotalAverage = table.Column<float>(type: "real", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesTotalAverage = table.Column<float>(type: "real", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesTotalAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeAverage = table.Column<int>(type: "int", nullable: false),
                    RangeChange = table.Column<int>(type: "int", nullable: false),
                    RangeTotalAverage = table.Column<int>(type: "int", nullable: false),
                    InventoryCountTotal = table.Column<int>(type: "int", nullable: false),
                    InventoryValueTotal = table.Column<float>(type: "real", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedChange = table.Column<int>(type: "int", nullable: false),
                    OImpactAverage = table.Column<float>(type: "real", nullable: false),
                    OImpactTotalAverage = table.Column<float>(type: "real", nullable: false),
                    DImpactAverage = table.Column<float>(type: "real", nullable: false),
                    DImpactTotalAverage = table.Column<float>(type: "real", nullable: false),
                    PowerAverage = table.Column<float>(type: "real", nullable: false),
                    PowerTotalAverage = table.Column<float>(type: "real", nullable: false),
                    SpeedAverage = table.Column<float>(type: "real", nullable: false),
                    SpeedTotalAverage = table.Column<float>(type: "real", nullable: false),
                    Heat = table.Column<float>(type: "real", nullable: false),
                    HeatIndex = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileReportsWeekly", x => new { x.DateId, x.ProfileId, x.SegmentId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileReportsWeekly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileReportsWeekly_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileReportsWeekly_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletionsLimit = table.Column<int>(type: "int", nullable: true),
                    CompletionsRemaining = table.Column<int>(type: "int", nullable: true),
                    IsEasterEgg = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ActiveUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RewardValue = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Quests_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quests_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quests_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SegmentMetrics",
                columns: table => new
                {
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentMetrics", x => new { x.SegmentId, x.Type, x.DateId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_SegmentMetrics_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SegmentMetrics_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SegmentReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityTotal = table.Column<int>(type: "int", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedTotal = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentTotal = table.Column<float>(type: "real", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreTotal = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenTotal = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedTotal = table.Column<int>(type: "int", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverTotal = table.Column<int>(type: "int", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorTotal = table.Column<float>(type: "real", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionTotal = table.Column<float>(type: "real", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesTotal = table.Column<int>(type: "int", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsBoughtTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedChange = table.Column<int>(type: "int", nullable: false),
                    MembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    NonMembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentReportsDaily", x => new { x.DateId, x.SegmentId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SegmentReportsDaily_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SegmentReportsWeekly",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentAverage = table.Column<float>(type: "real", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedAverage = table.Column<float>(type: "real", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedAverage = table.Column<float>(type: "real", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverAverage = table.Column<float>(type: "real", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorAverage = table.Column<float>(type: "real", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionAverage = table.Column<float>(type: "real", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesAverage = table.Column<float>(type: "real", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeAverage = table.Column<int>(type: "int", nullable: false),
                    RangeChange = table.Column<int>(type: "int", nullable: false),
                    RangeAverage = table.Column<int>(type: "int", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedChange = table.Column<int>(type: "int", nullable: false),
                    OImpactAverage = table.Column<float>(type: "real", nullable: false),
                    OImpactAverageTotal = table.Column<float>(type: "real", nullable: false),
                    DImpactAverage = table.Column<float>(type: "real", nullable: false),
                    DImpactAverageTotal = table.Column<float>(type: "real", nullable: false),
                    PowerAverage = table.Column<float>(type: "real", nullable: false),
                    PowerAverageTotal = table.Column<float>(type: "real", nullable: false),
                    SpeedAverage = table.Column<float>(type: "real", nullable: false),
                    SpeedAverageTotal = table.Column<float>(type: "real", nullable: false),
                    HeatAverageTotal = table.Column<float>(type: "real", nullable: false),
                    MembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    NonMembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentReportsWeekly", x => new { x.DateId, x.SegmentId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SegmentReportsWeekly_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsDiscounted = table.Column<bool>(type: "bit", nullable: false),
                    GiftFor = table.Column<int>(type: "int", nullable: true),
                    LastModifiedDateId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    PriceDiscountedFor = table.Column<float>(type: "real", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopPurchases", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ShopPurchases_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopPurchases_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopPurchases_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopPurchases_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopPurchases_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskSyncs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ExternalProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegrationType = table.Column<int>(type: "int", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskSyncs_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AssistantSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArchivedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Teams_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopLogs",
                columns: table => new
                {
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Event = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopLogs", x => new { x.ShopId, x.LogId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ShopLogs_Logs_LogId",
                        column: x => x.LogId,
                        principalTable: "Logs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopLogs_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TokenTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    FinalBalance = table.Column<double>(type: "float", nullable: false),
                    TxnHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    ClaimRequired = table.Column<bool>(type: "bit", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenTransactions", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_TokenTransactions_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenTransactions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TokenTransactions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TokenTransactions_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopItemSegments",
                columns: table => new
                {
                    ShopItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    DiscountPrice = table.Column<float>(type: "real", nullable: true),
                    DiscountEndsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HiddenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemSegments", x => new { x.ShopItemId, x.SegmentId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ShopItemSegments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopItemSegments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopItemSegments_ShopItems_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogSettings",
                columns: table => new
                {
                    LogDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogEvent = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSettings", x => new { x.LogDeviceId, x.LogEvent, x.OrganizationId });
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogSettings_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimBundleItems",
                columns: table => new
                {
                    ClaimBundleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileInventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBundleItems", x => new { x.ClaimBundleId, x.ProfileInventoryItemId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ClaimBundleItems_ClaimBundles_ClaimBundleId",
                        column: x => x.ClaimBundleId,
                        principalTable: "ClaimBundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimBundleItems_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimBundleItems_ProfileInventoryItems_ProfileInventoryItemId",
                        column: x => x.ProfileInventoryItemId,
                        principalTable: "ProfileInventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PullRequestReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommitId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PullRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewerProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PullRequestReviews", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_PullRequestReviews_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequestReviews_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviews_Profiles_ReviewerProfileId",
                        column: x => x.ReviewerProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviews_PullRequests_PullRequestId",
                        column: x => x.PullRequestId,
                        principalTable: "PullRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IntegrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationFields", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_IntegrationFields_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationFields_Integrations_IntegrationId",
                        column: x => x.IntegrationId,
                        principalTable: "Integrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationFields_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestCommits",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestCommits", x => new { x.QuestId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestCommits_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestCommits_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestCommits_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestCompletions",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestCompletions", x => new { x.QuestId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestCompletions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestCompletions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestCompletions_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCommentRequired = table.Column<bool>(type: "bit", nullable: false),
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestGoals", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_QuestGoals_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestGoals_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestGoals_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestRewards",
                columns: table => new
                {
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestRewards", x => new { x.QuestId, x.ItemId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestRewards_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestRewards_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestRewards_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestSegments",
                columns: table => new
                {
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestSegments", x => new { x.QuestId, x.SegmentId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestSegments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestSegments_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestSegments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Invitations_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invitations_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invitations_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileAssignments", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ProfileAssignments_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileAssignments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileAssignments_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileAssignments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileAssignments_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegrationInstallationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameWithOwner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Repositories_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repositories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Repositories_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegrationType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReporterProfileId = table.Column<int>(type: "int", nullable: false),
                    AssigneeProfileId = table.Column<int>(type: "int", nullable: false),
                    AssigneeProfileId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLogs", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_TaskLogs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskLogs_Profiles_AssigneeProfileId1",
                        column: x => x.AssigneeProfileId1,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskLogs_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskLogs_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ExternalProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegrationType = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AutoTimeSpentInMinutes = table.Column<int>(type: "int", nullable: true),
                    TimeSpentInMinutes = table.Column<int>(type: "int", nullable: true),
                    TimeOriginalEstimatInMinutes = table.Column<int>(type: "int", nullable: true),
                    StoryPoints = table.Column<int>(type: "int", nullable: true),
                    Complexity = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    BugSeverity = table.Column<int>(type: "int", nullable: true),
                    BugPopulationAffect = table.Column<float>(type: "real", nullable: true),
                    IsProductionBugCausing = table.Column<bool>(type: "bit", nullable: false),
                    IsProductionBugFixing = table.Column<bool>(type: "bit", nullable: false),
                    EffortScore = table.Column<float>(type: "real", nullable: true),
                    Labels = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDateId = table.Column<int>(type: "int", nullable: false),
                    ReporterProfileId = table.Column<int>(type: "int", nullable: false),
                    AssigneeExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssigneeProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SegmentAreaId = table.Column<int>(type: "int", nullable: true),
                    SegmentAreaId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: true),
                    TaskCategoryId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Tasks_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Profiles_AssigneeProfileId",
                        column: x => x.AssigneeProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_SegmentAreas_SegmentAreaId1",
                        column: x => x.SegmentAreaId1,
                        principalTable: "SegmentAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskCategories_TaskCategoryId1",
                        column: x => x.TaskCategoryId1,
                        principalTable: "TaskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMetrics",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMetrics", x => new { x.TeamId, x.Type, x.DateId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_TeamMetrics_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMetrics_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    IsUnassigned = table.Column<bool>(type: "bit", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityTotal = table.Column<int>(type: "int", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedTotal = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentTotal = table.Column<float>(type: "real", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreTotal = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenTotal = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedTotal = table.Column<int>(type: "int", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverTotal = table.Column<int>(type: "int", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorTotal = table.Column<float>(type: "real", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionTotal = table.Column<float>(type: "real", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesTotal = table.Column<int>(type: "int", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsBoughtTotal = table.Column<int>(type: "int", nullable: false),
                    QuestsCompletedChange = table.Column<int>(type: "int", nullable: false),
                    QuestsCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    MembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    NonMembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamReportsDaily", x => new { x.DateId, x.TeamId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_TeamReportsDaily_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamReportsDaily_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamReportsWeekly",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentAverage = table.Column<float>(type: "real", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedAverage = table.Column<float>(type: "real", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedAverage = table.Column<float>(type: "real", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverAverage = table.Column<float>(type: "real", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorAverage = table.Column<float>(type: "real", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionAverage = table.Column<float>(type: "real", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesAverage = table.Column<float>(type: "real", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeAverage = table.Column<int>(type: "int", nullable: false),
                    RangeChange = table.Column<int>(type: "int", nullable: false),
                    RangeAverage = table.Column<int>(type: "int", nullable: false),
                    OImpactAverage = table.Column<float>(type: "real", nullable: false),
                    OImpactAverageTotal = table.Column<float>(type: "real", nullable: false),
                    DImpactAverage = table.Column<float>(type: "real", nullable: false),
                    DImpactAverageTotal = table.Column<float>(type: "real", nullable: false),
                    PowerAverage = table.Column<float>(type: "real", nullable: false),
                    PowerAverageTotal = table.Column<float>(type: "real", nullable: false),
                    SpeedAverage = table.Column<float>(type: "real", nullable: false),
                    SpeedAverageTotal = table.Column<float>(type: "real", nullable: false),
                    HeatAverageTotal = table.Column<float>(type: "real", nullable: false),
                    MembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    NonMembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamReportsWeekly", x => new { x.DateId, x.TeamId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_TeamReportsWeekly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamReportsWeekly_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimBundleTokenTxns",
                columns: table => new
                {
                    ClaimBundleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBundleTokenTxns", x => new { x.ClaimBundleId, x.TokenTransactionId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ClaimBundleTokenTxns_ClaimBundles_ClaimBundleId",
                        column: x => x.ClaimBundleId,
                        principalTable: "ClaimBundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimBundleTokenTxns_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimBundleTokenTxns_TokenTransactions_TokenTransactionId",
                        column: x => x.TokenTransactionId,
                        principalTable: "TokenTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PullRequestReviewComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExternalUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommenterProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PullRequestReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PullRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PullRequestReviewComments", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_PullRequestReviewComments_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequestReviewComments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviewComments_Profiles_CommenterProfileId",
                        column: x => x.CommenterProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviewComments_PullRequestReviews_PullRequestReviewId",
                        column: x => x.PullRequestReviewId,
                        principalTable: "PullRequestReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequestReviewComments_PullRequests_PullRequestId",
                        column: x => x.PullRequestId,
                        principalTable: "PullRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestGoalCompletions",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoalId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestGoalCompletions", x => new { x.GoalId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_QuestGoalCompletions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestGoalCompletions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestGoalCompletions_QuestGoals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "QuestGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPoints_OrganizationId",
                table: "ActionPoints",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPoints_ProfileId_OrganizationId",
                table: "ActionPoints",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPoints_SegmentId_OrganizationId",
                table: "ActionPoints",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSettings_OrganizationId",
                table: "ActionPointSettings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSettings_SegmentId_OrganizationId",
                table: "ActionPointSettings",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_OrganizationId",
                table: "Blobs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleItems_OrganizationId",
                table: "ClaimBundleItems",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleItems_ProfileInventoryItemId_OrganizationId",
                table: "ClaimBundleItems",
                columns: new[] { "ProfileInventoryItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundles_OrganizationId",
                table: "ClaimBundles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundles_ProfileId_OrganizationId",
                table: "ClaimBundles",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleTokenTxns_OrganizationId",
                table: "ClaimBundleTokenTxns",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleTokenTxns_TokenTransactionId_OrganizationId",
                table: "ClaimBundleTokenTxns",
                columns: new[] { "TokenTransactionId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangeLogs_OrganizationId",
                table: "EntityChangeLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_GitCommits_AuthorProfileId_OrganizationId",
                table: "GitCommits",
                columns: new[] { "AuthorProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_GitCommits_OrganizationId",
                table: "GitCommits",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationFields_IntegrationId_OrganizationId",
                table: "IntegrationFields",
                columns: new[] { "IntegrationId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationFields_OrganizationId",
                table: "IntegrationFields",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_OrganizationId",
                table: "Integrations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_ProfileId_SegmentId_OrganizationId",
                table: "Integrations",
                columns: new[] { "ProfileId", "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_SegmentId_OrganizationId",
                table: "Integrations",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_OrganizationId",
                table: "Invitations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_SegmentId_OrganizationId",
                table: "Invitations",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_TeamId_OrganizationId",
                table: "Invitations",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ItemId_OrganizationId",
                table: "ItemDisenchants",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_OrganizationId",
                table: "ItemDisenchants",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ProfileId_OrganizationId",
                table: "ItemDisenchants",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ItemId_OrganizationId",
                table: "ItemGifts",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_OrganizationId",
                table: "ItemGifts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ReceiverId_OrganizationId",
                table: "ItemGifts",
                columns: new[] { "ReceiverId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_SenderId_OrganizationId",
                table: "ItemGifts",
                columns: new[] { "SenderId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Items_OrganizationId",
                table: "Items",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_LogDevices_OrganizationId",
                table: "LogDevices",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_LogDevices_ProfileId_OrganizationId",
                table: "LogDevices",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_LoginLogs_OrganizationId",
                table: "LoginLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_OrganizationId",
                table: "Logs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_LogSettings_OrganizationId",
                table: "LogSettings",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_LogSettings_ProfileId_OrganizationId",
                table: "LogSettings",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_OrganizationId",
                table: "ProfileAssignments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_ProfileId_OrganizationId",
                table: "ProfileAssignments",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_ProfileId_OrganizationId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId", "OrganizationId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments",
                columns: new[] { "TeamId", "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_OrganizationId",
                table: "ProfileExternalIds",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_ProfileId_OrganizationId",
                table: "ProfileExternalIds",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_SegmentId_OrganizationId",
                table: "ProfileExternalIds",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ItemId_ProfileId_IsActive_OrganizationId",
                table: "ProfileInventoryItems",
                columns: new[] { "ItemId", "ProfileId", "IsActive", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_OrganizationId",
                table: "ProfileInventoryItems",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ProfileId_IsActive_OrganizationId",
                table: "ProfileInventoryItems",
                columns: new[] { "ProfileId", "IsActive", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_LogId_OrganizationId",
                table: "ProfileLogs",
                columns: new[] { "LogId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_OrganizationId",
                table: "ProfileLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_ProfileId_Event_OrganizationId",
                table: "ProfileLogs",
                columns: new[] { "ProfileId", "Event", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileMetrics_OrganizationId",
                table: "ProfileMetrics",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileMetrics_SegmentId_OrganizationId",
                table: "ProfileMetrics",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePraises_OrganizationId",
                table: "ProfilePraises",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePraises_ProfileId_OrganizationId",
                table: "ProfilePraises",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_OrganizationId",
                table: "ProfileReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_ProfileId_OrganizationId",
                table: "ProfileReportsDaily",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_SegmentId_OrganizationId",
                table: "ProfileReportsDaily",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_OrganizationId",
                table: "ProfileReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_ProfileId_OrganizationId",
                table: "ProfileReportsWeekly",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_SegmentId_OrganizationId",
                table: "ProfileReportsWeekly",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_IdentityId_OrganizationId",
                table: "Profiles",
                columns: new[] { "IdentityId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_OrganizationId",
                table: "Profiles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Username_OrganizationId",
                table: "Profiles",
                columns: new[] { "Username", "OrganizationId" },
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_CommenterProfileId_OrganizationId",
                table: "PullRequestReviewComments",
                columns: new[] { "CommenterProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_OrganizationId",
                table: "PullRequestReviewComments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_PullRequestId_OrganizationId",
                table: "PullRequestReviewComments",
                columns: new[] { "PullRequestId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_PullRequestReviewId_OrganizationId",
                table: "PullRequestReviewComments",
                columns: new[] { "PullRequestReviewId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviews_OrganizationId",
                table: "PullRequestReviews",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviews_PullRequestId_OrganizationId",
                table: "PullRequestReviews",
                columns: new[] { "PullRequestId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviews_ReviewerProfileId_OrganizationId",
                table: "PullRequestReviews",
                columns: new[] { "ReviewerProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_AuthorProfileId_OrganizationId",
                table: "PullRequests",
                columns: new[] { "AuthorProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_OrganizationId",
                table: "PullRequests",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestCommits_OrganizationId",
                table: "QuestCommits",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestCommits_ProfileId_OrganizationId",
                table: "QuestCommits",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestCompletions_OrganizationId",
                table: "QuestCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestCompletions_ProfileId_OrganizationId",
                table: "QuestCompletions",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoalCompletions_OrganizationId",
                table: "QuestGoalCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoalCompletions_ProfileId_OrganizationId",
                table: "QuestGoalCompletions",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoals_OrganizationId",
                table: "QuestGoals",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoals_QuestId_OrganizationId",
                table: "QuestGoals",
                columns: new[] { "QuestId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestRewards_ItemId_OrganizationId",
                table: "QuestRewards",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestRewards_OrganizationId",
                table: "QuestRewards",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_OrganizationId",
                table: "Quests",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_SegmentId_OrganizationId",
                table: "Quests",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestSegments_OrganizationId",
                table: "QuestSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestSegments_SegmentId_OrganizationId",
                table: "QuestSegments",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_OrganizationId",
                table: "Repositories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_TeamId_OrganizationId",
                table: "Repositories",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentAreas_Name_OrganizationId",
                table: "SegmentAreas",
                columns: new[] { "Name", "OrganizationId" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentAreas_OrganizationId",
                table: "SegmentAreas",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentMetrics_OrganizationId",
                table: "SegmentMetrics",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_OrganizationId",
                table: "SegmentReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_SegmentId_OrganizationId",
                table: "SegmentReportsDaily",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_OrganizationId",
                table: "SegmentReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_SegmentId_OrganizationId",
                table: "SegmentReportsWeekly",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key_ArchivedAt_OrganizationId",
                table: "Segments",
                columns: new[] { "Key", "ArchivedAt", "OrganizationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Segments_OrganizationId",
                table: "Segments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ItemId_OrganizationId",
                table: "ShopItems",
                columns: new[] { "ItemId", "OrganizationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_OrganizationId",
                table: "ShopItems",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemSegments_OrganizationId",
                table: "ShopItemSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemSegments_SegmentId_OrganizationId",
                table: "ShopItemSegments",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopLogs_LogId_OrganizationId",
                table: "ShopLogs",
                columns: new[] { "LogId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopLogs_OrganizationId",
                table: "ShopLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ItemId_OrganizationId",
                table: "ShopPurchases",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_OrganizationId",
                table: "ShopPurchases",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ProfileId_Status_OrganizationId",
                table: "ShopPurchases",
                columns: new[] { "ProfileId", "Status", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_SegmentId_OrganizationId",
                table: "ShopPurchases",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_OrganizationId",
                table: "Shops",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_Name",
                table: "TaskCategories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_AssigneeProfileId1_OrganizationId",
                table: "TaskLogs",
                columns: new[] { "AssigneeProfileId1", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_OrganizationId",
                table: "TaskLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_SegmentId_OrganizationId",
                table: "TaskLogs",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_TeamId_OrganizationId",
                table: "TaskLogs",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneeProfileId_OrganizationId",
                table: "Tasks",
                columns: new[] { "AssigneeProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_OrganizationId",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_SegmentId_OrganizationId",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType", "SegmentId", "OrganizationId" },
                unique: true,
                filter: "[ExternalId] IS NOT NULL AND [SegmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OrganizationId",
                table: "Tasks",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SegmentAreaId1_OrganizationId",
                table: "Tasks",
                columns: new[] { "SegmentAreaId1", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SegmentId_OrganizationId",
                table: "Tasks",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskCategoryId1_OrganizationId",
                table: "Tasks",
                columns: new[] { "TaskCategoryId1", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TeamId_OrganizationId",
                table: "Tasks",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_OrganizationId",
                table: "TaskSyncs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_SegmentId_OrganizationId",
                table: "TaskSyncs",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMetrics_OrganizationId",
                table: "TeamMetrics",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_OrganizationId",
                table: "TeamReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TeamId_OrganizationId",
                table: "TeamReportsDaily",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_OrganizationId",
                table: "TeamReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TeamId_OrganizationId",
                table: "TeamReportsWeekly",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_OrganizationId",
                table: "Teams",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_OrganizationId",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchivedAt", "OrganizationId" },
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_OrganizationId",
                table: "Tokens",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_OrganizationId",
                table: "TokenTransactions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_ProfileId_OrganizationId",
                table: "TokenTransactions",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_TokenId_OrganizationId",
                table: "TokenTransactions",
                columns: new[] { "TokenId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_WebhookEventLogs_OrganizationId",
                table: "WebhookEventLogs",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionPoints");

            migrationBuilder.DropTable(
                name: "ActionPointSettings");

            migrationBuilder.DropTable(
                name: "Blobs");

            migrationBuilder.DropTable(
                name: "ClaimBundleItems");

            migrationBuilder.DropTable(
                name: "ClaimBundleTokenTxns");

            migrationBuilder.DropTable(
                name: "EntityChangeLogs");

            migrationBuilder.DropTable(
                name: "GitCommits");

            migrationBuilder.DropTable(
                name: "IntegrationFields");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "ItemDisenchants");

            migrationBuilder.DropTable(
                name: "ItemGifts");

            migrationBuilder.DropTable(
                name: "LoginLogs");

            migrationBuilder.DropTable(
                name: "LogSettings");

            migrationBuilder.DropTable(
                name: "ProfileAssignments");

            migrationBuilder.DropTable(
                name: "ProfileExternalIds");

            migrationBuilder.DropTable(
                name: "ProfileLogs");

            migrationBuilder.DropTable(
                name: "ProfileMetrics");

            migrationBuilder.DropTable(
                name: "ProfilePraises");

            migrationBuilder.DropTable(
                name: "ProfileReportsDaily");

            migrationBuilder.DropTable(
                name: "ProfileReportsWeekly");

            migrationBuilder.DropTable(
                name: "PullRequestReviewComments");

            migrationBuilder.DropTable(
                name: "QuestCommits");

            migrationBuilder.DropTable(
                name: "QuestCompletions");

            migrationBuilder.DropTable(
                name: "QuestGoalCompletions");

            migrationBuilder.DropTable(
                name: "QuestRewards");

            migrationBuilder.DropTable(
                name: "QuestSegments");

            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DropTable(
                name: "SegmentMetrics");

            migrationBuilder.DropTable(
                name: "SegmentReportsDaily");

            migrationBuilder.DropTable(
                name: "SegmentReportsWeekly");

            migrationBuilder.DropTable(
                name: "ShopItemSegments");

            migrationBuilder.DropTable(
                name: "ShopLogs");

            migrationBuilder.DropTable(
                name: "ShopPurchases");

            migrationBuilder.DropTable(
                name: "TaskLogs");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskSyncs");

            migrationBuilder.DropTable(
                name: "TeamMetrics");

            migrationBuilder.DropTable(
                name: "TeamReportsDaily");

            migrationBuilder.DropTable(
                name: "TeamReportsWeekly");

            migrationBuilder.DropTable(
                name: "WebhookEventLogs");

            migrationBuilder.DropTable(
                name: "ProfileInventoryItems");

            migrationBuilder.DropTable(
                name: "ClaimBundles");

            migrationBuilder.DropTable(
                name: "TokenTransactions");

            migrationBuilder.DropTable(
                name: "Integrations");

            migrationBuilder.DropTable(
                name: "LogDevices");

            migrationBuilder.DropTable(
                name: "PullRequestReviews");

            migrationBuilder.DropTable(
                name: "QuestGoals");

            migrationBuilder.DropTable(
                name: "ShopItems");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "SegmentAreas");

            migrationBuilder.DropTable(
                name: "TaskCategories");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "PullRequests");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Segments");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
