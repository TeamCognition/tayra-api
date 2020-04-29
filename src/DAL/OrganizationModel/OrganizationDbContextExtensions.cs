using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public static class OrganizationDbContextExtensions
    {
        public static ClaimBundle GetTrackedClaimBundle(this OrganizationDbContext dbContext, int profileId, ClaimBundleTypes claimBundleType)
        {
            var trackedClaimBundle = dbContext.ChangeTracker.Entries<ClaimBundle>()
                    .Where(x => x.State == EntityState.Added)
                    .Where(x => x.Entity.ProfileId == profileId && x.Entity.Type == claimBundleType)
                    .FirstOrDefault();

            if (trackedClaimBundle == null)
            {
                trackedClaimBundle = dbContext.Add(new ClaimBundle
                {
                    ProfileId = profileId,
                    Type = claimBundleType
                });
            }

            return trackedClaimBundle.Entity;
        }
    }
}
