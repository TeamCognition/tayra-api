using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Finbuckle.MultiTenant;
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
        #region Constructor

        public SyncIssuesLoader(
            LogService logService,
            CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, JObject requestBody, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                // LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(TenantModel.WithConnectionStringOnly(tenant.ConnectionString), null))
                {
                    PullIssuesNew(organizationDb, date, new TasksService(organizationDb), requestBody);
                }
            }
        }

        public static void PullIssuesNew(OrganizationDbContext organizationDb,
                                         DateTime fromDay,
                                         ITasksService tasksService,
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
                TaskHelpers.DoStandardStuff(new TaskConverterJira(organizationDb, task, TaskConverterMode.BULK), tasksService, null, null, null);
            }
            organizationDb.SaveChanges();
        }

        #endregion
    }
}
