using System;
using System.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public static class IntegrationHelpers
    {
        public static Guid? GetIntegrationId(OrganizationDbContext organizationDb, string projectId, IntegrationType type)
        {
            if (type == IntegrationType.ATJ)
            {
                var rewardStatusField = organizationDb
                                    .IntegrationFields
                                    .OrderByDescending(x => x.Created)
                                    .FirstOrDefault(x => x.Key == ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + projectId);
                return rewardStatusField?.IntegrationId;
            }
            throw new ArgumentException("Integration type not supported");
        }
    }
}