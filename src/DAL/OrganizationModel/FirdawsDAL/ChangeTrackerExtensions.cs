using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cog.DAL
{
    public static class ChangeTrackerExtensions
    {
        //public static void SetShadowProperties(this ChangeTracker changeTracker, IUserSession userSession)
        //{
        //    changeTracker.DetectChanges();

        //    var timestamp = DateTime.UtcNow;

        //    foreach (var entry in changeTracker.Entries())
        //    {
        //        if (entry.Entity is IAuditedEntityBase)
        //        {
        //            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
        //            {
        //                entry.Property("ModifiedOn").CurrentValue = timestamp;
        //                entry.Property("ModifiedById").CurrentValue = userSession.UserId;
        //            }

        //            if (entry.State == EntityState.Added)
        //            {
        //                entry.Property("CreatedOn").CurrentValue = timestamp;
        //                entry.Property("CreatedById").CurrentValue = userSession.UserId;
        //                if (entry.Entity is ITenant)
        //                {
        //                    entry.Property("TenantId").CurrentValue = userSession.TenantId;
        //                }
        //            }
        //        }

        //        if (entry.State == EntityState.Deleted && entry.Entity is ISoftDelete)
        //        {
        //            entry.State = EntityState.Modified;
        //            entry.Property("IsDeleted").CurrentValue = true;
        //        }
        //    }
        //}
    }
}
