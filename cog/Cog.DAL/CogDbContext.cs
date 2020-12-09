using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Cog.DAL
{
    public class CogDbContext : DbContext
    {
        protected const string AuditCreatedProp = nameof(ITimeStampedEntity.Created);
        protected const string AuditCreatedByProp = nameof(IUserStampedEntity.CreatedBy);
        protected const string AuditLastModifiedProp = nameof(ITimeStampedEntity.LastModified);
        protected const string AuditLastModifiedByProp = nameof(IUserStampedEntity.LastModifiedBy);

        protected const string TenantIdFK = "OrganizationId";
        protected const string ArchivedAtProp = "ArchivedAt";

        public CogDbContext() : this(null)
        {
        }

        public CogDbContext(IHttpContextAccessor httpContext) : this(null, httpContext)
        {
        }

        public CogDbContext(TenantDTO tenant, IHttpContextAccessor httpContext)
        {
            CurrentTenant = tenant;
            Instantiated = DateTime.UtcNow;
            if (httpContext?.HttpContext?.User != null) UserPrincipal = new CogPrincipal(httpContext.HttpContext.User);
        }

        public void SetTimeStamps()
        {
            var timestamp = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<ITimeStampedEntity>())
            {
                if (entry.State == EntityState.Modified) entry.Property(AuditLastModifiedProp).CurrentValue = timestamp;

                if (entry.State == EntityState.Added)
                {
                    /* FOR DEMO ONLY */
                    var timestamp2 = (DateTime) entry.Property(AuditCreatedProp).CurrentValue;
                    if (timestamp2 == DateTime.MinValue) timestamp2 = DateTime.UtcNow;
                    entry.Property(AuditCreatedProp).CurrentValue = timestamp2;
                }
            }
        }

        public void SetUserStamps(CogPrincipal userSession)
        {
            foreach (var entry in ChangeTracker.Entries<IUserStampedEntity>())
            {
                if (entry.State == EntityState.Modified)
                    entry.Property(AuditLastModifiedByProp).CurrentValue = userSession.ProfileId;

                if (entry.State == EntityState.Added)
                    entry.Property(AuditCreatedByProp).CurrentValue = userSession.ProfileId;
            }
        }

        private void HandleTenantEntities()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added
                            && !e.Entity.GetType().HasAttribute<TenantSharedEntityAttribute>()))
                entry.Property(TenantIdFK).CurrentValue = CurrentTenantId;
        }

        private void HandleSoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries<IArchivableEntity>())
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property(ArchivedAtProp).CurrentValue = DateHelper2.GetCurrentUnixTimestamp();
                }
        }

        public override int SaveChanges()
        {
            SetTimeStamps();
            HandleTenantEntities();
            HandleSoftDelete();

            if (UserPrincipal != null) SetUserStamps(UserPrincipal);

            if (CurrentTenant != null) HandleTenantEntities();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimeStamps();
            HandleTenantEntities();
            HandleSoftDelete();

            if (UserPrincipal != null) SetUserStamps(UserPrincipal);

            if (CurrentTenant != null) HandleTenantEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        #region Properties

        public DateTime Instantiated { get; }

        public CogPrincipal UserPrincipal { get; set; }

        protected TenantDTO CurrentTenant { get; set; }

        public int CurrentTenantId
        {
            get
            {
                if (CurrentTenant == null) return -1;

                return CurrentTenant.ShardingKey;
            }
        }

        #endregion
    }
}