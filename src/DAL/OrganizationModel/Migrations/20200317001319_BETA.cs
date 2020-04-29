using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class BETA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Purpose = table.Column<int>(nullable: false),
                    Filename = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Filesize = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityChangeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    EntityType = table.Column<string>(nullable: true),
                    EntityId = table.Column<int>(nullable: false),
                    EntityState = table.Column<int>(nullable: false),
                    AuditType = table.Column<byte>(nullable: false),
                    ChangedValues = table.Column<string>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    IsActivable = table.Column<bool>(nullable: false),
                    IsDisenchantable = table.Column<bool>(nullable: false),
                    IsGiftable = table.Column<bool>(nullable: false),
                    IsQuantityLimited = table.Column<bool>(nullable: false),
                    Rarity = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    IdentityId = table.Column<int>(nullable: false),
                    ClaimsJson = table.Column<string>(nullable: true),
                    FailReason = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    Event = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    Username = table.Column<string>(maxLength: 20, nullable: true),
                    Avatar = table.Column<string>(maxLength: 2000, nullable: true),
                    Role = table.Column<int>(nullable: false),
                    JobPosition = table.Column<string>(maxLength: 100, nullable: true),
                    BornOn = table.Column<DateTime>(nullable: true),
                    EmployedOn = table.Column<DateTime>(nullable: true),
                    IdentityId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    ArchievedAt = table.Column<long>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SegmentAreas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(maxLength: 2000, nullable: true),
                    Timezone = table.Column<string>(maxLength: 50, nullable: true),
                    DataStore = table.Column<string>(maxLength: 4000, nullable: true),
                    DataWarehouse = table.Column<string>(maxLength: 4000, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    ArchievedAt = table.Column<long>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ClosedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCategories", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_TaskCategories_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskCategories_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Symbol = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    SupplyAddress = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebhookEventLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemReservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    QuantityChange = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReservations", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ItemReservations_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemReservations_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemReservations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    QuantityReservedRemaining = table.Column<int>(nullable: true),
                    DiscountPrice = table.Column<float>(nullable: true),
                    DiscountEndsAt = table.Column<DateTime>(nullable: true),
                    FeaturedUntil = table.Column<DateTime>(nullable: true),
                    DisabledAt = table.Column<DateTime>(nullable: true),
                    IsGlobal = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    ArchievedAt = table.Column<long>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimBundles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    RewardClaimedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimBundles_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemDisenchants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    SenderId = table.Column<int>(nullable: false),
                    ReceiverId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 250, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                name: "ProfileInventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    AcquireMethod = table.Column<int>(nullable: false),
                    AcquireDetail = table.Column<string>(nullable: true),
                    ClaimRequired = table.Column<bool>(nullable: false),
                    ClaimedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                    ProfileId = table.Column<int>(nullable: false),
                    LogId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Event = table.Column<int>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileLogs_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileOneUps",
                columns: table => new
                {
                    UppedProfileId = table.Column<int>(nullable: false),
                    DateId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileOneUps", x => new { x.DateId, x.UppedProfileId, x.CreatedBy, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileOneUps_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileOneUps_Profiles_UppedProfileId",
                        column: x => x.UppedProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: true),
                    ProfileId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    DateId = table.Column<int>(nullable: false),
                    ConcludedOn = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                    Type = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    NotifyByEmail = table.Column<bool>(nullable: false),
                    NotifyByPush = table.Column<bool>(nullable: false),
                    NotifyByNotification = table.Column<bool>(nullable: false),
                    MuteUntil = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionPointSettings", x => new { x.Type, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ActionPointSettings_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionPointSettings_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CompletionsLimit = table.Column<int>(nullable: true),
                    CompletionsRemaining = table.Column<int>(nullable: true),
                    IsEasterEgg = table.Column<bool>(nullable: false),
                    IsArchived = table.Column<bool>(nullable: false),
                    ActiveUntil = table.Column<DateTime>(nullable: true),
                    EndedAt = table.Column<DateTime>(nullable: true),
                    RewardValue = table.Column<float>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    SegmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Challenges_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenges_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Challenges_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Integrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: true),
                    SegmentId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                    IntegrationType = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(maxLength: 100, nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileExternalIds", x => new { x.ExternalId, x.IntegrationType, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileExternalIds_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ShopPurchases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    IsFeatured = table.Column<bool>(nullable: false),
                    IsDiscounted = table.Column<bool>(nullable: false),
                    GiftFor = table.Column<int>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    PriceDiscountedFor = table.Column<float>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(nullable: false),
                    AvatarColor = table.Column<string>(maxLength: 50, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    ArchievedAt = table.Column<long>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                    ShopId = table.Column<int>(nullable: false),
                    LogId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Event = table.Column<int>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopLogs_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    TaskCategoryId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IterationCount = table.Column<int>(nullable: false),
                    ProfileRole = table.Column<int>(nullable: false),
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityTotal = table.Column<int>(nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(nullable: false),
                    CompanyTokensEarnedTotal = table.Column<float>(nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(nullable: false),
                    CompanyTokensSpentTotal = table.Column<float>(nullable: false),
                    EffortScoreChange = table.Column<float>(nullable: false),
                    EffortScoreTotal = table.Column<float>(nullable: false),
                    OneUpsGivenChange = table.Column<int>(nullable: false),
                    OneUpsGivenTotal = table.Column<int>(nullable: false),
                    OneUpsReceivedChange = table.Column<int>(nullable: false),
                    OneUpsReceivedTotal = table.Column<int>(nullable: false),
                    AssistsChange = table.Column<int>(nullable: false),
                    AssistsTotal = table.Column<int>(nullable: false),
                    TasksCompletedChange = table.Column<int>(nullable: false),
                    TasksCompletedTotal = table.Column<int>(nullable: false),
                    TurnoverChange = table.Column<int>(nullable: false),
                    TurnoverTotal = table.Column<int>(nullable: false),
                    ErrorChange = table.Column<float>(nullable: false),
                    ErrorTotal = table.Column<float>(nullable: false),
                    ContributionChange = table.Column<float>(nullable: false),
                    ContributionTotal = table.Column<float>(nullable: false),
                    SavesChange = table.Column<int>(nullable: false),
                    SavesTotal = table.Column<int>(nullable: false),
                    TacklesChange = table.Column<int>(nullable: false),
                    TacklesTotal = table.Column<int>(nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(nullable: false),
                    TasksCompletionTimeTotal = table.Column<int>(nullable: false),
                    InventoryCountTotal = table.Column<int>(nullable: false),
                    InventoryValueTotal = table.Column<float>(nullable: false),
                    ItemsBoughtChange = table.Column<int>(nullable: false),
                    ItemsBoughtTotal = table.Column<int>(nullable: false),
                    ItemsGiftedChange = table.Column<int>(nullable: false),
                    ItemsGiftedTotal = table.Column<int>(nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(nullable: false),
                    ItemsDisenchantedTotal = table.Column<int>(nullable: false),
                    ItemsCreatedChange = table.Column<int>(nullable: false),
                    ItemsCreatedTotal = table.Column<int>(nullable: false),
                    ActivityChartJson = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileReportsDaily", x => new { x.DateId, x.ProfileId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileReportsDaily_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileReportsDaily_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileReportsDaily_TaskCategories_TaskCategoryId",
                        column: x => x.TaskCategoryId,
                        principalTable: "TaskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileReportsWeekly",
                columns: table => new
                {
                    DateId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    TaskCategoryId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IterationCount = table.Column<int>(nullable: false),
                    ProfileRole = table.Column<int>(nullable: false),
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityTotalAverage = table.Column<float>(nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(nullable: false),
                    CompanyTokensEarnedTotalAverage = table.Column<float>(nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(nullable: false),
                    CompanyTokensSpentTotalAverage = table.Column<float>(nullable: false),
                    EffortScoreChange = table.Column<float>(nullable: false),
                    EffortScoreTotalAverage = table.Column<float>(nullable: false),
                    OneUpsGivenChange = table.Column<int>(nullable: false),
                    OneUpsGivenTotalAverage = table.Column<float>(nullable: false),
                    OneUpsReceivedChange = table.Column<int>(nullable: false),
                    OneUpsReceivedTotalAverage = table.Column<float>(nullable: false),
                    AssistsChange = table.Column<int>(nullable: false),
                    AssistsTotalAverage = table.Column<float>(nullable: false),
                    TasksCompletedChange = table.Column<int>(nullable: false),
                    TasksCompletedTotalAverage = table.Column<float>(nullable: false),
                    TurnoverChange = table.Column<int>(nullable: false),
                    TurnoverTotalAverage = table.Column<float>(nullable: false),
                    ErrorChange = table.Column<float>(nullable: false),
                    ErrorTotalAverage = table.Column<float>(nullable: false),
                    ContributionChange = table.Column<float>(nullable: false),
                    ContributionTotalAverage = table.Column<float>(nullable: false),
                    SavesChange = table.Column<int>(nullable: false),
                    SavesTotalAverage = table.Column<float>(nullable: false),
                    TacklesChange = table.Column<int>(nullable: false),
                    TacklesTotalAverage = table.Column<float>(nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(nullable: false),
                    TasksCompletionTimeAverage = table.Column<int>(nullable: false),
                    RangeChange = table.Column<int>(nullable: false),
                    RangeTotalAverage = table.Column<int>(nullable: false),
                    InventoryCountTotal = table.Column<int>(nullable: false),
                    InventoryValueTotal = table.Column<float>(nullable: false),
                    ItemsBoughtChange = table.Column<int>(nullable: false),
                    ItemsGiftedChange = table.Column<int>(nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(nullable: false),
                    ItemsCreatedChange = table.Column<int>(nullable: false),
                    OImpactAverage = table.Column<float>(nullable: false),
                    OImpactTotalAverage = table.Column<float>(nullable: false),
                    DImpactAverage = table.Column<float>(nullable: false),
                    DImpactTotalAverage = table.Column<float>(nullable: false),
                    PowerAverage = table.Column<float>(nullable: false),
                    PowerTotalAverage = table.Column<float>(nullable: false),
                    SpeedAverage = table.Column<float>(nullable: false),
                    SpeedTotalAverage = table.Column<float>(nullable: false),
                    Heat = table.Column<float>(nullable: false),
                    HeatIndex = table.Column<float>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileReportsWeekly", x => new { x.DateId, x.ProfileId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ProfileReportsWeekly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileReportsWeekly_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileReportsWeekly_TaskCategories_TaskCategoryId",
                        column: x => x.TaskCategoryId,
                        principalTable: "TaskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SegmentReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    TaskCategoryId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IterationCount = table.Column<int>(nullable: false),
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityTotal = table.Column<int>(nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(nullable: false),
                    CompanyTokensEarnedTotal = table.Column<float>(nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(nullable: false),
                    CompanyTokensSpentTotal = table.Column<float>(nullable: false),
                    EffortScoreChange = table.Column<float>(nullable: false),
                    EffortScoreTotal = table.Column<float>(nullable: false),
                    OneUpsGivenChange = table.Column<int>(nullable: false),
                    OneUpsGivenTotal = table.Column<int>(nullable: false),
                    OneUpsReceivedChange = table.Column<int>(nullable: false),
                    OneUpsReceivedTotal = table.Column<int>(nullable: false),
                    AssistsChange = table.Column<int>(nullable: false),
                    AssistsTotal = table.Column<int>(nullable: false),
                    TasksCompletedChange = table.Column<int>(nullable: false),
                    TasksCompletedTotal = table.Column<int>(nullable: false),
                    TurnoverChange = table.Column<int>(nullable: false),
                    TurnoverTotal = table.Column<int>(nullable: false),
                    ErrorChange = table.Column<float>(nullable: false),
                    ErrorTotal = table.Column<float>(nullable: false),
                    ContributionChange = table.Column<float>(nullable: false),
                    ContributionTotal = table.Column<float>(nullable: false),
                    SavesChange = table.Column<int>(nullable: false),
                    SavesTotal = table.Column<int>(nullable: false),
                    TacklesChange = table.Column<int>(nullable: false),
                    TacklesTotal = table.Column<int>(nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(nullable: false),
                    TasksCompletionTimeTotal = table.Column<int>(nullable: false),
                    ItemsBoughtChange = table.Column<int>(nullable: false),
                    ItemsBoughtTotal = table.Column<int>(nullable: false),
                    ItemsGiftedChange = table.Column<int>(nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(nullable: false),
                    ItemsCreatedChange = table.Column<int>(nullable: false),
                    MembersCountTotal = table.Column<int>(nullable: false),
                    NonMembersCountTotal = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentReportsDaily", x => new { x.DateId, x.SegmentId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_SegmentReportsDaily_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegmentReportsDaily_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SegmentReportsDaily_TaskCategories_TaskCategoryId",
                        column: x => x.TaskCategoryId,
                        principalTable: "TaskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SegmentReportsWeekly",
                columns: table => new
                {
                    DateId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    TaskCategoryId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IterationCount = table.Column<int>(nullable: false),
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityAverage = table.Column<float>(nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(nullable: false),
                    CompanyTokensEarnedAverage = table.Column<float>(nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(nullable: false),
                    CompanyTokensSpentAverage = table.Column<float>(nullable: false),
                    EffortScoreChange = table.Column<float>(nullable: false),
                    EffortScoreAverage = table.Column<float>(nullable: false),
                    OneUpsGivenChange = table.Column<int>(nullable: false),
                    OneUpsGivenAverage = table.Column<float>(nullable: false),
                    OneUpsReceivedChange = table.Column<int>(nullable: false),
                    OneUpsReceivedAverage = table.Column<float>(nullable: false),
                    AssistsChange = table.Column<int>(nullable: false),
                    AssistsAverage = table.Column<float>(nullable: false),
                    TasksCompletedChange = table.Column<int>(nullable: false),
                    TasksCompletedAverage = table.Column<float>(nullable: false),
                    TurnoverChange = table.Column<int>(nullable: false),
                    TurnoverAverage = table.Column<float>(nullable: false),
                    ErrorChange = table.Column<float>(nullable: false),
                    ErrorAverage = table.Column<float>(nullable: false),
                    ContributionChange = table.Column<float>(nullable: false),
                    ContributionAverage = table.Column<float>(nullable: false),
                    SavesChange = table.Column<int>(nullable: false),
                    SavesAverage = table.Column<float>(nullable: false),
                    TacklesChange = table.Column<int>(nullable: false),
                    TacklesAverage = table.Column<float>(nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(nullable: false),
                    TasksCompletionTimeAverage = table.Column<int>(nullable: false),
                    RangeChange = table.Column<int>(nullable: false),
                    RangeAverage = table.Column<int>(nullable: false),
                    ItemsBoughtChange = table.Column<int>(nullable: false),
                    ItemsGiftedChange = table.Column<int>(nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(nullable: false),
                    ItemsCreatedChange = table.Column<int>(nullable: false),
                    OImpactAverage = table.Column<float>(nullable: false),
                    OImpactAverageTotal = table.Column<float>(nullable: false),
                    DImpactAverage = table.Column<float>(nullable: false),
                    DImpactAverageTotal = table.Column<float>(nullable: false),
                    PowerAverage = table.Column<float>(nullable: false),
                    PowerAverageTotal = table.Column<float>(nullable: false),
                    SpeedAverage = table.Column<float>(nullable: false),
                    SpeedAverageTotal = table.Column<float>(nullable: false),
                    HeatAverageTotal = table.Column<float>(nullable: false),
                    MembersCountTotal = table.Column<int>(nullable: false),
                    NonMembersCountTotal = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentReportsWeekly", x => new { x.DateId, x.SegmentId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_SegmentReportsWeekly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegmentReportsWeekly_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SegmentReportsWeekly_TaskCategories_TaskCategoryId",
                        column: x => x.TaskCategoryId,
                        principalTable: "TaskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    IsIndividual = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    ScheduledEndAt = table.Column<DateTime>(nullable: true),
                    EndedAt = table.Column<DateTime>(nullable: true),
                    TokenRewardValue = table.Column<double>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Theme = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    RepeatWhenCompleted = table.Column<bool>(nullable: false),
                    RepeatCount = table.Column<int>(nullable: false),
                    PreviousCompetitionId = table.Column<int>(nullable: true),
                    TokenId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    WinnerId = table.Column<int>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Competitions_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competitions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Competitions_Competitions_PreviousCompetitionId",
                        column: x => x.PreviousCompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TokenTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    FinalBalance = table.Column<double>(nullable: false),
                    TxnHash = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(maxLength: 200, nullable: true),
                    Reason = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    TokenId = table.Column<int>(nullable: false),
                    ClaimRequired = table.Column<bool>(nullable: false),
                    ClaimedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                    ShopItemId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    DiscountPrice = table.Column<float>(nullable: true),
                    DiscountEndsAt = table.Column<DateTime>(nullable: true),
                    HiddenAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemSegments", x => new { x.ShopItemId, x.SegmentId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ShopItemSegments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "ClaimBundleItems",
                columns: table => new
                {
                    ClaimBundleId = table.Column<int>(nullable: false),
                    ProfileInventoryItemId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimBundleItems_ProfileInventoryItems_ProfileInventoryItemId",
                        column: x => x.ProfileInventoryItemId,
                        principalTable: "ProfileInventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeCommits",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    ChallengeId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeCommits", x => new { x.ChallengeId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeCommits_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeCommits_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeCommits_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeCompletions",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    ChallengeId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeCompletions", x => new { x.ChallengeId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeCompletions_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeCompletions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeCompletions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeGoals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    IsCommentRequired = table.Column<bool>(nullable: false),
                    ChallengeId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeGoals", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ChallengeGoals_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeGoals_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeGoals_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeRewards",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeRewards", x => new { x.ChallengeId, x.ItemId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeRewards_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeRewards_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeRewards_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeSegments",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeSegments", x => new { x.ChallengeId, x.SegmentId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeSegments_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeSegments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeSegments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    IntegrationId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Code = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 1000, nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    SegmentId = table.Column<int>(nullable: true),
                    TeamId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                name: "TaskLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(nullable: true),
                    IntegrationType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ReporterProfileId = table.Column<int>(nullable: false),
                    AssigneeProfileId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLogs", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_TaskLogs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskLogs_Profiles_AssigneeProfileId",
                        column: x => x.AssigneeProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(nullable: true),
                    ExternalProjectId = table.Column<string>(nullable: true),
                    IntegrationType = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    AutoTimeSpentInMinutes = table.Column<int>(nullable: true),
                    TimeSpentInMinutes = table.Column<int>(nullable: true),
                    TimeOriginalEstimatInMinutes = table.Column<int>(nullable: true),
                    StoryPoints = table.Column<int>(nullable: true),
                    Complexity = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    BugSeverity = table.Column<int>(nullable: true),
                    BugPopulationAffect = table.Column<float>(nullable: true),
                    IsProductionBugCausing = table.Column<bool>(nullable: false),
                    IsProductionBugFixing = table.Column<bool>(nullable: false),
                    EffortScore = table.Column<float>(nullable: true),
                    Labels = table.Column<string>(nullable: true),
                    LastModifiedDateId = table.Column<int>(nullable: false),
                    ReporterProfileId = table.Column<int>(nullable: false),
                    AssigneeExternalId = table.Column<string>(nullable: true),
                    AssigneeProfileId = table.Column<int>(nullable: true),
                    TeamId = table.Column<int>(nullable: true),
                    SegmentId = table.Column<int>(nullable: true),
                    SegmentAreaId = table.Column<int>(nullable: true),
                    TaskCategoryId = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Tasks_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Profiles_AssigneeProfileId",
                        column: x => x.AssigneeProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_SegmentAreas_SegmentAreaId",
                        column: x => x.SegmentAreaId,
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
                        name: "FK_Tasks_TaskCategories_TaskCategoryId",
                        column: x => x.TaskCategoryId,
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
                name: "TeamReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    TaskCategoryId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IterationCount = table.Column<int>(nullable: false),
                    IsUnassigned = table.Column<bool>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityTotal = table.Column<int>(nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(nullable: false),
                    CompanyTokensEarnedTotal = table.Column<float>(nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(nullable: false),
                    CompanyTokensSpentTotal = table.Column<float>(nullable: false),
                    EffortScoreChange = table.Column<float>(nullable: false),
                    EffortScoreTotal = table.Column<float>(nullable: false),
                    OneUpsGivenChange = table.Column<int>(nullable: false),
                    OneUpsGivenTotal = table.Column<int>(nullable: false),
                    OneUpsReceivedChange = table.Column<int>(nullable: false),
                    OneUpsReceivedTotal = table.Column<int>(nullable: false),
                    AssistsChange = table.Column<int>(nullable: false),
                    AssistsTotal = table.Column<int>(nullable: false),
                    TasksCompletedChange = table.Column<int>(nullable: false),
                    TasksCompletedTotal = table.Column<int>(nullable: false),
                    TurnoverChange = table.Column<int>(nullable: false),
                    TurnoverTotal = table.Column<int>(nullable: false),
                    ErrorChange = table.Column<float>(nullable: false),
                    ErrorTotal = table.Column<float>(nullable: false),
                    ContributionChange = table.Column<float>(nullable: false),
                    ContributionTotal = table.Column<float>(nullable: false),
                    SavesChange = table.Column<int>(nullable: false),
                    SavesTotal = table.Column<int>(nullable: false),
                    TacklesChange = table.Column<int>(nullable: false),
                    TacklesTotal = table.Column<int>(nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(nullable: false),
                    TasksCompletionTimeTotal = table.Column<int>(nullable: false),
                    MembersCountTotal = table.Column<int>(nullable: false),
                    NonMembersCountTotal = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamReportsDaily", x => new { x.DateId, x.TeamId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_TeamReportsDaily_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamReportsDaily_TaskCategories_TaskCategoryId",
                        column: x => x.TaskCategoryId,
                        principalTable: "TaskCategories",
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
                    DateId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    TaskCategoryId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IterationCount = table.Column<int>(nullable: false),
                    SegmentId = table.Column<int>(nullable: false),
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityAverage = table.Column<float>(nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(nullable: false),
                    CompanyTokensEarnedAverage = table.Column<float>(nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(nullable: false),
                    CompanyTokensSpentAverage = table.Column<float>(nullable: false),
                    EffortScoreChange = table.Column<float>(nullable: false),
                    EffortScoreAverage = table.Column<float>(nullable: false),
                    OneUpsGivenChange = table.Column<int>(nullable: false),
                    OneUpsGivenAverage = table.Column<float>(nullable: false),
                    OneUpsReceivedChange = table.Column<int>(nullable: false),
                    OneUpsReceivedAverage = table.Column<float>(nullable: false),
                    AssistsChange = table.Column<int>(nullable: false),
                    AssistsAverage = table.Column<float>(nullable: false),
                    TasksCompletedChange = table.Column<int>(nullable: false),
                    TasksCompletedAverage = table.Column<float>(nullable: false),
                    TurnoverChange = table.Column<int>(nullable: false),
                    TurnoverAverage = table.Column<float>(nullable: false),
                    ErrorChange = table.Column<float>(nullable: false),
                    ErrorAverage = table.Column<float>(nullable: false),
                    ContributionChange = table.Column<float>(nullable: false),
                    ContributionAverage = table.Column<float>(nullable: false),
                    SavesChange = table.Column<int>(nullable: false),
                    SavesAverage = table.Column<float>(nullable: false),
                    TacklesChange = table.Column<int>(nullable: false),
                    TacklesAverage = table.Column<float>(nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(nullable: false),
                    TasksCompletionTimeAverage = table.Column<int>(nullable: false),
                    RangeChange = table.Column<int>(nullable: false),
                    RangeAverage = table.Column<int>(nullable: false),
                    OImpactAverage = table.Column<float>(nullable: false),
                    OImpactAverageTotal = table.Column<float>(nullable: false),
                    DImpactAverage = table.Column<float>(nullable: false),
                    DImpactAverageTotal = table.Column<float>(nullable: false),
                    PowerAverage = table.Column<float>(nullable: false),
                    PowerAverageTotal = table.Column<float>(nullable: false),
                    SpeedAverage = table.Column<float>(nullable: false),
                    SpeedAverageTotal = table.Column<float>(nullable: false),
                    HeatAverageTotal = table.Column<float>(nullable: false),
                    MembersCountTotal = table.Column<int>(nullable: false),
                    NonMembersCountTotal = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamReportsWeekly", x => new { x.DateId, x.TeamId, x.TaskCategoryId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_TeamReportsWeekly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamReportsWeekly_TaskCategories_TaskCategoryId",
                        column: x => x.TaskCategoryId,
                        principalTable: "TaskCategories",
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
                name: "CompetitionLogs",
                columns: table => new
                {
                    CompetitionId = table.Column<int>(nullable: false),
                    LogId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Event = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionLogs", x => new { x.CompetitionId, x.LogId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_CompetitionLogs_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionLogs_Logs_LogId",
                        column: x => x.LogId,
                        principalTable: "Logs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionLogs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionRewards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    TokenValue = table.Column<float>(nullable: false),
                    TokenId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    CompetitionId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionRewards", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_CompetitionRewards_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionRewards_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionRewards_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionRewards_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionRewards_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    CompetitionId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: true),
                    TeamId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ScoreValue = table.Column<double>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Competitors_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competitors_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitors_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Competitors_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitors_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClaimBundleTokenTxns",
                columns: table => new
                {
                    ClaimBundleId = table.Column<int>(nullable: false),
                    TokenTransactionId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimBundleTokenTxns_TokenTransactions_TokenTransactionId",
                        column: x => x.TokenTransactionId,
                        principalTable: "TokenTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeGoalCompletions",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false),
                    GoalId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeGoalCompletions", x => new { x.GoalId, x.ProfileId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_ChallengeGoalCompletions_ChallengeGoals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "ChallengeGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeGoalCompletions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeGoalCompletions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompetitorScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    CompetitorId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    CompetitionId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitorScores", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_CompetitorScores_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitorScores_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitorScores_Competitors_CompetitorId",
                        column: x => x.CompetitorId,
                        principalTable: "Competitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitorScores_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitorScores_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitorScores_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
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
                name: "IX_ChallengeCommits_OrganizationId",
                table: "ChallengeCommits",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCommits_ProfileId_OrganizationId",
                table: "ChallengeCommits",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCompletions_OrganizationId",
                table: "ChallengeCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCompletions_ProfileId_OrganizationId",
                table: "ChallengeCompletions",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoalCompletions_OrganizationId",
                table: "ChallengeGoalCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoalCompletions_ProfileId_OrganizationId",
                table: "ChallengeGoalCompletions",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoals_OrganizationId",
                table: "ChallengeGoals",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeGoals_ChallengeId_OrganizationId",
                table: "ChallengeGoals",
                columns: new[] { "ChallengeId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRewards_OrganizationId",
                table: "ChallengeRewards",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRewards_ItemId_OrganizationId",
                table: "ChallengeRewards",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_OrganizationId",
                table: "Challenges",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_SegmentId_OrganizationId",
                table: "Challenges",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeSegments_OrganizationId",
                table: "ChallengeSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeSegments_SegmentId_OrganizationId",
                table: "ChallengeSegments",
                columns: new[] { "SegmentId", "OrganizationId" });

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
                name: "IX_CompetitionLogs_OrganizationId",
                table: "CompetitionLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionLogs_LogId_OrganizationId",
                table: "CompetitionLogs",
                columns: new[] { "LogId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionLogs_CompetitionId_Event_OrganizationId",
                table: "CompetitionLogs",
                columns: new[] { "CompetitionId", "Event", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_OrganizationId",
                table: "CompetitionRewards",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_CompetitionId_OrganizationId",
                table: "CompetitionRewards",
                columns: new[] { "CompetitionId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_ItemId_OrganizationId",
                table: "CompetitionRewards",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_TokenId_OrganizationId",
                table: "CompetitionRewards",
                columns: new[] { "TokenId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_OrganizationId",
                table: "Competitions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_PreviousCompetitionId_OrganizationId",
                table: "Competitions",
                columns: new[] { "PreviousCompetitionId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_SegmentId_OrganizationId",
                table: "Competitions",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_TokenId_OrganizationId",
                table: "Competitions",
                columns: new[] { "TokenId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_OrganizationId",
                table: "Competitors",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_ProfileId_OrganizationId",
                table: "Competitors",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_TeamId_OrganizationId",
                table: "Competitors",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_OrganizationId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_TeamId_OrganizationId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_TeamId_OrganizationId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId", "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_OrganizationId",
                table: "CompetitorScores",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_CompetitionId_OrganizationId",
                table: "CompetitorScores",
                columns: new[] { "CompetitionId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_ProfileId_OrganizationId",
                table: "CompetitorScores",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_TeamId_OrganizationId",
                table: "CompetitorScores",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_CompetitorId_ProfileId_OrganizationId",
                table: "CompetitorScores",
                columns: new[] { "CompetitorId", "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_CompetitorId_TeamId_OrganizationId",
                table: "CompetitorScores",
                columns: new[] { "CompetitorId", "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangeLogs_OrganizationId",
                table: "EntityChangeLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationFields_OrganizationId",
                table: "IntegrationFields",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationFields_IntegrationId_OrganizationId",
                table: "IntegrationFields",
                columns: new[] { "IntegrationId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_OrganizationId",
                table: "Integrations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_SegmentId_OrganizationId",
                table: "Integrations",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_ProfileId_SegmentId_OrganizationId",
                table: "Integrations",
                columns: new[] { "ProfileId", "SegmentId", "OrganizationId" });

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
                name: "IX_ItemDisenchants_OrganizationId",
                table: "ItemDisenchants",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ItemId_OrganizationId",
                table: "ItemDisenchants",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ProfileId_OrganizationId",
                table: "ItemDisenchants",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_OrganizationId",
                table: "ItemGifts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ItemId_OrganizationId",
                table: "ItemGifts",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ReceiverId_OrganizationId",
                table: "ItemGifts",
                columns: new[] { "ReceiverId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_SenderId_OrganizationId",
                table: "ItemGifts",
                columns: new[] { "SenderId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemReservations_OrganizationId",
                table: "ItemReservations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemReservations_ItemId_OrganizationId",
                table: "ItemReservations",
                columns: new[] { "ItemId", "OrganizationId" });

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
                name: "IX_ProfileAssignments_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments",
                columns: new[] { "TeamId", "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_OrganizationId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId", "OrganizationId" });

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
                name: "IX_ProfileInventoryItems_OrganizationId",
                table: "ProfileInventoryItems",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ProfileId_IsActive_OrganizationId",
                table: "ProfileInventoryItems",
                columns: new[] { "ProfileId", "IsActive", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ItemId_ProfileId_IsActive_OrganizationId",
                table: "ProfileInventoryItems",
                columns: new[] { "ItemId", "ProfileId", "IsActive", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_OrganizationId",
                table: "ProfileLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_LogId_OrganizationId",
                table: "ProfileLogs",
                columns: new[] { "LogId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_ProfileId_Event_OrganizationId",
                table: "ProfileLogs",
                columns: new[] { "ProfileId", "Event", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileOneUps_OrganizationId",
                table: "ProfileOneUps",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileOneUps_UppedProfileId_OrganizationId",
                table: "ProfileOneUps",
                columns: new[] { "UppedProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_OrganizationId",
                table: "ProfileReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_ProfileId_OrganizationId",
                table: "ProfileReportsDaily",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_TaskCategoryId_OrganizationId",
                table: "ProfileReportsDaily",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_OrganizationId",
                table: "ProfileReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_ProfileId_OrganizationId",
                table: "ProfileReportsWeekly",
                columns: new[] { "ProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_TaskCategoryId_OrganizationId",
                table: "ProfileReportsWeekly",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_OrganizationId",
                table: "Profiles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_IdentityId_OrganizationId",
                table: "Profiles",
                columns: new[] { "IdentityId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Username_OrganizationId",
                table: "Profiles",
                columns: new[] { "Username", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentAreas_OrganizationId",
                table: "SegmentAreas",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentAreas_Name_OrganizationId",
                table: "SegmentAreas",
                columns: new[] { "Name", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_OrganizationId",
                table: "SegmentReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_SegmentId_OrganizationId",
                table: "SegmentReportsDaily",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsDaily_TaskCategoryId_OrganizationId",
                table: "SegmentReportsDaily",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_OrganizationId",
                table: "SegmentReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_SegmentId_OrganizationId",
                table: "SegmentReportsWeekly",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentReportsWeekly_TaskCategoryId_OrganizationId",
                table: "SegmentReportsWeekly",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Segments_OrganizationId",
                table: "Segments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_Key_ArchievedAt_OrganizationId",
                table: "Segments",
                columns: new[] { "Key", "ArchievedAt", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_OrganizationId",
                table: "ShopItems",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ItemId_OrganizationId",
                table: "ShopItems",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemSegments_OrganizationId",
                table: "ShopItemSegments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemSegments_SegmentId_OrganizationId",
                table: "ShopItemSegments",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopLogs_OrganizationId",
                table: "ShopLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopLogs_LogId_OrganizationId",
                table: "ShopLogs",
                columns: new[] { "LogId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_OrganizationId",
                table: "ShopPurchases",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ItemId_OrganizationId",
                table: "ShopPurchases",
                columns: new[] { "ItemId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_SegmentId_OrganizationId",
                table: "ShopPurchases",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ProfileId_Status_OrganizationId",
                table: "ShopPurchases",
                columns: new[] { "ProfileId", "Status", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_OrganizationId",
                table: "Shops",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_OrganizationId",
                table: "TaskCategories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_Name_OrganizationId",
                table: "TaskCategories",
                columns: new[] { "Name", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_OrganizationId",
                table: "TaskLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_AssigneeProfileId_OrganizationId",
                table: "TaskLogs",
                columns: new[] { "AssigneeProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_SegmentId_OrganizationId",
                table: "TaskLogs",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_TeamId_OrganizationId",
                table: "TaskLogs",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OrganizationId",
                table: "Tasks",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneeProfileId_OrganizationId",
                table: "Tasks",
                columns: new[] { "AssigneeProfileId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SegmentAreaId_OrganizationId",
                table: "Tasks",
                columns: new[] { "SegmentAreaId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SegmentId_OrganizationId",
                table: "Tasks",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskCategoryId_OrganizationId",
                table: "Tasks",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TeamId_OrganizationId",
                table: "Tasks",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_OrganizationId",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType_SegmentId_OrganizationId",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType", "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_OrganizationId",
                table: "TaskSyncs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_SegmentId_OrganizationId",
                table: "TaskSyncs",
                columns: new[] { "SegmentId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_OrganizationId",
                table: "TeamReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TaskCategoryId_OrganizationId",
                table: "TeamReportsDaily",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TeamId_OrganizationId",
                table: "TeamReportsDaily",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_OrganizationId",
                table: "TeamReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TaskCategoryId_OrganizationId",
                table: "TeamReportsWeekly",
                columns: new[] { "TaskCategoryId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TeamId_OrganizationId",
                table: "TeamReportsWeekly",
                columns: new[] { "TeamId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_OrganizationId",
                table: "Teams",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SegmentId_Key_ArchievedAt_OrganizationId",
                table: "Teams",
                columns: new[] { "SegmentId", "Key", "ArchievedAt", "OrganizationId" });

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
                name: "ChallengeCommits");

            migrationBuilder.DropTable(
                name: "ChallengeCompletions");

            migrationBuilder.DropTable(
                name: "ChallengeGoalCompletions");

            migrationBuilder.DropTable(
                name: "ChallengeRewards");

            migrationBuilder.DropTable(
                name: "ChallengeSegments");

            migrationBuilder.DropTable(
                name: "ClaimBundleItems");

            migrationBuilder.DropTable(
                name: "ClaimBundleTokenTxns");

            migrationBuilder.DropTable(
                name: "CompetitionLogs");

            migrationBuilder.DropTable(
                name: "CompetitionRewards");

            migrationBuilder.DropTable(
                name: "CompetitorScores");

            migrationBuilder.DropTable(
                name: "EntityChangeLogs");

            migrationBuilder.DropTable(
                name: "IntegrationFields");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "ItemDisenchants");

            migrationBuilder.DropTable(
                name: "ItemGifts");

            migrationBuilder.DropTable(
                name: "ItemReservations");

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
                name: "ProfileOneUps");

            migrationBuilder.DropTable(
                name: "ProfileReportsDaily");

            migrationBuilder.DropTable(
                name: "ProfileReportsWeekly");

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
                name: "TeamReportsDaily");

            migrationBuilder.DropTable(
                name: "TeamReportsWeekly");

            migrationBuilder.DropTable(
                name: "WebhookEventLogs");

            migrationBuilder.DropTable(
                name: "ChallengeGoals");

            migrationBuilder.DropTable(
                name: "ProfileInventoryItems");

            migrationBuilder.DropTable(
                name: "ClaimBundles");

            migrationBuilder.DropTable(
                name: "TokenTransactions");

            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.DropTable(
                name: "Integrations");

            migrationBuilder.DropTable(
                name: "LogDevices");

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
                name: "Challenges");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Segments");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
