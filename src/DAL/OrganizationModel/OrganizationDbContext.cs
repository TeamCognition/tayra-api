using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tayra.Models.Organizations
{
    public class OrganizationDbContext : FirdawsDbContext, IAuditPersistenceStore
    {
        private const string OrganizationIdFK = "OrganizationId";
        private readonly TenantDTO _tenant;

        #region Constructor

        // C'tor to deploy schema and migrations to a new shard
        /// <summary>
        /// Use the protected c'tor with the connection string parameter
        /// to intialize a new shard. 
        /// </summary>
        protected internal OrganizationDbContext(string connectionString)
            : base(ConfigureDbContextOptions(connectionString))
        {
        }
        protected internal OrganizationDbContext(DbContextOptions<OrganizationDbContext> options) : base(options)
        {//deliete this after done with migrations
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
            : base(httpContext, CreateDDRConnection(shardMapProvider.ShardMap, tenantProvider.GetTenant().ShardingKey, shardMapProvider.TemplateConnectionString))
        {
            _tenant = tenantProvider.GetTenant();
            this.Database.Migrate();
        }


        #endregion

        #region Properties

        public int OrganizationId
        {
            get
            {
                if (_tenant == null)
                {
                    return -1;
                }

                return _tenant.ShardingKey;
            }
        }

        #endregion

        #region Db Sets
        public DbSet<ActionPoint> ActionPoints { get; set; }
        public DbSet<ActionPointProfile> ActionPointProfiles { get; set; }
        public DbSet<ActionPointSegment> ActionPointSegments { get; set; }
        public DbSet<ActionPointSetting> ActionPointSettings { get; set; }
        public DbSet<Blob> Blobs { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<ChallengeCompletion> ChallengeCompletions { get; set; }
        public DbSet<ClaimBundle> ClaimBundles { get; set; }
        public DbSet<ClaimBundleItem> ClaimBundleItems { get; set; }
        public DbSet<ClaimBundleTokenTxn> ClaimBundleTokenTxns { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<CompetitionLog> CompetitionLogs { get; set; }
        //public DbSet<Date> Dates { get; set; }
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
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileExternalId> ProfileExternalIds { get; set; }
        public DbSet<ProfileInventoryItem> ProfileInventoryItems { get; set; }
        public DbSet<ProfileLog> ProfileLogs { get; set; }
        public DbSet<ProfileOneUp> ProfileOneUps { get; set; }
        public DbSet<ProfileReportDaily> ProfileReportsDaily { get; set; }
        public DbSet<ProfileReportWeekly> ProfileReportsWeekly { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<SegmentArea> SegmentAreas { get; set; }
        public DbSet<SegmentMember> SegmentMembers { get; set; }
        public DbSet<SegmentReportDaily> SegmentReportsDaily { get; set; }
        public DbSet<SegmentReportWeekly> SegmentReportsWeekly { get; set; }
        public DbSet<SegmentTeam> ProjectTeams { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<ShopItemSegment> ShopItemSegments { get; set; }
        public DbSet<ShopLog> ShopLogs { get; set; }
        public DbSet<ShopPurchase> ShopPurchases { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TeamReportDaily> TeamReportsDaily { get; set; }
        public DbSet<TeamReportWeekly> TeamReportsWeekly { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenTransaction> TokenTransactions { get; set; }
        public DbSet<WebhookEventLog> WebhookEventLogs { get; set; }

        #endregion

        #region Db Stat Sets

        public DbSet<StatType> StatTypes { get; set; }

        #endregion

        #region IAuditPersistenceStore

        public DbSet<EntityChangeLog> EntityChangeLogs { get; set; }

        #endregion

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionPointProfile>(entity =>
            {
                entity.HasKey(x => new { x.ActionPointId, x.ProfileId });
            });

            modelBuilder.Entity<ActionPointSegment>(entity =>
            {
                entity.HasKey(x => new { x.ActionPointId, x.SegmentId });
            });

            modelBuilder.Entity<ActionPointSetting>(entity =>
            {
                entity.HasKey(x => new { x.Type });
            });


            modelBuilder.Entity<ChallengeCompletion>().HasKey(x => new { x.ChallengeId, x.ProfileId });

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


            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Profile>().HasIndex(x => x.Username).IsUnique();

            modelBuilder.Entity<ProfileExternalId>(entity =>
            {
                entity.HasKey(x => new { x.ProfileId, x.SegmentId, x.IntegrationType });
            });

            modelBuilder.Entity<ProfileInventoryItem>(entity =>
            {
                entity.HasIndex(x => new { x.ItemId, x.ProfileId, x.IsActive }); //?
                entity.HasIndex(x => new { x.ProfileId, x.IsActive });
            });

            modelBuilder.Entity<ProfileLog>(entity =>
            {
                entity.HasKey(x => new { x.ProfileId, x.LogId });
                entity.HasIndex(x => new { x.ProfileId, x.Event });
            });

            modelBuilder.Entity<ProfileOneUp>().HasKey(x => new { x.DateId, x.UppedProfileId, x.CreatedBy });

            modelBuilder.Entity<ProfileReportDaily>(entity =>
            {
                entity.HasKey(x => new { x.DateId, x.ProfileId, x.TaskCategoryId });
            });

            modelBuilder.Entity<ProfileReportWeekly>(entity =>
            {
                entity.HasKey(x => new { x.DateId, x.ProfileId, x.TaskCategoryId });
            });

            modelBuilder.Entity<Segment>().HasIndex(x => x.Key).IsUnique();
            modelBuilder.Entity<SegmentArea>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<SegmentMember>().HasKey(x => new { x.SegmentId, x.ProfileId });
            modelBuilder.Entity<SegmentReportDaily>().HasKey(x => new { x.DateId, x.SegmentId, x.TaskCategoryId });
            modelBuilder.Entity<SegmentReportWeekly>().HasKey(x => new { x.DateId, x.SegmentId, x.TaskCategoryId });
            modelBuilder.Entity<SegmentTeam>().HasKey(x => new { x.SegmentId, x.TeamId });

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
                entity.HasIndex(x => new { x.ExternalId, x.IntegrationType }).IsUnique();
                entity.HasIndex(x => x.AssigneeProfileId);
            });

            modelBuilder.Entity<TaskCategory>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(x => new { x.Key, x.ArchivedAt }).IsUnique();
            });

            modelBuilder.Entity<TeamMember>().HasKey(x => new { x.TeamId, x.ProfileId });

            modelBuilder.Entity<TeamReportDaily>(entity =>
            {
                entity.HasKey(x => new { x.DateId, x.TeamId, x.TaskCategoryId });
            });

            modelBuilder.Entity<TeamReportWeekly>(entity =>
            {
                entity.HasKey(x => new { x.DateId, x.TeamId, x.TaskCategoryId });
            });

            modelBuilder.Ignore<Date>();

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            var orgEntity = modelBuilder.Model.FindEntityType(typeof(Organization));
            var orgPKey = orgEntity.FindPrimaryKey();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(x => !x.ClrType.HasAttribute<TenantSharedEntityAttribute>()))
            {
                var id = entityType.GetProperties().FirstOrDefault(x => x.IsPrimaryKey() && x.Name == "Id");
                if (id != null) id.ValueGenerated = ValueGenerated.OnAdd;

                var orgId = entityType.GetOrAddProperty(OrganizationIdFK, typeof(int));
                entityType.GetOrAddForeignKey(orgId, orgPKey, orgEntity);
                entityType.SetPrimaryKey(entityType.FindPrimaryKey().Properties.Append(orgId).ToList());

                var clrType = entityType.ClrType;
                var method = SetGlobalQueryMethod.MakeGenericMethod(clrType);
                method.Invoke(this, new object[] { modelBuilder });
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                && !e.Entity.GetType().HasAttribute<TenantSharedEntityAttribute>()))
            {
                entry.Property(OrganizationIdFK).CurrentValue = OrganizationId;
            }

            return base.SaveChanges();
        }

        #endregion

        static readonly MethodInfo SetGlobalQueryMethod = typeof(OrganizationDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                                                      .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : class
        {
            //Debug.WriteLine("Adding global query for: " + typeof(T));
            builder.Entity<T>().HasQueryFilter(e => EF.Property<int>(e, OrganizationIdFK) == _tenant.ShardingKey);
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
            // Ask shard map to broker a validated connection for the given key
            SqlConnection sqlConn = shardMap.OpenConnectionForKey(shardingKey, connectionStr);

            //// Set TenantId in SESSION_CONTEXT to shardingKey to enable Row-Level Security filtering
            //SqlCommand cmd = sqlConn.CreateCommand();
            //cmd.CommandText = @"exec sp_set_session_context @key=N'OrganizationId', @value=@shardingKey";
            //cmd.Parameters.AddWithValue("@shardingKey", shardingKey);
            //cmd.ExecuteNonQuery();

            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();
            var options = optionsBuilder.UseSqlServer(sqlConn).Options;
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();

            return options;
        }

        private static DbContextOptions ConfigureDbContextOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();
            return optionsBuilder.UseSqlServer(connectionString).Options;
        }
        //TODO: convert these two to use OnConfiguring, what will happen to migrations
    }
}
