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
                name: "Integrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    WorthValue = table.Column<float>(nullable: false),
                    IsActivable = table.Column<bool>(nullable: false),
                    IsDisenchantable = table.Column<bool>(nullable: false),
                    IsGiftable = table.Column<bool>(nullable: false),
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
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Nickname = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(maxLength: 2000, nullable: true),
                    Role = table.Column<int>(nullable: false),
                    IdentityId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                name: "ProjectAreas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAreas", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_ProjectAreas_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectAreas_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Avatar = table.Column<string>(maxLength: 2000, nullable: true),
                    Timezone = table.Column<string>(maxLength: 50, nullable: true),
                    DataStore = table.Column<string>(maxLength: 4000, nullable: true),
                    DataWarehouse = table.Column<string>(maxLength: 4000, nullable: true),
                    ArchivedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_Projects_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                name: "StatTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IntegrationType = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatTypes", x => new { x.Id, x.OrganizationId });
                    table.UniqueConstraint("AK_StatTypes_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatTypes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                    Id = table.Column<int>(nullable: false),
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
                    Id = table.Column<int>(nullable: false),
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
                name: "IntegrationFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    DiscountPrice = table.Column<float>(nullable: true),
                    DiscountEndsAt = table.Column<DateTime>(nullable: true),
                    FeaturedUntil = table.Column<DateTime>(nullable: true),
                    DisabledAt = table.Column<DateTime>(nullable: true),
                    ArchivedAt = table.Column<DateTime>(nullable: true),
                    IsGlobal = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                    Id = table.Column<int>(nullable: false),
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
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Code = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    EmailId = table.Column<string>(maxLength: 1000, nullable: true),
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
                        name: "FK_Invitations_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemDisenchants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                    Id = table.Column<int>(nullable: false),
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
                name: "ProfileInventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                    table.UniqueConstraint("AK_ProfileLogs_ProfileId_LogId", x => new { x.ProfileId, x.LogId });
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
                    table.UniqueConstraint("AK_ProfileOneUps_DateId_UppedProfileId_CreatedBy", x => new { x.DateId, x.UppedProfileId, x.CreatedBy });
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
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    TokenRewardValue = table.Column<double>(nullable: false),
                    CustomReward = table.Column<string>(nullable: true),
                    CompletionsLimit = table.Column<int>(nullable: true),
                    CompletionsRemaining = table.Column<int>(nullable: true),
                    IsEasterEgg = table.Column<bool>(nullable: false),
                    IsArchived = table.Column<bool>(nullable: false),
                    ActiveUntil = table.Column<DateTime>(nullable: true),
                    EndedAt = table.Column<DateTime>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        name: "FK_Challenges_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => new { x.ProjectId, x.ProfileId, x.OrganizationId });
                    table.UniqueConstraint("AK_ProjectMembers_ProjectId_ProfileId", x => new { x.ProjectId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShopPurchases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
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
                        name: "FK_ShopPurchases_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(maxLength: 2000, nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    ArchivedAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
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
                        name: "FK_Teams_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
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
                    table.UniqueConstraint("AK_ShopLogs_ShopId_LogId", x => new { x.ShopId, x.LogId });
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
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityTotal = table.Column<int>(nullable: false),
                    CompanyTokensChange = table.Column<float>(nullable: false),
                    CompanyTokensTotal = table.Column<float>(nullable: false),
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
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileReportsDaily", x => new { x.DateId, x.ProfileId, x.TaskCategoryId, x.OrganizationId });
                    table.UniqueConstraint("AK_ProfileReportsDaily_DateId_ProfileId_TaskCategoryId", x => new { x.DateId, x.ProfileId, x.TaskCategoryId });
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
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityTotalAverage = table.Column<float>(nullable: false),
                    CompanyTokensChange = table.Column<float>(nullable: false),
                    CompanyTokensTotalAverage = table.Column<float>(nullable: false),
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
                    RangeChange = table.Column<int>(nullable: false),
                    RangeTotalAverage = table.Column<int>(nullable: false),
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
                    table.UniqueConstraint("AK_ProfileReportsWeekly_DateId_ProfileId_TaskCategoryId", x => new { x.DateId, x.ProfileId, x.TaskCategoryId });
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
                name: "ProjectReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    TaskCategoryId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IterationCount = table.Column<int>(nullable: false),
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityTotal = table.Column<int>(nullable: false),
                    CompanyTokensChange = table.Column<float>(nullable: false),
                    CompanyTokensTotal = table.Column<float>(nullable: false),
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
                    MembersCountTotal = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReportsDaily", x => new { x.DateId, x.ProjectId, x.TaskCategoryId, x.OrganizationId });
                    table.UniqueConstraint("AK_ProjectReportsDaily_DateId_ProjectId_TaskCategoryId", x => new { x.DateId, x.ProjectId, x.TaskCategoryId });
                    table.ForeignKey(
                        name: "FK_ProjectReportsDaily_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectReportsDaily_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectReportsDaily_TaskCategories_TaskCategoryId",
                        column: x => x.TaskCategoryId,
                        principalTable: "TaskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectReportsWeekly",
                columns: table => new
                {
                    DateId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    TaskCategoryId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IterationCount = table.Column<int>(nullable: false),
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityAverage = table.Column<float>(nullable: false),
                    CompanyTokensChange = table.Column<float>(nullable: false),
                    CompanyTokensAverage = table.Column<float>(nullable: false),
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
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReportsWeekly", x => new { x.DateId, x.ProjectId, x.TaskCategoryId, x.OrganizationId });
                    table.UniqueConstraint("AK_ProjectReportsWeekly_DateId_ProjectId_TaskCategoryId", x => new { x.DateId, x.ProjectId, x.TaskCategoryId });
                    table.ForeignKey(
                        name: "FK_ProjectReportsWeekly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectReportsWeekly_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectReportsWeekly_TaskCategories_TaskCategoryId",
                        column: x => x.TaskCategoryId,
                        principalTable: "TaskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                    ProjectId = table.Column<int>(nullable: false),
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
                        name: "FK_Competitions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
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
                    Id = table.Column<int>(nullable: false),
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
                name: "ShopItemProjects",
                columns: table => new
                {
                    ShopItemId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    DiscountPrice = table.Column<float>(nullable: true),
                    DiscountEndsAt = table.Column<DateTime>(nullable: true),
                    HiddenAt = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemProjects", x => new { x.ShopItemId, x.ProjectId, x.OrganizationId });
                    table.UniqueConstraint("AK_ShopItemProjects_ShopItemId_ProjectId", x => new { x.ShopItemId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ShopItemProjects_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopItemProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopItemProjects_ShopItems_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItems",
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
                    table.UniqueConstraint("AK_ClaimBundleItems_ClaimBundleId_ProfileInventoryItemId", x => new { x.ClaimBundleId, x.ProfileInventoryItemId });
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
                    table.UniqueConstraint("AK_ChallengeCompletions_ChallengeId_ProfileId", x => new { x.ChallengeId, x.ProfileId });
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
                name: "ProjectTeams",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeams", x => new { x.ProjectId, x.TeamId, x.OrganizationId });
                    table.UniqueConstraint("AK_ProjectTeams_ProjectId_TeamId", x => new { x.ProjectId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_ProjectTeams_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTeams_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(nullable: true),
                    IntegrationType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ReporterProfileId = table.Column<int>(nullable: false),
                    AssigneeProfileId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
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
                        name: "FK_TaskLogs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
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
                    Id = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(nullable: true),
                    IntegrationType = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
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
                    AssigneeProfileId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    ProjectAreaId = table.Column<int>(nullable: true),
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
                        name: "FK_Tasks_ProjectAreas_ProjectAreaId",
                        column: x => x.ProjectAreaId,
                        principalTable: "ProjectAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
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
                name: "TeamMembers",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => new { x.TeamId, x.ProfileId, x.OrganizationId });
                    table.UniqueConstraint("AK_TeamMembers_TeamId_ProfileId", x => new { x.TeamId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_TeamMembers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Teams_TeamId",
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
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityTotal = table.Column<int>(nullable: false),
                    CompanyTokensChange = table.Column<float>(nullable: false),
                    CompanyTokensTotal = table.Column<float>(nullable: false),
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
                    MembersCountTotal = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamReportsDaily", x => new { x.DateId, x.TeamId, x.TaskCategoryId, x.OrganizationId });
                    table.UniqueConstraint("AK_TeamReportsDaily_DateId_TeamId_TaskCategoryId", x => new { x.DateId, x.TeamId, x.TaskCategoryId });
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
                    ComplexityChange = table.Column<int>(nullable: false),
                    ComplexityAverage = table.Column<float>(nullable: false),
                    CompanyTokensChange = table.Column<float>(nullable: false),
                    CompanyTokensAverage = table.Column<float>(nullable: false),
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
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamReportsWeekly", x => new { x.DateId, x.TeamId, x.TaskCategoryId, x.OrganizationId });
                    table.UniqueConstraint("AK_TeamReportsWeekly_DateId_TeamId_TaskCategoryId", x => new { x.DateId, x.TeamId, x.TaskCategoryId });
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
                    table.UniqueConstraint("AK_CompetitionLogs_CompetitionId_LogId", x => new { x.CompetitionId, x.LogId });
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
                    Id = table.Column<int>(nullable: false),
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
                    Id = table.Column<int>(nullable: false),
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
                    table.UniqueConstraint("AK_ClaimBundleTokenTxns_ClaimBundleId_TokenTransactionId", x => new { x.ClaimBundleId, x.TokenTransactionId });
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
                name: "CompetitorScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                name: "IX_ChallengeCompletions_OrganizationId",
                table: "ChallengeCompletions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeCompletions_ProfileId",
                table: "ChallengeCompletions",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_OrganizationId",
                table: "Challenges",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_ProjectId",
                table: "Challenges",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleItems_OrganizationId",
                table: "ClaimBundleItems",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleItems_ProfileInventoryItemId",
                table: "ClaimBundleItems",
                column: "ProfileInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundles_OrganizationId",
                table: "ClaimBundles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundles_ProfileId",
                table: "ClaimBundles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleTokenTxns_OrganizationId",
                table: "ClaimBundleTokenTxns",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBundleTokenTxns_TokenTransactionId",
                table: "ClaimBundleTokenTxns",
                column: "TokenTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionLogs_LogId",
                table: "CompetitionLogs",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionLogs_OrganizationId",
                table: "CompetitionLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionLogs_CompetitionId_Event",
                table: "CompetitionLogs",
                columns: new[] { "CompetitionId", "Event" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_CompetitionId",
                table: "CompetitionRewards",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_ItemId",
                table: "CompetitionRewards",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_OrganizationId",
                table: "CompetitionRewards",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionRewards_TokenId",
                table: "CompetitionRewards",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_OrganizationId",
                table: "Competitions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_PreviousCompetitionId",
                table: "Competitions",
                column: "PreviousCompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_ProjectId",
                table: "Competitions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_TokenId",
                table: "Competitions",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_OrganizationId",
                table: "Competitors",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_ProfileId",
                table: "Competitors",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_TeamId",
                table: "Competitors",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId" },
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_TeamId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "TeamId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Competitors_CompetitionId_ProfileId_TeamId",
                table: "Competitors",
                columns: new[] { "CompetitionId", "ProfileId", "TeamId" },
                unique: true,
                filter: "[ProfileId] IS NOT NULL AND [TeamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_CompetitionId",
                table: "CompetitorScores",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_OrganizationId",
                table: "CompetitorScores",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_ProfileId",
                table: "CompetitorScores",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_TeamId",
                table: "CompetitorScores",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_CompetitorId_ProfileId",
                table: "CompetitorScores",
                columns: new[] { "CompetitorId", "ProfileId" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorScores_CompetitorId_TeamId",
                table: "CompetitorScores",
                columns: new[] { "CompetitorId", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangeLogs_OrganizationId",
                table: "EntityChangeLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationFields_IntegrationId",
                table: "IntegrationFields",
                column: "IntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationFields_OrganizationId",
                table: "IntegrationFields",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_OrganizationId",
                table: "Integrations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_OrganizationId",
                table: "Invitations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ProfileId",
                table: "Invitations",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ItemId",
                table: "ItemDisenchants",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_OrganizationId",
                table: "ItemDisenchants",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDisenchants_ProfileId",
                table: "ItemDisenchants",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ItemId",
                table: "ItemGifts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_OrganizationId",
                table: "ItemGifts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_ReceiverId",
                table: "ItemGifts",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGifts_SenderId",
                table: "ItemGifts",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_OrganizationId",
                table: "Items",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_OrganizationId",
                table: "Logs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_OrganizationId",
                table: "ProfileInventoryItems",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ProfileId_IsActive",
                table: "ProfileInventoryItems",
                columns: new[] { "ProfileId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInventoryItems_ItemId_ProfileId_IsActive",
                table: "ProfileInventoryItems",
                columns: new[] { "ItemId", "ProfileId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_LogId",
                table: "ProfileLogs",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_OrganizationId",
                table: "ProfileLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLogs_ProfileId_Event",
                table: "ProfileLogs",
                columns: new[] { "ProfileId", "Event" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileOneUps_OrganizationId",
                table: "ProfileOneUps",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileOneUps_UppedProfileId",
                table: "ProfileOneUps",
                column: "UppedProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_OrganizationId",
                table: "ProfileReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_ProfileId",
                table: "ProfileReportsDaily",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsDaily_TaskCategoryId",
                table: "ProfileReportsDaily",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_OrganizationId",
                table: "ProfileReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_ProfileId",
                table: "ProfileReportsWeekly",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileReportsWeekly_TaskCategoryId",
                table: "ProfileReportsWeekly",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Nickname",
                table: "Profiles",
                column: "Nickname",
                unique: true,
                filter: "[Nickname] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_OrganizationId",
                table: "Profiles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAreas_Name",
                table: "ProjectAreas",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAreas_OrganizationId",
                table: "ProjectAreas",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_OrganizationId",
                table: "ProjectMembers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProfileId",
                table: "ProjectMembers",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReportsDaily_OrganizationId",
                table: "ProjectReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReportsDaily_ProjectId",
                table: "ProjectReportsDaily",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReportsDaily_TaskCategoryId",
                table: "ProjectReportsDaily",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReportsWeekly_OrganizationId",
                table: "ProjectReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReportsWeekly_ProjectId",
                table: "ProjectReportsWeekly",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReportsWeekly_TaskCategoryId",
                table: "ProjectReportsWeekly",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Key",
                table: "Projects",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId",
                table: "Projects",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeams_OrganizationId",
                table: "ProjectTeams",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeams_TeamId",
                table: "ProjectTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProjects_OrganizationId",
                table: "ShopItemProjects",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProjects_ProjectId",
                table: "ShopItemProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_ItemId",
                table: "ShopItems",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopItems_OrganizationId",
                table: "ShopItems",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopLogs_LogId",
                table: "ShopLogs",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopLogs_OrganizationId",
                table: "ShopLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ItemId",
                table: "ShopPurchases",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_OrganizationId",
                table: "ShopPurchases",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ProjectId",
                table: "ShopPurchases",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopPurchases_ProfileId_Status",
                table: "ShopPurchases",
                columns: new[] { "ProfileId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Shops_OrganizationId",
                table: "Shops",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_StatTypes_OrganizationId",
                table: "StatTypes",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_Name",
                table: "TaskCategories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_OrganizationId",
                table: "TaskCategories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_AssigneeProfileId",
                table: "TaskLogs",
                column: "AssigneeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_OrganizationId",
                table: "TaskLogs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_ProjectId",
                table: "TaskLogs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLogs_TeamId",
                table: "TaskLogs",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssigneeProfileId",
                table: "Tasks",
                column: "AssigneeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OrganizationId",
                table: "Tasks",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectAreaId",
                table: "Tasks",
                column: "ProjectAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskCategoryId",
                table: "Tasks",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TeamId",
                table: "Tasks",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExternalId_IntegrationType",
                table: "Tasks",
                columns: new[] { "ExternalId", "IntegrationType" },
                unique: true,
                filter: "[ExternalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_OrganizationId",
                table: "TeamMembers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_ProfileId",
                table: "TeamMembers",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_OrganizationId",
                table: "TeamReportsDaily",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TaskCategoryId",
                table: "TeamReportsDaily",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsDaily_TeamId",
                table: "TeamReportsDaily",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_OrganizationId",
                table: "TeamReportsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TaskCategoryId",
                table: "TeamReportsWeekly",
                column: "TaskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamReportsWeekly_TeamId",
                table: "TeamReportsWeekly",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_OrganizationId",
                table: "Teams",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ProjectId",
                table: "Teams",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Key_ArchivedAt",
                table: "Teams",
                columns: new[] { "Key", "ArchivedAt" },
                unique: true,
                filter: "[Key] IS NOT NULL AND [ArchivedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_OrganizationId",
                table: "Tokens",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_OrganizationId",
                table: "TokenTransactions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_ProfileId",
                table: "TokenTransactions",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenTransactions_TokenId",
                table: "TokenTransactions",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookEventLogs_OrganizationId",
                table: "WebhookEventLogs",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeCompletions");

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
                name: "ProfileLogs");

            migrationBuilder.DropTable(
                name: "ProfileOneUps");

            migrationBuilder.DropTable(
                name: "ProfileReportsDaily");

            migrationBuilder.DropTable(
                name: "ProfileReportsWeekly");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "ProjectReportsDaily");

            migrationBuilder.DropTable(
                name: "ProjectReportsWeekly");

            migrationBuilder.DropTable(
                name: "ProjectTeams");

            migrationBuilder.DropTable(
                name: "ShopItemProjects");

            migrationBuilder.DropTable(
                name: "ShopLogs");

            migrationBuilder.DropTable(
                name: "ShopPurchases");

            migrationBuilder.DropTable(
                name: "StatTypes");

            migrationBuilder.DropTable(
                name: "TaskLogs");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "TeamReportsDaily");

            migrationBuilder.DropTable(
                name: "TeamReportsWeekly");

            migrationBuilder.DropTable(
                name: "WebhookEventLogs");

            migrationBuilder.DropTable(
                name: "Challenges");

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
                name: "ShopItems");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "ProjectAreas");

            migrationBuilder.DropTable(
                name: "TaskCategories");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
