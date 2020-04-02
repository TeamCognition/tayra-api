using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class IntegrationsService : BaseService<OrganizationDbContext>, IIntegrationsService
    {
        #region Constructor

        public IntegrationsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        IQueryable<Integration> ProfileIntegrationsScope(int profileId, int segmentId) => DbContext.Integrations.Where(x => x.ProfileId == profileId && x.SegmentId == segmentId);
        IQueryable<Integration> SegmentIntegrationsScope(int segmentId) => DbContext.Integrations.Where(x => x.ProfileId == null && x.SegmentId == segmentId);

        #region Public Methods

        /// <summary>
        /// USE ONLY IF YOU NEED. VERY SLOW. USE ProfilesService.GetByExternalId instead
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        public int GetProfileIdByExternalId(string externalId)
        {
            return DbContext.IntegrationFields.Where(x => x.Value == externalId).Select(x => x.Integration.SegmentId).FirstOrDefault();
        }

        public void DeleteSegmentIntegration(int profileId, int segmentId, IntegrationType integrationType)
        {
            var integration = ProfileIntegrationsScope(profileId, segmentId)
                                .Include(x => x.Fields)
                                .LastOrDefault(x => x.ProfileId == profileId && x.SegmentId == segmentId && x.Type == integrationType);

            integration.EnsureNotNull(segmentId, integrationType);

            integration.Fields.ToList().ForEach(x => DbContext.Remove(x));
            DbContext.Remove(integration);
        }

        public List<IntegrationProfileConfigDTO> GetProfileIntegrationsWithPending(int[] segmentIds, int profileId)
        {
            var integrations = DbContext.Integrations
                .Where(x => (x.ProfileId == profileId || x.ProfileId == null) && segmentIds.Contains(x.SegmentId))
                .Select(x => new IntegrationProfileConfigDTO
                {
                    Id = x.Id,
                    SegmentId = x.SegmentId,
                    Type = x.Type,
                    ExternalId = x.ProfileId != null ? x.Fields.Where(e => e.Key == Constants.PROFILE_EXTERNAL_ID).Select(e => e.Value).FirstOrDefault() : null
                })
                .ToList();

            foreach(var i in integrations.Where(x => x.ExternalId == null).ToArray())
            {
                if (integrations.Any(x => x.SegmentId == i.SegmentId && x.ExternalId != null))
                    integrations.Remove(i);
            }
            return integrations;
        }

        public List<IntegrationSegmentViewDTO> GetSegmentIntegrations(string segmentKey)
        {
            var segment = DbContext.Segments.Where(x => x.Key == segmentKey).FirstOrDefault();

            segment.EnsureNotNull(segmentKey);

            return SegmentIntegrationsScope(segment.Id)
                .Select(x => new IntegrationSegmentViewDTO
                {
                    Type = x.Type,
                    Created = x.Created,
                    LastModified = x.LastModified ?? x.Created
                })
                .DistinctBy(x => x.Type)
                .OrderByDescending(x => x.Created)
                .ToList();
        }

        public JiraSettingsViewDTO GetJiraSettingsViewDTO(string webhookServerUrl, string tenantKey, int segmentId)
        {
            var integration = SegmentIntegrationsScope(segmentId)
                                .Include(x => x.Fields)
                                .LastOrDefault(x => x.Type == IntegrationType.ATJ);

            if (integration == null)
            {
                throw new ApplicationException("No Jira integration associated with segment " + segmentId);
            }

            var jiraConnector = new AtlassianJiraConnector(null, DbContext);

            var allProjects = jiraConnector.GetProjects(integration.Id);
            foreach (var x in allProjects)
            {
                x.Statuses = jiraConnector.GetIssueStatuses(integration.Id, x.Id);
            }

            var jiraProjects = integration.Fields.Where(x => x.Key == ATConstants.ATJ_PROJECT_ID);
            var rewardStatus = integration.Fields.Where(x => x.Key.StartsWith(ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_, StringComparison.InvariantCulture));

            var activeProjects = jiraProjects
                .Select(p => new ActiveProject(p.Value, rewardStatus.FirstOrDefault(x => x.Key.Contains(p.Value)).Value));


            var atSiteName = integration.Fields.FirstOrDefault(x => x.Key == ATConstants.AT_SITE_NAME)?.Value;
            //TODO: check if data is valid with jira
            //TODO: crashes if not all projects have reward status
            return new JiraSettingsViewDTO
            {
                JiraWebhookSettingsUrl = $"https://{atSiteName}.atlassian.net/plugins/servlet/webhooks",
                WebhookUrl = $"https://{webhookServerUrl}/webhooks/atjissueupdate?tenant={tenantKey}",
                AllProjects = allProjects,
                ActiveProjects = activeProjects.ToList()
            };
        }

        public async void UpdateJiraSettingsWithSaveChanges(int segmentId, string organizationKey, JiraSettingsUpdateDTO dto)
        {
            var integration = SegmentIntegrationsScope(segmentId)
                                .Include(x => x.Fields)
                                .LastOrDefault(x => x.Type == IntegrationType.ATJ);

            if (integration == null)
            {
                throw new ApplicationException("No Jira integration associated with segment " + segmentId);
            }

            var fields = integration.Fields
                .Where(x => x.Key == ATConstants.ATJ_PROJECT_ID || x.Key.StartsWith(ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_, StringComparison.InvariantCulture));

            var newProjects = dto.ActiveProjects.ExceptBy(fields.Where(x => x.Key == ATConstants.ATJ_PROJECT_ID).Select(x => new ActiveProject(x.Value, string.Empty)), e => e.ProjectId).ToArray();

            fields.ToList().ForEach(x => DbContext.Remove(x));

            foreach (var s in dto.ActiveProjects)
            {
                var projId = s.ProjectId;
                var rewardStatus = s.RewardStatusId;
                if (projId == null || rewardStatus == null)
                {
                    throw new ApplicationException("projectId or rewardStatusId is null"); //use nameOf
                }
                integration.Fields.Add(new IntegrationField { Key = ATConstants.ATJ_PROJECT_ID, Value = projId });
                integration.Fields.Add(new IntegrationField { Key = ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + projId, Value = rewardStatus });
            }

            DbContext.SaveChanges();

            if(dto.PullTasksForNewProjects)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://tayra-sync.azurewebsites.net/");
                    client.DefaultRequestHeaders.Add("x-functions-key", "xLVyFfJSbfPl5S9XEuP5heqms1XxO4XzxCzZ81NYXFLy9ZZWOliKxg==");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    foreach (var p in newProjects)
                    {
                        //TODO:make real class instead of anonymous object
                        await client.PostAsync("api/SyncIssuesHttp", new StringContent(JsonConvert.SerializeObject(new { tenantKey = organizationKey, @params = new { jiraProjectId = p.ProjectId } }), Encoding.UTF8, "application/json"));
                    }
                }
            }
        }

        #endregion
    }
}