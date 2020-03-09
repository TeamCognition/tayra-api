using System;
using System.Linq;
using Firdaws.Core;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices
{
    public class SyncIssuesLoader : BaseLoader
    {
        #region Private Variables

        private readonly IShardMapProvider _shardMapProvider;

        #endregion

        #region Constructor

        public SyncIssuesLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
            _shardMapProvider = shardMapProvider;
        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Name);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Name), _shardMapProvider))
                {
                    PullIssues(organizationDb, date, LogService);
                }
            }
        }

        public static void PullIssues(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            //var jiraConnector = new AtlassianJiraConnector(null, organizationDb);

            //var tasks = jiraConnector.GetBulkIssuesWithChangelog()
        }

        #endregion
    }
}
