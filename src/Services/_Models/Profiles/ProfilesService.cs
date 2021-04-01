using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.Services.Models.Profiles
{
    public class ProfilesService
    { 
        public async Task<ProfilePulse> GetProfilePulseDTO(OrganizationDbContext db, Guid profileId, CancellationToken token)
        {
            var yesterdayDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(-1));
            var tasks = await (from t in db.Tasks
                         where t.AssigneeProfileId == profileId
                         where t.Status == WorkUnitStatuses.InProgress || (t.Status == WorkUnitStatuses.Done && t.LastModifiedDateId >= yesterdayDateId)
                         select new
                         {
                             DTO = new ProfilePulse.Task
                             {
                                 Status = t.Status,
                                 Summary = t.Summary,
                                 ExternalUrl = t.ExternalUrl
                             },
                             LastModifiedDateId = t.LastModifiedDateId
                         }).ToArrayAsync(token);

            string jiraBoardUrl = null;
            var segmentId = db.ProfileAssignments.FirstOrDefault(x => x.ProfileId == profileId)?.SegmentId;
            if (segmentId != null)
            {
                var sFields = db.Integrations
                    .Where(x => x.SegmentId == segmentId && x.ProfileId == null && x.Type == IntegrationType.ATJ)
                    .Select(x => x.Fields).FirstOrDefault();

                if (sFields != null)
                {
                    var jiraSiteName = sFields.FirstOrDefault(x => x.Key == ATConstants.AT_SITE_NAME)?.Value;
                    var projectKey = sFields.FirstOrDefault(x => x.Key.StartsWith(ATConstants.ATJ_KEY_FOR_PROJECT_))?.Value;
                    jiraBoardUrl = $"https://{jiraSiteName}.atlassian.net/browse/{projectKey}";
                }
            }

            return new ProfilePulse
            {
                InProgress = tasks.Select(x => x.DTO).Where(x => x.Status == WorkUnitStatuses.InProgress).ToArray(),
                RecentlyDone = tasks.Select(x => x.DTO).Where(x => x.Status == WorkUnitStatuses.Done).ToArray(),
                JiraBoardUrl = jiraBoardUrl
            };
        }

        public class ProfileActiveItemsDTO
        {
            public IList<ItemActiveDTO> Badges { get; set; }
            public ItemActiveDTO Title { get; set; }
            public ItemActiveDTO Border { get; set; }
        }
        
        public ProfileActiveItemsDTO GetProfileActiveItems(OrganizationDbContext dbContext, Guid profileId)
        {
            var activeItems = (from ii in dbContext.ProfileInventoryItems
                where ii.ProfileId == profileId
                //where !ii.ClaimRequired || ii.ClaimedAt.HasValue
                where ii.IsActive
                select new ItemActiveDTO
                {
                    InventoryItemId = ii.Id,
                    Name = ii.Item.Name,
                    Image = ii.Item.Image,
                    Type = ii.Item.Type,
                    Rarity = ii.Item.Rarity
                }).ToList();

            return new ProfileActiveItemsDTO
            {
                Badges = activeItems.Where(x => x.Type == ItemTypes.TayraBadge).ToList(),
                Title = activeItems.FirstOrDefault(x => x.Type == ItemTypes.TayraTitle),
                Border = activeItems.FirstOrDefault(x => x.Type == ItemTypes.TayraBorder)
            };
        }
        
        public async Task<bool> IsUsernameUnique(OrganizationDbContext dbContext, string username, CancellationToken token)
        {
            return !(await dbContext.Profiles.AnyAsync(x => x.Username == username, token));
        }
        
        public bool IsUsernameUniqueNonAsync(OrganizationDbContext dbContext, string username)
        {
            return !dbContext.Profiles.Any(x => x.Username == username);
        }
        
        public Profile GetProfileByExternalId(OrganizationDbContext dbContext, string externalId, IntegrationType integrationType)
        {
            var pe = dbContext.ProfileExternalIds.Include(x => x.Profile).FirstOrDefault(x => x.ExternalId == externalId && x.IntegrationType == integrationType);
            return pe?.Profile;
        }
    }
}