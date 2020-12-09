using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.TaskConverters;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices
{
    public class SyncIssuesLoader : BaseLoader
    {
        #region Private Variables

        private readonly IShardMapProvider _shardMapProvider;

        #endregion

        #region Constructor

        public SyncIssuesLoader(
            IShardMapProvider shardMapProvider,
            LogService logService,
            CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
            _shardMapProvider = shardMapProvider;
        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, JObject requestBody, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                // LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider))
                {
                    PullIssuesNew(organizationDb, date, new TasksService(organizationDb), new ProfilesService(null, null, null, organizationDb), requestBody);
                }
            }
        }

        public static void PullIssuesNew(OrganizationDbContext organizationDb,
                                         DateTime fromDay,
                                         ITasksService tasksService,
                                         IProfilesService profilesService,
                                         JObject requestBody)
        {
            var syncReq = requestBody.ToObject<SyncRequest>();

            if (syncReq?.Params == null
            || !syncReq.Params.TryGetValue("jiraProjectId", out string jiraProjectId))
            {
                throw new ApplicationException("param jiraProjectId not provided");
            }

            var jiraConnector = new AtlassianJiraConnector(null, organizationDb, null);

            Guid? integrationId = IntegrationHelpers.GetIntegrationId(organizationDb, jiraProjectId, IntegrationType.ATJ);
            if (!integrationId.HasValue)
            {
                throw new ApplicationException($"Jira project with Id: {jiraProjectId} is not connected to any tayra segments");
            }
            var tasks = jiraConnector.GetBulkIssuesWithChangelog(integrationId.Value, "status", jiraProjectId);
            foreach (var task in tasks)
            {
                TaskHelpers.DoStandardStuff(new TaskConverterJira(organizationDb, profilesService, task, TaskConverterMode.BULK), tasksService, null, null, null);
            }
            organizationDb.SaveChanges();
        }

        #endregion
    }
}
