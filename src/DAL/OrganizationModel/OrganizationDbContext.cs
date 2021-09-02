using System;
using System.Linq;
using System.Threading;
using Cog.Core;
using Cog.DAL;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Models.Catalog;

namespace Tayra.Models.Organizations
{
    public class OrganizationDbContext : CogDbContext, IMultiTenantDbContext
    {
        public ITenantInfo TenantInfo { get; }
        public TenantMismatchMode TenantMismatchMode => TenantMismatchMode.Overwrite;
        public TenantNotSetMode TenantNotSetMode => TenantNotSetMode.Throw;

        #region Constructor
        
        public OrganizationDbContext(ITenantInfo tenantInfo, IHttpContextAccessor httpContext)
            : base(httpContext)
        {
            TenantInfo = tenantInfo ?? throw new ApplicationException("unknown identifier");
            this.Database.Migrate();
        }
        
        #endregion

        #region Db Sets
        public DbSet<ActionPoint> ActionPoints { get; set; }
        public DbSet<ActionPointSetting> ActionPointSettings { get; set; }
        public DbSet<Blob> Blobs { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<QuestCommit> QuestCommits { get; set; }
        public DbSet<QuestCompletion> QuestCompletions { get; set; }
        public DbSet<QuestGoal> QuestGoals { get; set; }
        public DbSet<QuestGoalCompletion> QuestGoalCompletions { get; set; }
        public DbSet<QuestReward> QuestRewards { get; set; }
        public DbSet<QuestSegment> QuestSegments { get; set; }
        public DbSet<ClaimBundle> ClaimBundles { get; set; }
        public DbSet<ClaimBundleItem> ClaimBundleItems { get; set; }
        public DbSet<ClaimBundleTokenTxn> ClaimBundleTokenTxns { get; set; }
        
        //public DbSet<Date> Dates { get; set; }
        public DbSet<GitCommit> GitCommits { get; set; }

        public DbSet<PullRequest> PullRequests { get; set; }

        public DbSet<PullRequestReview> PullRequestReviews { get; set; }

        public DbSet<PullRequestReviewComment> PullRequestReviewComments { get; set; }

        public DbSet<Integration> Integrations { get; set; }
        public DbSet<IntegrationField> IntegrationFields { get; set; }
        
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemDisenchant> ItemDisenchants { get; set; }
        public DbSet<ItemGift> ItemGifts { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogDevice> LogDevices { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<LogSetting> LogSettings { get; set; }
        public DbSet<LocalTenant> LocalTenants { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileAssignment> ProfileAssignments { get; set; }
        public DbSet<ProfileExternalId> ProfileExternalIds { get; set; }
        public DbSet<ProfileInventoryItem> ProfileInventoryItems { get; set; }
        public DbSet<ProfileLog> ProfileLogs { get; set; }
        public DbSet<ProfileMetric> ProfileMetrics { get; set; }
        public DbSet<ProfilePraise> ProfilePraises { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<SegmentArea> SegmentAreas { get; set; }
        public DbSet<SegmentMetric> SegmentMetrics { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<ShopItemSegment> ShopItemSegments { get; set; }
        public DbSet<ShopLog> ShopLogs { get; set; }
        public DbSet<ShopPurchase> ShopPurchases { get; set; }
        public DbSet<WorkUnit> Tasks { get; set; }
        public DbSet<WorkUnitLog> TaskLogs { get; set; }
        
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMetric> TeamMetrics { get; set; }
        public DbSet<TokenTransaction> TokenTransactions { get; set; }
        public DbSet<WebhookEventLog> WebhookEventLogs { get; set; }

        #endregion
        
        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {   //IArchivableEntity
                if (typeof(IArchivableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddProperty(ArchivedAtProp, typeof(long));
                }
            }
            modelBuilder.ApplyGlobalFilters<IArchivableEntity>(e => EF.Property<long>(e, ArchivedAtProp) == 0);

            modelBuilder.Entity<ActionPointSetting>(entity =>
            {
                entity.HasKey(x => new { x.Type });
            });

            modelBuilder.Entity<QuestCommit>().HasKey(x => new { QuestId = x.QuestId, x.ProfileId });
            modelBuilder.Entity<QuestCompletion>().HasKey(x => new { x.QuestId, x.ProfileId });

            modelBuilder.Entity<QuestGoalCompletion>(entity =>
            {
                entity.HasKey(x => new { x.GoalId, x.ProfileId });
            });

            modelBuilder.Entity<QuestReward>(entity =>
            {
                entity.HasKey(x => new { x.QuestId, x.ItemId });
            });

            modelBuilder.Entity<QuestSegment>(entity =>
            {
                entity.HasKey(x => new { x.QuestId, x.SegmentId });
            });

            modelBuilder.Entity<ClaimBundleItem>().HasKey(x => new { x.ClaimBundleId, x.ProfileInventoryItemId });
            modelBuilder.Entity<ClaimBundleTokenTxn>().HasKey(x => new { x.ClaimBundleId, x.TokenTransactionId });
            
            modelBuilder.Entity<Integration>(entity =>
            {
                entity.HasIndex(x => new { x.ProfileId, x.SegmentId });
            });

            modelBuilder.Entity<LogSetting>(entity =>
            {
                entity.HasKey(x => new { x.LogDeviceId, x.LogEvent });
            });

            modelBuilder.Entity<LocalTenant>(entity =>
            {
                entity.HasKey(x => x.TenantId);
                entity.Property(x => x.TenantId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasIndex(x => x.IdentityId);
                entity.HasIndex(x => x.Username).IsUnique();
                entity.Ignore(x => x.FullName);
            });

            modelBuilder.Entity<ProfileAssignment>(entity =>
            {
                entity.HasIndex(x => new { x.SegmentId, x.TeamId, x.ProfileId }).IsUnique();
                entity.HasIndex(x => new { x.SegmentId, x.ProfileId });
                entity.HasIndex(x => new { x.TeamId, x.ProfileId });
            });

            modelBuilder.Entity<ProfileExternalId>(entity =>
            {
                entity.HasKey(x => new { x.ExternalId, x.IntegrationType, x.SegmentId });
            });

            modelBuilder.Entity<ProfileInventoryItem>(entity =>
            {
                entity.HasIndex(x => new { x.ItemId, x.ProfileId, x.IsActive });
                entity.HasIndex(x => new { x.ProfileId, x.IsActive });
            });

            modelBuilder.Entity<ProfileLog>(entity =>
            {
                entity.HasKey(x => new { x.ProfileId, x.LogId });
                entity.HasIndex(x => new { x.ProfileId, x.Event });
            });

            modelBuilder.Entity<ProfileMetric>(entity =>
            {
                entity.HasKey(x => new { x.ProfileId, x.Type, x.DateId });
                entity.Property(p => p.Type)
                    .HasConversion(
                        p => p.Value,
                        p => MetricType.FromValue(p));

            });

            modelBuilder.Entity<ProfilePraise>().HasKey(x => new { x.DateId, x.ProfileId, x.PraiserProfileId });

            modelBuilder.Entity<Segment>().HasIndex(nameof(Segment.Key), ArchivedAtProp).IsUnique();
            modelBuilder.Entity<SegmentArea>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<SegmentMetric>(entity =>
            {
                entity.HasKey(x => new { x.SegmentId, x.Type, x.DateId });
                entity.Property(p => p.Type)
                    .HasConversion(
                        p => p.Value,
                        p => MetricType.FromValue(p));
            });

            modelBuilder.Entity<ShopItem>(entity =>
            {
                entity.HasIndex(x => x.ItemId).IsUnique();
            });

            modelBuilder.Entity<ShopItemSegment>(entity =>
            {
                entity.HasKey(x => new { x.ShopItemId, x.SegmentId });
            });

            modelBuilder.Entity<ShopLog>().HasKey(x => new { x.ShopId, x.LogId });

            modelBuilder.Entity<ShopPurchase>(entity =>
            {
                entity.HasIndex(x => new { x.ProfileId, x.Status });
            });

            modelBuilder.Entity<WorkUnit>(entity =>
            {
                entity.HasIndex(x => new { x.ExternalId, x.IntegrationType, x.SegmentId }).IsUnique();
                entity.HasIndex(x => new { x.ExternalId, x.IntegrationType });
                entity.HasIndex(x => x.AssigneeProfileId);
            });
            
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(nameof(Team.SegmentId), nameof(Team.Key), ArchivedAtProp).IsUnique();
            });

            modelBuilder.Entity<TeamMetric>(entity =>
            {
                entity.HasKey(x => new { x.TeamId, x.Type, x.DateId });
                entity.Property(p => p.Type)
                    .HasConversion(
                        p => p.Value,
                        p => MetricType.FromValue(p));
            });

            modelBuilder.Ignore<Date>();

            var localTenantEntity = modelBuilder.Model.FindEntityType(typeof(LocalTenant));
            var localTenantPk = localTenantEntity.FindPrimaryKey();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(x => !x.ClrType.HasAttribute<TenantSharedEntityAttribute>()))
            {
                //modelBuilder.Entity(entityType.ClrType).IsMultiTenant();
                //not needed because we have custom EnforceMultiTenantOnLocalTenant
                if (entityType.FindPrimaryKey() == null)
                    continue;

                //TenantId
                var tenantIdProp = entityType.AddProperty(TenantIdFK, new LocalTenant().TenantId.GetType());
                tenantIdProp.IsNullable = false;
                entityType.AddForeignKey(tenantIdProp, localTenantPk, localTenantEntity);
                var pk = entityType.FindPrimaryKey().Properties;
                entityType.SetPrimaryKey(pk.Append(tenantIdProp).ToArray());

                //remove alternatePrimaryKey
                if (pk.Count > 1 || pk[0].Name != nameof(Entity<object>.Id))
                    entityType.RemoveKey(pk);

                var indexes = entityType.GetIndexes().Where(x => x.Properties.Count > 1 || x.Properties[0] != tenantIdProp).ToArray();
                foreach (var idx in indexes)
                {
                    var newIndex = entityType.AddIndex(idx.Properties.Append(tenantIdProp).ToArray());
                    newIndex.IsUnique = idx.IsUnique;
                    entityType.RemoveIndex(idx.Properties);
                }
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<ClaimBundleItem>(entity =>
            {
                entity.HasOne(x => x.ProfileInventoryItem)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(TenantInfo.ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        #endregion


        #region SaveChanges
        
        public override int SaveChanges()
        {
            //this.EnforceMultiTenant();
            this.EnforceMultiTenantOnLocalTenant();
            return base.SaveChanges();
        }
        
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //this.EnforceMultiTenant();
            this.EnforceMultiTenantOnLocalTenant();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        
        public override async System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //this.EnforceMultiTenant();
            this.EnforceMultiTenantOnLocalTenant();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override async System.Threading.Tasks.Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            //this.EnforceMultiTenant();
            this.EnforceMultiTenantOnLocalTenant();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        
        #endregion

        public static void DatabaseEnsureCreatedAndMigrated(string connectionString)
        {
            using var context =
                new OrganizationDbContext(TenantModel.WithConnectionStringOnly(connectionString), null);
            context.Database.Migrate();
        }
    }
}