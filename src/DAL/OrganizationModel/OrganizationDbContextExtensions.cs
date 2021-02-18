using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public static class OrganizationDbContextExtensions
    {
        public static ClaimBundle GetTrackedClaimBundle(this OrganizationDbContext dbContext, Guid profileId, ClaimBundleTypes claimBundleType)
        {
            var trackedClaimBundle = dbContext.ChangeTracker
                .Entries<ClaimBundle>()
                .Where(x => x.State == EntityState.Added)
                .FirstOrDefault(x => x.Entity.ProfileId == profileId && x.Entity.Type == claimBundleType);

            if (trackedClaimBundle == null)
            {
                trackedClaimBundle = dbContext.Add(new ClaimBundle
                {
                    Id = Guid.NewGuid(),
                    ProfileId = profileId,
                    Type = claimBundleType
                });
            }

            return trackedClaimBundle.Entity;
        }
    }
}
