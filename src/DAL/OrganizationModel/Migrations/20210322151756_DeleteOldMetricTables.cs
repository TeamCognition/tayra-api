using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tayra.Models.Organizations.Migrations
{
    public partial class DeleteOldMetricTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskCategories_TaskCategoryId1",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "ProfileReportsDaily");

            migrationBuilder.DropTable(
                name: "ProfileReportsWeekly");

            migrationBuilder.DropTable(
                name: "SegmentReportsDaily");

            migrationBuilder.DropTable(
                name: "SegmentReportsWeekly");

            migrationBuilder.DropTable(
                name: "TaskCategories");

            migrationBuilder.DropTable(
                name: "TaskSyncs");

            migrationBuilder.DropTable(
                name: "TeamReportsDaily");

            migrationBuilder.DropTable(
                name: "TeamReportsWeekly");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskCategoryId1_TenantId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_TenantId",
                table: "ProfileAssignments");

            migrationBuilder.DropColumn(
                name: "TaskCategoryId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskCategoryId1",
                table: "Tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "ProfileAssignments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "Invitations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SegmentId",
                table: "Invitations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_TenantId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId", "TenantId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_TenantId",
                table: "ProfileAssignments");

            migrationBuilder.AddColumn<int>(
                name: "TaskCategoryId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaskCategoryId1",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "ProfileAssignments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "Invitations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "SegmentId",
                table: "Invitations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "ProfileReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityChartJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsTotal = table.Column<int>(type: "int", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedTotal = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentTotal = table.Column<float>(type: "real", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityTotal = table.Column<int>(type: "int", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionTotal = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreTotal = table.Column<float>(type: "real", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorTotal = table.Column<float>(type: "real", nullable: false),
                    InventoryCountTotal = table.Column<int>(type: "int", nullable: false),
                    InventoryValueTotal = table.Column<float>(type: "real", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsBoughtTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedTotal = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenTotal = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedTotal = table.Column<int>(type: "int", nullable: false),
                    ProfileRole = table.Column<int>(type: "int", nullable: false),
                    QuestsCompletedChange = table.Column<int>(type: "int", nullable: false),
                    QuestsCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesTotal = table.Column<int>(type: "int", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeTotal = table.Column<int>(type: "int", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverTotal = table.Column<int>(type: "int", nullable: false)
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
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsTotalAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedTotalAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentTotalAverage = table.Column<float>(type: "real", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityTotalAverage = table.Column<float>(type: "real", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionTotalAverage = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DImpactAverage = table.Column<float>(type: "real", nullable: false),
                    DImpactTotalAverage = table.Column<float>(type: "real", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreTotalAverage = table.Column<float>(type: "real", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorTotalAverage = table.Column<float>(type: "real", nullable: false),
                    Heat = table.Column<float>(type: "real", nullable: false),
                    HeatIndex = table.Column<float>(type: "real", nullable: false),
                    InventoryCountTotal = table.Column<int>(type: "int", nullable: false),
                    InventoryValueTotal = table.Column<float>(type: "real", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedChange = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OImpactAverage = table.Column<float>(type: "real", nullable: false),
                    OImpactTotalAverage = table.Column<float>(type: "real", nullable: false),
                    PowerAverage = table.Column<float>(type: "real", nullable: false),
                    PowerTotalAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenTotalAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedTotalAverage = table.Column<float>(type: "real", nullable: false),
                    ProfileRole = table.Column<int>(type: "int", nullable: false),
                    RangeChange = table.Column<int>(type: "int", nullable: false),
                    RangeTotalAverage = table.Column<int>(type: "int", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesTotalAverage = table.Column<float>(type: "real", nullable: false),
                    SpeedAverage = table.Column<float>(type: "real", nullable: false),
                    SpeedTotalAverage = table.Column<float>(type: "real", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesTotalAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedTotalAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletionTimeAverage = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverTotalAverage = table.Column<float>(type: "real", nullable: false)
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
                name: "SegmentReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsTotal = table.Column<int>(type: "int", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedTotal = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentTotal = table.Column<float>(type: "real", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityTotal = table.Column<int>(type: "int", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionTotal = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreTotal = table.Column<float>(type: "real", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorTotal = table.Column<float>(type: "real", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsBoughtTotal = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedChange = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    NonMembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenTotal = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedTotal = table.Column<int>(type: "int", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesTotal = table.Column<int>(type: "int", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeTotal = table.Column<int>(type: "int", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverTotal = table.Column<int>(type: "int", nullable: false)
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
                    AssistsAverage = table.Column<float>(type: "real", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    CompanyTokensEarnedAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    ComplexityAverage = table.Column<float>(type: "real", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ContributionAverage = table.Column<float>(type: "real", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DImpactAverage = table.Column<float>(type: "real", nullable: false),
                    DImpactAverageTotal = table.Column<float>(type: "real", nullable: false),
                    EffortScoreAverage = table.Column<float>(type: "real", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    ErrorAverage = table.Column<float>(type: "real", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    HeatAverageTotal = table.Column<float>(type: "real", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsCreatedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsDisenchantedChange = table.Column<int>(type: "int", nullable: false),
                    ItemsGiftedChange = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    NonMembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    OImpactAverage = table.Column<float>(type: "real", nullable: false),
                    OImpactAverageTotal = table.Column<float>(type: "real", nullable: false),
                    PowerAverage = table.Column<float>(type: "real", nullable: false),
                    PowerAverageTotal = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    RangeAverage = table.Column<int>(type: "int", nullable: false),
                    RangeChange = table.Column<int>(type: "int", nullable: false),
                    SavesAverage = table.Column<float>(type: "real", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SpeedAverage = table.Column<float>(type: "real", nullable: false),
                    SpeedAverageTotal = table.Column<float>(type: "real", nullable: false),
                    TacklesAverage = table.Column<float>(type: "real", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeAverage = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverAverage = table.Column<float>(type: "real", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false)
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
                name: "TaskCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskSyncs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    ExternalProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegrationType = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "TeamReportsDaily",
                columns: table => new
                {
                    DateId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    AssistsTotal = table.Column<int>(type: "int", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedTotal = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentTotal = table.Column<float>(type: "real", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ComplexityTotal = table.Column<int>(type: "int", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    ContributionTotal = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    EffortScoreTotal = table.Column<float>(type: "real", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    ErrorTotal = table.Column<float>(type: "real", nullable: false),
                    IsUnassigned = table.Column<bool>(type: "bit", nullable: false),
                    ItemsBoughtChange = table.Column<int>(type: "int", nullable: false),
                    ItemsBoughtTotal = table.Column<int>(type: "int", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    NonMembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesGivenTotal = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedTotal = table.Column<int>(type: "int", nullable: false),
                    QuestsCompletedChange = table.Column<int>(type: "int", nullable: false),
                    QuestsCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SavesTotal = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TacklesTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedTotal = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeTotal = table.Column<int>(type: "int", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverTotal = table.Column<int>(type: "int", nullable: false)
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
                    AssistsAverage = table.Column<float>(type: "real", nullable: false),
                    AssistsChange = table.Column<int>(type: "int", nullable: false),
                    CompanyTokensEarnedAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensEarnedChange = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentAverage = table.Column<float>(type: "real", nullable: false),
                    CompanyTokensSpentChange = table.Column<float>(type: "real", nullable: false),
                    ComplexityAverage = table.Column<float>(type: "real", nullable: false),
                    ComplexityChange = table.Column<int>(type: "int", nullable: false),
                    ContributionAverage = table.Column<float>(type: "real", nullable: false),
                    ContributionChange = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DImpactAverage = table.Column<float>(type: "real", nullable: false),
                    DImpactAverageTotal = table.Column<float>(type: "real", nullable: false),
                    EffortScoreAverage = table.Column<float>(type: "real", nullable: false),
                    EffortScoreChange = table.Column<float>(type: "real", nullable: false),
                    ErrorAverage = table.Column<float>(type: "real", nullable: false),
                    ErrorChange = table.Column<float>(type: "real", nullable: false),
                    HeatAverageTotal = table.Column<float>(type: "real", nullable: false),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    NonMembersCountTotal = table.Column<int>(type: "int", nullable: false),
                    OImpactAverage = table.Column<float>(type: "real", nullable: false),
                    OImpactAverageTotal = table.Column<float>(type: "real", nullable: false),
                    PowerAverage = table.Column<float>(type: "real", nullable: false),
                    PowerAverageTotal = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesGivenChange = table.Column<int>(type: "int", nullable: false),
                    PraisesReceivedAverage = table.Column<float>(type: "real", nullable: false),
                    PraisesReceivedChange = table.Column<int>(type: "int", nullable: false),
                    RangeAverage = table.Column<int>(type: "int", nullable: false),
                    RangeChange = table.Column<int>(type: "int", nullable: false),
                    SavesAverage = table.Column<float>(type: "real", nullable: false),
                    SavesChange = table.Column<int>(type: "int", nullable: false),
                    SegmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpeedAverage = table.Column<float>(type: "real", nullable: false),
                    SpeedAverageTotal = table.Column<float>(type: "real", nullable: false),
                    TacklesAverage = table.Column<float>(type: "real", nullable: false),
                    TacklesChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletedAverage = table.Column<float>(type: "real", nullable: false),
                    TasksCompletedChange = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeAverage = table.Column<int>(type: "int", nullable: false),
                    TasksCompletionTimeChange = table.Column<int>(type: "int", nullable: false),
                    TurnoverAverage = table.Column<float>(type: "real", nullable: false),
                    TurnoverChange = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskCategoryId1_TenantId",
                table: "Tasks",
                columns: new[] { "TaskCategoryId1", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAssignments_SegmentId_TeamId_ProfileId_TenantId",
                table: "ProfileAssignments",
                columns: new[] { "SegmentId", "TeamId", "ProfileId", "TenantId" },
                unique: true,
                filter: "[TeamId] IS NOT NULL");

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
                name: "IX_TaskCategories_Name",
                table: "TaskCategories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_SegmentId_TenantId",
                table: "TaskSyncs",
                columns: new[] { "SegmentId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSyncs_TenantId",
                table: "TaskSyncs",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskCategories_TaskCategoryId1",
                table: "Tasks",
                column: "TaskCategoryId1",
                principalTable: "TaskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
