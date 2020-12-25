using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalTenants",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCreateSegmentOnboarding = table.Column<bool>(type: "bit", nullable: false),
                    IsAddSourcesOnboarding = table.Column<bool>(type: "bit", nullable: false),
                    IsInviteUsersOnboarding = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalTenants", x => x.TenantId);
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Blobs", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Blobs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blobs_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Items", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Items_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoginLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLogs", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_LoginLogs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginLogs_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Event = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Logs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Profiles", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Profiles_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SegmentAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentAreas", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_SegmentAreas_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SegmentAreas_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Segments", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Segments_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segments_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Shops_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shops_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebhookEventLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegrationType = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookEventLogs", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_WebhookEventLogs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookEventLogs_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ShopItems", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_ShopItems_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopItems_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimBundles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RewardClaimedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBundles", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_ClaimBundles_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimBundles_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_GitCommits", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_GitCommits_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GitCommits_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDisenchants", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_ItemDisenchants_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDisenchants_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemDisenchants_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ItemGifts", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_ItemGifts_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemGifts_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemGifts_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDevices", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_LogDevices_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogDevices_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ProfileInventoryItems", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_ProfileInventoryItems_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileInventoryItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileInventoryItems_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Event = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileLogs", x => new { x.ProfileId, x.LogId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ProfileLogs_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileLogs_Logs_LogId",
                        column: x => x.LogId,
                        principalTable: "Logs",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ProfilePraises", x => new { x.DateId, x.ProfileId, x.PraiserProfileId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ProfilePraises_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_PullRequests", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_PullRequests_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequests_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PullRequests_Profiles_AuthorProfileId",
                        column: x => x.AuthorProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TokenTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    TxnHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenType = table.Column<int>(type: "int", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    ClaimRequired = table.Column<bool>(type: "bit", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenTransactions", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_TokenTransactions_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenTransactions_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TokenTransactions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ActionPoints", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_ActionPoints_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionPoints_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ActionPointSettings", x => new { x.Type, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ActionPointSettings_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Integrations", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Integrations_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Integrations_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileExternalIds", x => new { x.ExternalId, x.IntegrationType, x.SegmentId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ProfileExternalIds_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileMetrics", x => new { x.ProfileId, x.Type, x.DateId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ProfileMetrics_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ProfileReportsDaily", x => new { x.DateId, x.ProfileId, x.SegmentId, x.TaskCategoryId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ProfileReportsDaily_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ProfileReportsWeekly", x => new { x.DateId, x.ProfileId, x.SegmentId, x.TaskCategoryId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ProfileReportsWeekly_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    Id = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Quests", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Quests_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quests_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentMetrics", x => new { x.SegmentId, x.Type, x.DateId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_SegmentMetrics_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_SegmentReportsDaily", x => new { x.DateId, x.SegmentId, x.TaskCategoryId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_SegmentReportsDaily_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_SegmentReportsWeekly", x => new { x.DateId, x.SegmentId, x.TaskCategoryId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_SegmentReportsWeekly_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ShopPurchases", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_ShopPurchases_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopPurchases_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopPurchases_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegrationType = table.Column<int>(type: "int", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSyncs", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_TaskSyncs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskSyncs_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Teams", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Teams_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Event = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopLogs", x => new { x.ShopId, x.LogId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ShopLogs_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopLogs_Logs_LogId",
                        column: x => x.LogId,
                        principalTable: "Logs",
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
                name: "ShopItemSegments",
                columns: table => new
                {
                    ShopItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountPrice = table.Column<float>(type: "real", nullable: true),
                    DiscountEndsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HiddenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemSegments", x => new { x.ShopItemId, x.SegmentId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ShopItemSegments_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSettings", x => new { x.LogDeviceId, x.LogEvent, x.TenantId });
                    table.ForeignKey(
                        name: "FK_LogSettings_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogSettings_LogDevices_LogDeviceId",
                        column: x => x.LogDeviceId,
                        principalTable: "LogDevices",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBundleItems", x => new { x.ClaimBundleId, x.ProfileInventoryItemId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ClaimBundleItems_ClaimBundles_ClaimBundleId",
                        column: x => x.ClaimBundleId,
                        principalTable: "ClaimBundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimBundleItems_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_PullRequestReviews", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_PullRequestReviews_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequestReviews_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                name: "ClaimBundleTokenTxns",
                columns: table => new
                {
                    ClaimBundleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBundleTokenTxns", x => new { x.ClaimBundleId, x.TokenTransactionId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_ClaimBundleTokenTxns_ClaimBundles_ClaimBundleId",
                        column: x => x.ClaimBundleId,
                        principalTable: "ClaimBundles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimBundleTokenTxns_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimBundleTokenTxns_TokenTransactions_TokenTransactionId",
                        column: x => x.TokenTransactionId,
                        principalTable: "TokenTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_IntegrationFields", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_IntegrationFields_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationFields_Integrations_IntegrationId",
                        column: x => x.IntegrationId,
                        principalTable: "Integrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationFields_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestCommits",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestCommits", x => new { x.QuestId, x.ProfileId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_QuestCommits_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestCompletions", x => new { x.QuestId, x.ProfileId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_QuestCompletions_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    Id = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCommentRequired = table.Column<bool>(type: "bit", nullable: false),
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestGoals", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_QuestGoals_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestGoals_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestRewards", x => new { x.QuestId, x.ItemId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_QuestRewards_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestRewards_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestSegments", x => new { x.QuestId, x.SegmentId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_QuestSegments_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Invitations", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Invitations_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ProfileAssignments", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_ProfileAssignments_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileAssignments_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Repositories", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Repositories_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repositories_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TaskLogs", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_TaskLogs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskLogs_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Tasks", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_Tasks_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMetrics", x => new { x.TeamId, x.Type, x.DateId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_TeamMetrics_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TeamReportsDaily", x => new { x.DateId, x.TeamId, x.TaskCategoryId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_TeamReportsDaily_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TeamReportsWeekly", x => new { x.DateId, x.TeamId, x.TaskCategoryId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_TeamReportsWeekly_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamReportsWeekly_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PullRequestReviewComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_PullRequestReviewComments", x => new { x.Id, x.TenantId });
                    table.UniqueConstraint("AK_PullRequestReviewComments_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PullRequestReviewComments_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestGoalCompletions", x => new { x.GoalId, x.ProfileId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_QuestGoalCompletions_LocalTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "LocalTenants",
                        principalColumn: "TenantId",
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
                name: "IX_ActionPoints_ProfileId_TenantId",
                table: "ActionPoints",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPoints_SegmentId_TenantId",
                table: "ActionPoints",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPoints_TenantId",
                table: "ActionPoints",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSettings_SegmentId_TenantId",
                table: "ActionPointSettings",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActionPointSettings_TenantId",
                table: "ActionPointSettings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_TenantId",
                table: "Blobs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleItems_ProfileInventoryItemId_TenantId",
                table: "ClaimBundleItems",
                columns: new[] { "ProfileInventoryItemId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleItems_TenantId",
                table: "ClaimBundleItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundles_ProfileId_TenantId",
                table: "ClaimBundles",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundles_TenantId",
                table: "ClaimBundles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleTokenTxns_TenantId",
                table: "ClaimBundleTokenTxns",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleTokenTxns_TokenTransactionId_TenantId",
                table: "ClaimBundleTokenTxns",
                columns: new[] { "TokenTransactionId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_GitCommits_AuthorProfileId_TenantId",
                table: "GitCommits",
                columns: new[] { "AuthorProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_GitCommits_TenantId",
                table: "GitCommits",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationFields_IntegrationId_TenantId",
                table: "IntegrationFields",
                columns: new[] { "IntegrationId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationFields_TenantId",
                table: "IntegrationFields",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_ProfileId_SegmentId_TenantId",
                table: "Integrations",
                columns: new[] { "ProfileId", "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_SegmentId_TenantId",
                table: "Integrations",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_TenantId",
                table: "Integrations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_SegmentId_TenantId",
                table: "Invitations",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_TeamId_TenantId",
                table: "Invitations",
                columns: new[] { "TeamId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_TenantId",
                table: "Invitations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ItemId_TenantId",
                table: "ItemDisenchants",
                columns: new[] { "ItemId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ProfileId_TenantId",
                table: "ItemDisenchants",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_TenantId",
                table: "ItemDisenchants",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ItemId_TenantId",
                table: "ItemGifts",
                columns: new[] { "ItemId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ReceiverId_TenantId",
                table: "ItemGifts",
                columns: new[] { "ReceiverId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_SenderId_TenantId",
                table: "ItemGifts",
                columns: new[] { "SenderId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_TenantId",
                table: "ItemGifts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_TenantId",
                table: "Items",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LogDevices_ProfileId_TenantId",
                table: "LogDevices",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LogDevices_TenantId",
                table: "LogDevices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginLogs_TenantId",
                table: "LoginLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_TenantId",
                table: "Logs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LogSettings_ProfileId_TenantId",
                table: "LogSettings",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_LogSettings_TenantId",
                table: "LogSettings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_ProfileId_TenantId",
                table: "ProfileAssignments",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_ProfileId_TenantId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_TenantId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId", "TenantId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_TeamId_ProfileId_TenantId",
                table: "ProfileAssignments",
                columns: new[] { "TeamId", "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_TenantId",
                table: "ProfileAssignments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_ProfileId_TenantId",
                table: "ProfileExternalIds",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_SegmentId_TenantId",
                table: "ProfileExternalIds",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileExternalIds_TenantId",
                table: "ProfileExternalIds",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ItemId_ProfileId_IsActive_TenantId",
                table: "ProfileInventoryItems",
                columns: new[] { "ItemId", "ProfileId", "IsActive", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ProfileId_IsActive_TenantId",
                table: "ProfileInventoryItems",
                columns: new[] { "ProfileId", "IsActive", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_TenantId",
                table: "ProfileInventoryItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_LogId_TenantId",
                table: "ProfileLogs",
                columns: new[] { "LogId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_ProfileId_Event_TenantId",
                table: "ProfileLogs",
                columns: new[] { "ProfileId", "Event", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_TenantId",
                table: "ProfileLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileMetrics_SegmentId_TenantId",
                table: "ProfileMetrics",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileMetrics_TenantId",
                table: "ProfileMetrics",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePraises_ProfileId_TenantId",
                table: "ProfilePraises",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePraises_TenantId",
                table: "ProfilePraises",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_ProfileId_TenantId",
                table: "ProfileReportsDaily",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_SegmentId_TenantId",
                table: "ProfileReportsDaily",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_TenantId",
                table: "ProfileReportsDaily",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_ProfileId_TenantId",
                table: "ProfileReportsWeekly",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_SegmentId_TenantId",
                table: "ProfileReportsWeekly",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_TenantId",
                table: "ProfileReportsWeekly",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_IdentityId_TenantId",
                table: "Profiles",
                columns: new[] { "IdentityId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_TenantId",
                table: "Profiles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Username_TenantId",
                table: "Profiles",
                columns: new[] { "Username", "TenantId" },
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_CommenterProfileId_TenantId",
                table: "PullRequestReviewComments",
                columns: new[] { "CommenterProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_PullRequestId_TenantId",
                table: "PullRequestReviewComments",
                columns: new[] { "PullRequestId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_PullRequestReviewId_TenantId",
                table: "PullRequestReviewComments",
                columns: new[] { "PullRequestReviewId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviewComments_TenantId",
                table: "PullRequestReviewComments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviews_PullRequestId_TenantId",
                table: "PullRequestReviews",
                columns: new[] { "PullRequestId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviews_ReviewerProfileId_TenantId",
                table: "PullRequestReviews",
                columns: new[] { "ReviewerProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequestReviews_TenantId",
                table: "PullRequestReviews",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_AuthorProfileId_TenantId",
                table: "PullRequests",
                columns: new[] { "AuthorProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_PullRequests_TenantId",
                table: "PullRequests",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestCommits_ProfileId_TenantId",
                table: "QuestCommits",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestCommits_TenantId",
                table: "QuestCommits",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestCompletions_ProfileId_TenantId",
                table: "QuestCompletions",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestCompletions_TenantId",
                table: "QuestCompletions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoalCompletions_ProfileId_TenantId",
                table: "QuestGoalCompletions",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoalCompletions_TenantId",
                table: "QuestGoalCompletions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoals_QuestId_TenantId",
                table: "QuestGoals",
                columns: new[] { "QuestId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestGoals_TenantId",
                table: "QuestGoals",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestRewards_ItemId_TenantId",
                table: "QuestRewards",
                columns: new[] { "ItemId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestRewards_TenantId",
                table: "QuestRewards",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_SegmentId_TenantId",
                table: "Quests",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Quests_TenantId",
                table: "Quests",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestSegments_SegmentId_TenantId",
                table: "QuestSegments",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestSegments_TenantId",
                table: "QuestSegments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_TeamId_TenantId",
                table: "Repositories",
                columns: new[] { "TeamId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_TenantId",
                table: "Repositories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentAreas_Name_TenantId",
                table: "SegmentAreas",
                columns: new[] { "Name", "TenantId" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentAreas_TenantId",
                table: "SegmentAreas",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentMetrics_TenantId",
                table: "SegmentMetrics",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_SegmentId_TenantId",
                table: "SegmentReportsDaily",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_TenantId",
                table: "SegmentReportsDaily",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_SegmentId_TenantId",
                table: "SegmentReportsWeekly",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_TenantId",
                table: "SegmentReportsWeekly",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key_ArchivedAt_TenantId",
                table: "Segments",
                columns: new[] { "Key", "ArchivedAt", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Segments_TenantId",
                table: "Segments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ItemId_TenantId",
                table: "ShopItems",
                columns: new[] { "ItemId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_TenantId",
                table: "ShopItems",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemSegments_SegmentId_TenantId",
                table: "ShopItemSegments",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemSegments_TenantId",
                table: "ShopItemSegments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopLogs_LogId_TenantId",
                table: "ShopLogs",
                columns: new[] { "LogId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopLogs_TenantId",
                table: "ShopLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ItemId_TenantId",
                table: "ShopPurchases",
                columns: new[] { "ItemId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ProfileId_Status_TenantId",
                table: "ShopPurchases",
                columns: new[] { "ProfileId", "Status", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_SegmentId_TenantId",
                table: "ShopPurchases",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_TenantId",
                table: "ShopPurchases",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_TenantId",
                table: "Shops",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_Name",
                table: "TaskCategories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_AssigneeProfileId1_TenantId",
                table: "TaskLogs",
                columns: new[] { "AssigneeProfileId1", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_SegmentId_TenantId",
                table: "TaskLogs",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_TeamId_TenantId",
                table: "TaskLogs",
                columns: new[] { "TeamId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_TenantId",
                table: "TaskLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneeProfileId_TenantId",
                table: "Tasks",
                columns: new[] { "AssigneeProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_SegmentId_TenantId",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType", "SegmentId", "TenantId" },
                unique: true,
                filter: "[ExternalId] IS NOT NULL AND [SegmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_TenantId",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SegmentAreaId1_TenantId",
                table: "Tasks",
                columns: new[] { "SegmentAreaId1", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SegmentId_TenantId",
                table: "Tasks",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskCategoryId1_TenantId",
                table: "Tasks",
                columns: new[] { "TaskCategoryId1", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TeamId_TenantId",
                table: "Tasks",
                columns: new[] { "TeamId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TenantId",
                table: "Tasks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_SegmentId_TenantId",
                table: "TaskSyncs",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_TenantId",
                table: "TaskSyncs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMetrics_TenantId",
                table: "TeamMetrics",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TeamId_TenantId",
                table: "TeamReportsDaily",
                columns: new[] { "TeamId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TenantId",
                table: "TeamReportsDaily",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TeamId_TenantId",
                table: "TeamReportsWeekly",
                columns: new[] { "TeamId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TenantId",
                table: "TeamReportsWeekly",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchivedAt_TenantId",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchivedAt", "TenantId" },
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TenantId",
                table: "Teams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_ProfileId_TenantId",
                table: "TokenTransactions",
                columns: new[] { "ProfileId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_TenantId",
                table: "TokenTransactions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookEventLogs_TenantId",
                table: "WebhookEventLogs",
                column: "TenantId");
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
                name: "LocalTenants");
        }
    }
}
