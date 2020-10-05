using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Cog.Core;
using Cog.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tayra.Analytics;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class OrganizationDbContext : CogDbContext, IAuditPersistenceStore
    {
        #region Constructor

        // C'tor to deploy schema and migrations to a new shard
        /// <summary>
        /// Use the protected c'tor with the connection string parameter
        /// to intialize a new shard. 
        /// </summary>
        protected internal OrganizationDbContext(string connectionString)
        {
            DirectConnectionString = connectionString;
        }


        // C'tor for data dependent routing. This call will open a validated connection routed to the proper
        // shard by the shard map manager. Note that the base class c'tor call will fail for an open connection
        // if migrations need to be done and SQL credentials are used. This is the reason for the 
        // separation of c'tors into the DDR case (this c'tor) and the internal c'tor for new shards.
        /// <summary>
        /// Use this public c'tor with the shard map parameter in
        // the regular application calls with a tenant id.
        /// </summary>
        public OrganizationDbContext(IHttpContextAccessor httpContext, ITenantProvider tenantProvider, IShardMapProvider shardMapProvider)
            : base(tenantProvider.GetTenant(), httpContext)
        {
            ShardMapProvider = shardMapProvider;
            this.Database.Migrate();
        }

        public string DirectConnectionString { get; set; }
        protected IShardMapProvider ShardMapProvider { get; set; }

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
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<CompetitionLog> CompetitionLogs { get; set; }
        //public DbSet<Date> Dates { get; set; }
        public DbSet<GitCommit> GitCommits { get; set; }
        public DbSet<Integration> Integrations { get; set; }
        public DbSet<IntegrationField> IntegrationFields { get; set; }
        public DbSet<CompetitionReward> CompetitionRewards { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<CompetitorScore> CompetitorScores { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemDisenchant> ItemDisenchants { get; set; }
        public DbSet<ItemGift> ItemGifts { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogDevice> LogDevices { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<LogSetting> LogSettings { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileAssignment> ProfileAssignments { get; set; }
        public DbSet<ProfileExternalId> ProfileExternalIds { get; set; }
        public DbSet<ProfileInventoryItem> ProfileInventoryItems { get; set; }
        public DbSet<ProfileLog> ProfileLogs { get; set; }
        public DbSet<ProfileMetric> ProfileMetrics { get; set; }
        public DbSet<ProfilePraise> ProfilePraises { get; set; }
        public DbSet<ProfileReportDaily> ProfileReportsDaily { get; set; }
        public DbSet<ProfileReportWeekly> ProfileReportsWeekly { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<SegmentArea> SegmentAreas { get; set; }
        public DbSet<SegmentMetric> SegmentMetrics { get; set; }
        public DbSet<SegmentReportDaily> SegmentReportsDaily { get; set; }
        public DbSet<SegmentReportWeekly> SegmentReportsWeekly { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<ShopItemSegment> ShopItemSegments { get; set; }
        public DbSet<ShopLog> ShopLogs { get; set; }
        public DbSet<ShopPurchase> ShopPurchases { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }
        public DbSet<TaskSync> TaskSyncs { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMetric> TeamMetrics { get; set; }
        public DbSet<TeamReportDaily> TeamReportsDaily { get; set; }
        public DbSet<TeamReportWeekly> TeamReportsWeekly { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenTransaction> TokenTransactions { get; set; }
        public DbSet<WebhookEventLog> WebhookEventLogs { get; set; }

        #endregion

        #region IAuditPersistenceStore

        public DbSet<EntityChangeLog> EntityChangeLogs { get; set; }

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

            modelBuilder.Entity<CompetitionLog>(entity =>
            {
                entity.HasKey(x => new { x.CompetitionId, x.LogId });
                entity.HasIndex(x => new { x.CompetitionId, x.Event });
            });

            modelBuilder.Entity<Competitor>(entity =>
            {
                entity.HasIndex(x => new { x.CompetitionId, x.ProfileId, x.TeamId }).IsUnique();
                entity.HasIndex(x => new { x.CompetitionId, x.ProfileId }).IsUnique();
                entity.HasIndex(x => new { x.CompetitionId, x.TeamId }).IsUnique();
            });

            modelBuilder.Entity<CompetitorScore>(entity =>
            {
                entity.HasIndex(x => new { x.CompetitorId, x.ProfileId });
                entity.HasIndex(x => x.ProfileId);
                entity.HasIndex(x => new { x.CompetitorId, x.TeamId });
            });

            modelBuilder.Entity<Integration>(entity =>
            {
                entity.HasIndex(x => new { x.ProfileId, x.SegmentId });
            });

            modelBuilder.Entity<LogSetting>(entity =>
            {
                entity.HasKey(x => new { x.LogDeviceId, x.LogEvent });
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasIndex(x => x.IdentityId);
                entity.HasIndex(x => x.Username).IsUnique();
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

            modelBuilder.Entity<ProfileReportDaily>(entity =>
            {
                entity.HasKey(x => new { x.DateId, x.ProfileId, x.SegmentId, x.TaskCategoryId });
            });

            modelBuilder.Entity<ProfileReportWeekly>(entity =>
            {
                entity.HasKey(x => new { x.DateId, x.ProfileId, x.SegmentId, x.TaskCategoryId });
            });

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
            modelBuilder.Entity<SegmentReportDaily>().HasKey(x => new { x.DateId, x.SegmentId, x.TaskCategoryId });
            modelBuilder.Entity<SegmentReportWeekly>().HasKey(x => new { x.DateId, x.SegmentId, x.TaskCategoryId });

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

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasIndex(x => new { x.ExternalId, x.IntegrationType, x.SegmentId }).IsUnique();
                entity.HasIndex(x => new { x.ExternalId, x.IntegrationType });
                entity.HasIndex(x => x.AssigneeProfileId);
            });

            modelBuilder.Entity<TaskCategory>().HasIndex(x => x.Name).IsUnique();

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
            
            modelBuilder.Entity<TeamReportDaily>(entity =>
            {
                entity.HasKey(x => new { x.DateId, x.TeamId, x.TaskCategoryId });
            });

            modelBuilder.Entity<TeamReportWeekly>(entity =>
            {
                entity.HasKey(x => new { x.DateId, x.TeamId, x.TaskCategoryId });
            });

            modelBuilder.Ignore<Date>();
            
            var orgEntity = modelBuilder.Model.FindEntityType(typeof(Organization));
            var orgPKey = orgEntity.FindPrimaryKey();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(x => !x.ClrType.HasAttribute<TenantSharedEntityAttribute>()))
            {
                if (entityType.FindPrimaryKey() == null)
                    continue;

                //OrganizationId
                var id = entityType.FindPrimaryKey().Properties.FirstOrDefault(x => x.Name == "Id");
                if (id != null) id.ValueGenerated = ValueGenerated.OnAdd;

                var orgId = entityType.AddProperty(TenantIdFK, typeof(int));
                entityType.AddForeignKey(orgId, orgPKey, orgEntity);
                var pk = entityType.FindPrimaryKey().Properties;
                entityType.SetPrimaryKey(pk.Append(orgId).ToArray());

                //remove alternatePrimaryKey
                if (pk.Count() > 1 || pk[0].Name != "Id")
                    entityType.RemoveKey(pk);

                var idxs = entityType.GetIndexes().Where(x => x.Properties.Count() > 1 || x.Properties[0] != orgId).ToArray();
                foreach (var idx in idxs)
                {
                    var newIndex = entityType.AddIndex(idx.Properties.Append(orgId).ToArray());
                    newIndex.IsUnique = idx.IsUnique;
                    entityType.RemoveIndex(idx.Properties);
                }

                //Set Global Query
                var clrType = entityType.ClrType;
                var method = SetGlobalQueryMethod.MakeGenericMethod(clrType);
                method.Invoke(this, new object[] { modelBuilder });
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
            if (!string.IsNullOrEmpty(DirectConnectionString))
            {
                optionsBuilder.UseSqlServer(DirectConnectionString);
            }
            else
            {
                SqlConnection sqlConn = null;
                try
                {
                    // Ask shard map to broker a validated connection for the given key
                    sqlConn = ShardMapProvider.ShardMap.OpenConnectionForKey(CurrentTenant.ShardingKey, ShardMapProvider.TemplateConnectionString);
                    sqlConn.Close(); //this lets ef core handle connection instead of manual

                    optionsBuilder.UseSqlServer(sqlConn);

                    optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>(); //TODO: this goes to CogDB as well?
                }
                catch (Exception e)
                {
                    if (sqlConn != null)
                        sqlConn?.Close();
                    throw new CogSecurityException(e.Message, "OrganizationDbcontext.CreateDDRConnection");
                }
            }
            base.OnConfiguring(optionsBuilder);
        }

        #endregion

        static readonly MethodInfo SetGlobalQueryMethod = typeof(OrganizationDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                                                      .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : class
        {
            //if (CurrentTenantId <= 0)//uncomment for tests
            //    return;

            if (typeof(IArchivableEntity).IsAssignableFrom(typeof(T)))
            {
                builder.Entity<T>().HasQueryFilter(e =>
                    EF.Property<long>(e, ArchivedAtProp) == 0 &&
                    EF.Property<int>(e, TenantIdFK) == CurrentTenant.ShardingKey);
            }
            else
            {
                builder.Entity<T>().HasQueryFilter(e => EF.Property<int>(e, TenantIdFK) == CurrentTenant.ShardingKey);
            }
        }

        /// <summary>
        /// Creates the DDR (Data Dependent Routing) connection.
        /// </summary>
        /// <param name="shardMap">The shard map.</param>
        /// <param name="shardingKey">The sharding key.</param>
        /// <param name="connectionStr">The Template connection string.</param>
        /// <returns></returns>
        // Only static methods are allowed in calls into base class c'tors
        private static DbContextOptions CreateDDRConnection(ShardMap shardMap, int shardingKey, string connectionStr)
        {
            try
            {
                // Ask shard map to broker a validated connection for the given key
                SqlConnection sqlConn = shardMap.OpenConnectionForKey(shardingKey, connectionStr);


                //// Set TenantId in SESSION_CONTEXT to shardingKey to enable Row-Level Security filtering
                //SqlCommand cmd = sqlConn.CreateCommand();
                //cmd.CommandText = @"exec sp_set_session_context @key=N'OrganizationId', @value=@shardingKey";
                //cmd.Parameters.AddWithValue("@shardingKey", shardingKey);
                //cmd.ExecuteNonQuery();

                var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();
                var options = optionsBuilder.UseSqlServer(sqlConn).Options;
                optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>(); //TODO: this goes to CogDB as well?

                return options;
            }
            catch (Exception e)
            {
                throw new CogSecurityException(e.Message, "OrganizationDbcontext.CreateDDRConnection");
            }
        }

        private static DbContextOptions ConfigureDbContextOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();
            return optionsBuilder.UseSqlServer(connectionString).Options;
        }
        //TODO: convert these two to use OnConfiguring, what will happen to migrations
    }
}