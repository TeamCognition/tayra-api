using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cog.DAL
{
    public class CogDbContext : DbContext
    {
        protected const string AuditCreatedProp = "Created";
        protected const string AuditCreatedByProp = "CreatedBy";
        protected const string AuditLastModifiedProp = "LastModified";
        protected const string AuditLastModifiedByProp = "LastModifiedBy";
        
        protected const string TenantIdFK = "OrganizationId";
        protected const string ArchivedAtProp = "ArchivedAt";


        #region Properties

        public DateTime Instantiated { get; private set; }

        public CogPrincipal UserPrincipal { get; set; }

        protected TenantDTO CurrentTenant { get; set; }

        public int CurrentTenantId
        {
            get
            {
                if (CurrentTenant == null)
                {
                    return -1;
                }

                return CurrentTenant.ShardingKey;
            }
        }

        #endregion

        public CogDbContext(DbContextOptions options) : this(null, options)
        {
        }

        public CogDbContext(IHttpContextAccessor httpContext, DbContextOptions options) : this(null, httpContext, options)
        {
        }

        public CogDbContext(TenantDTO tenant, IHttpContextAccessor httpContext, DbContextOptions options) : base(options)
        {
            CurrentTenant = tenant;
            Instantiated = DateTime.UtcNow;
            if (httpContext?.HttpContext?.User != null)
            {
                UserPrincipal = new CogPrincipal(httpContext.HttpContext.User);
            }
        }

        public void SetTimeStamps()
        {
            var timestamp = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<ITimeStampedEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(AuditLastModifiedProp).CurrentValue = timestamp;
                }

                if (entry.State == EntityState.Added)
                {
                    /* FOR DEMO ONLY */
                    var timestamp2 = (DateTime)entry.Property(AuditCreatedProp).CurrentValue;
                    if (timestamp2 == DateTime.MinValue)
                    {
                        timestamp2 = DateTime.UtcNow;
                    }
                    entry.Property(AuditCreatedProp).CurrentValue = timestamp2;
                }
            }
        }

        public void SetUserStamps(CogPrincipal userSession)
        {
            foreach (var entry in ChangeTracker.Entries<IUserStampedEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(AuditLastModifiedByProp).CurrentValue = userSession.ProfileId;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Property(AuditCreatedByProp).CurrentValue = userSession.ProfileId;
                }
            }
        }

        void HandleTenantEntities()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                && !e.Entity.GetType().HasAttribute<TenantSharedEntityAttribute>()))
            {
                entry.Property(TenantIdFK).CurrentValue = CurrentTenantId;
            }
        }

        void HandleSoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries<IArchivableEntity>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property(ArchivedAtProp).CurrentValue = DateHelper2.GetCurrentUnixTimestamp();
                }
            }
        }

        public override int SaveChanges()
        {
            SetTimeStamps();
            HandleTenantEntities();
            HandleSoftDelete();

            if(UserPrincipal != null)
            {
                SetUserStamps(UserPrincipal);
            }

            if(CurrentTenant != null)
            {
                HandleTenantEntities();
            }

            return base.SaveChanges();
            
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SetTimeStamps();
            HandleTenantEntities();
            HandleSoftDelete();

            if (UserPrincipal != null)
            {
                SetUserStamps(UserPrincipal);
            }

            if (CurrentTenant != null)
            {
                HandleTenantEntities();
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}