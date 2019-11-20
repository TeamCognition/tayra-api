using System.Data.SqlClient;
using System.Linq;
using Firdaws.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class OrganizationDbContext : FirdawsDbContext, IAuditPersistenceStore
    {
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

        // C'tor for data dependent routing. This call will open a validated connection routed to the proper
        // shard by the shard map manager. Note that the base class c'tor call will fail for an open connection
        // if migrations need to be done and SQL credentials are used. This is the reason for the 
        // separation of c'tors into the DDR case (this c'tor) and the internal c'tor for new shards.
        /// <summary>
        /// Use this public c'tor with the shard map parameter in
        // the regular application calls with a tenant id.
        /// </summary>
        public OrganizationDbContext(ShardMap shardMap, int shardingKey, string connectionString)
            : base(CreateDDRConnection(shardMap, shardingKey, connectionString))
        {
        }

        public OrganizationDbContext(IHttpContextAccessor httpContext, ShardMap shardMap, int shardingKey, string connectionStr)
            : base(httpContext, CreateDDRConnection(shardMap, shardingKey, connectionStr))
        {
        }


        #endregion

        #region Properties

        #endregion

        #region Db Sets

        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<ChallengeCompletion> ChallengeCompletions { get; set; }
        public DbSet<ClaimBundle> ClaimBundles { get; set; }
        public DbSet<ClaimBundleItem> ClaimBundleItems { get; set; }
        public DbSet<ClaimBundleTokenTxn> ClaimBundleTokenTxns { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<CompetitionLog> CompetitionLogs { get; set; }
        //public DbSet<Date> Dates { get; set; }
        public DbSet<CompetitionReward> CompetitionRewards { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<CompetitorScore> CompetitorScores { get; set; }
        public DbSet<Identity> Identities { get; set; }
        public DbSet<IdentityEmail> IdentityEmails { get; set; }
        public DbSet<IdentityExternalId> IdentityExternalIds { get; set; }
        public DbSet<Integration> Integrations { get; set; }
        public DbSet<IntegrationField> IntegrationFields { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemDisenchant> ItemDisenchants { get; set; }
        public DbSet<ItemGift> ItemGifts { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileInventoryItem> ProfileInventoryItems { get; set; }
        public DbSet<ProfileLog> ProfileLogs { get; set; }
        public DbSet<ProfileOneUp> ProfileOneUps { get; set; }
        public DbSet<ProfileReportDaily> ProfileReportsDaily { get; set; }
        public DbSet<ProfileReportWeekly> ProfileReportsWeekly { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectArea> ProjectAreas { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProjectReportDaily> ProjectReportsDaily { get; set; }
        public DbSet<ProjectReportWeekly> ProjectReportsWeekly { get; set; }
        public DbSet<ProjectTeam> ProjectTeams { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<ShopItemProject> ShopItemProjects { get; set; }
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
            modelBuilder.Entity<Identity>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<Profile>().HasIndex(x => x.Nickname).IsUnique();

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

            modelBuilder.Entity<IdentityEmail>(entity =>
            {
                entity.HasKey(x => new { x.IdentityId, x.Email });
                entity.HasIndex(x => x.Email).IsUnique();
                entity.HasIndex(x => new { x.IdentityId, x.IsPrimary }).IsUnique();
            });

            modelBuilder.Entity<IdentityExternalId>(entity =>
            {
                entity.HasKey(x => new { x.IdentityId, x.IntegrationType });
                entity.HasIndex(x => x.ExternalId).IsUnique();
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();
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

            modelBuilder.Entity<Project>().HasIndex(x => x.Key).IsUnique();
            modelBuilder.Entity<ProjectArea>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<ProjectMember>().HasKey(x => new { x.ProjectId, x.ProfileId });
            modelBuilder.Entity<ProjectReportDaily>().HasKey(x => new { x.DateId, x.ProjectId, x.TaskCategoryId });
            modelBuilder.Entity<ProjectReportWeekly>().HasKey(x => new { x.DateId, x.ProjectId, x.TaskCategoryId });
            modelBuilder.Entity<ProjectTeam>().HasKey(x => new { x.ProjectId, x.TeamId });

            modelBuilder.Entity<ShopItem>(entity =>
            {
                entity.HasIndex(x => x.ItemId).IsUnique();
            });

            modelBuilder.Entity<ShopItemProject>(entity =>
            {
                entity.HasKey(x => new { x.ShopItemId, x.ProjectId });
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

            
            Seed(modelBuilder);
            

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Seed

        private void Seed(ModelBuilder modelBuilder)
        {
            var cToken = new Token { Id = 1, Name = "Company Token", Symbol = "CT", Type = TokenType.CompanyToken };
            var expToken = new Token { Id = 2, Symbol = "EXP", Name = nameof(TokenType.Experience), Type = TokenType.Experience };
            var p1Token = new Token { Id = 3, Symbol = "1Up", Name = nameof(TokenType.OneUp), Type = TokenType.OneUp };

            modelBuilder.Entity<Token>().HasData(cToken);
            modelBuilder.Entity<Token>().HasData(expToken);
            modelBuilder.Entity<Token>().HasData(p1Token);

            var shop = new Shop { Id = 1, Name = "Employee Shop" };
            modelBuilder.Entity<Shop>().HasData(shop);

            var taskCategory = new TaskCategory { Id = 1, Name = "Undefined" };
            modelBuilder.Entity<TaskCategory>().HasData(taskCategory);

            #endregion
        }

        /// <summary>
        /// Creates the DDR (Data Dependent Routing) connection.
        /// </summary>
        /// <param name="shardMap">The shard map.</param>
        /// <param name="shardingKey">The sharding key.</param>
        /// <param name="connectionStr">The connection string.</param>
        /// <returns></returns>
        // Only static methods are allowed in calls into base class c'tors
        private static DbContextOptions CreateDDRConnection(ShardMap shardMap, int shardingKey, string connectionStr)
        {
            // Ask shard map to broker a validated connection for the given key
            SqlConnection sqlConn = shardMap.OpenConnectionForKey(shardingKey, connectionStr);

            // Set TenantId in SESSION_CONTEXT to shardingKey to enable Row-Level Security filtering
            SqlCommand cmd = sqlConn.CreateCommand();
            cmd.CommandText = @"exec sp_set_session_context @key=N'TenantId', @value=@shardingKey";
            cmd.Parameters.AddWithValue("@shardingKey", shardingKey);
            cmd.ExecuteNonQuery();

            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();
            var options = optionsBuilder.UseSqlServer(sqlConn).Options;

            return options;
        }

        private static DbContextOptions ConfigureDbContextOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();
            return optionsBuilder.UseSqlServer(connectionString).Options;
        }
    }
}
