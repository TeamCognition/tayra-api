﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Cog.Core;
using Cog.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
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

        IQueryable<Integration> ProfileIntegrationsScope(Guid profileId, Guid segmentId) => DbContext.Integrations.Where(x => x.ProfileId == profileId && x.SegmentId == segmentId);
        IQueryable<Integration> SegmentIntegrationsScope(Guid segmentId) => DbContext.Integrations.Where(x => x.ProfileId == null && x.SegmentId == segmentId);

        #region Public Methods

        /// <summary>
        /// USE ONLY IF YOU NEED. VERY SLOW. USE ProfilesService.GetByExternalId instead
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        public Guid GetProfileIdByExternalId(string externalId)
        {
            return DbContext.IntegrationFields.Where(x => x.Value == externalId).Select(x => x.Integration.SegmentId).FirstOrDefault();
        }

        public void DeleteSegmentIntegration(Guid profileId, Guid segmentId, IntegrationType integrationType)
        {
            var integration = ProfileIntegrationsScope(profileId, segmentId)
                                .Include(x => x.Fields)
                                .OrderByDescending(x => x.Created)
                                .FirstOrDefault(x => x.ProfileId == profileId && x.SegmentId == segmentId && x.Type == integrationType);

            integration.EnsureNotNull(segmentId, integrationType);

            integration.Fields.ToList().ForEach(x => DbContext.Remove(x));
            DbContext.Remove(integration);
        }

        public List<IntegrationProfileConfigDTO> GetProfileIntegrationsWithPending(Guid[] segmentIds, Guid profileId)
        {
            var integrations = DbContext.Integrations
                .Where(x => (x.ProfileId == profileId || x.ProfileId == null) && segmentIds.Contains(x.SegmentId))
                .Select(x => new IntegrationProfileConfigDTO
                {
                    Id = x.Id,
                    SegmentId = x.SegmentId,
                    Status = x.Status,
                    Type = x.Type,
                    ExternalId = x.ProfileId != null ? x.Fields.Where(e => e.Key == Constants.PROFILE_EXTERNAL_ID).Select(e => e.Value).FirstOrDefault() : null
                })
                .ToList();

            foreach (var i in integrations.Where(x => x.ExternalId == null).ToArray())
            {
                if (integrations.Any(x => x.SegmentId == i.SegmentId && x.Type == i.Type && x.ExternalId != null))
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
                    Status = x.Status,
                    LastModified = x.LastModified ?? x.Created,
                    MembersCount = DbContext.Integrations.Where(y => y.Type == x.Type && y.SegmentId == segment.Id && x.ProfileId != null).GroupBy(y => y.ProfileId).Count()
                })
                .DistinctBy(x => x.Type)
                .OrderByDescending(x => x.Created)
                .ToList();
        }

        public JiraSettingsViewDTO GetJiraSettingsViewDTO(string webhookServerUrl, string tenantKey, Guid segmentId, IConfiguration config)
        {
            var integration = SegmentIntegrationsScope(segmentId)
                                .Include(x => x.Fields)
                                .OrderByDescending(x => x.Created)
                                .FirstOrDefault(x => x.Type == IntegrationType.ATJ);

            if (integration == null)
            {
                throw new ApplicationException("No Jira integration associated with segment " + segmentId);
            }

            var jiraConnector = new AtlassianJiraConnector(null, DbContext, null, config);

            var allProjects = jiraConnector.GetProjects(integration.Id);
            foreach (var x in allProjects)
            {
                x.Statuses = jiraConnector.GetIssueStatuses(integration.Id, x.Id);
            }
            
            // var activeProjects = jiraProjects
            //     .Select(p => new ActiveProject(p.Value, rewardStatus.FirstOrDefault(x => x.Key.Contains(p.Value)).Value));

            var atSiteName = integration.Fields.FirstOrDefault(x => x.Key == ATConstants.AT_SITE_NAME)?.Value;
            //TODO: check if data is valid with jira
            //TODO: crashes if not all projects have reward status
            return new JiraSettingsViewDTO
            {
                JiraWebhookSettingsUrl = $"https://{atSiteName}.atlassian.net/plugins/servlet/webhooks",
                WebhookUrl = $"https://{webhookServerUrl}/webhooks/atjissueupdate?tenant={tenantKey}",
                Projects = AppsProjectConfig.From(allProjects, integration.Fields)
            };
        }

        public async void UpdateJiraSettingsWithSaveChanges(Guid segmentId, string organizationKey, JiraSettingsUpdateDTO dto, IConfiguration config)
        {
            var integration = SegmentIntegrationsScope(segmentId)
                                .Include(x => x.Fields)
                                .OrderByDescending(x => x.Created)
                                .FirstOrDefault(x => x.Type == IntegrationType.ATJ);

            if (integration == null)
            {
                throw new ApplicationException("No Jira integration associated with segment " + segmentId);
            }

            var integrationFields = integration.Fields.Where(x => x.IntegrationId == integration.Id && (
                x.Key == ATConstants.ATJ_PROJECT_ID ||
                x.Key.StartsWith(ATConstants.ATJ_KEY_FOR_PROJECT_) ||
                x.Key.StartsWith(ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_) ||
                x.Key.StartsWith(ATConstants.PM_WORK_UNIT_STATUS_FOR_PROJECT_)
                )).ToArray();
            
            var newProjectIds = dto.ActiveProjects.ExceptBy(integrationFields.Where(x => x.Key == ATConstants.ATJ_PROJECT_ID).Select(x => new SetAppsProjectConfig(x.Value)), e => e.ProjectId).Select(x => x.ProjectId).ToArray();
            
            var jiraConnector = new AtlassianJiraConnector(null, DbContext, null, config);
            var allProjects = jiraConnector.GetProjects(integration.Id);

            //will remove all fields (incl. access token)
            integrationFields.ForEach(x => DbContext.Remove(x));

            foreach (var activeProject in dto.ActiveProjects)
            {
                var project = allProjects.FirstOrDefault(x => x.Id == activeProject.ProjectId);
                var rewardStatus = activeProject.RewardStatusId;
                if (project == null || rewardStatus == null)
                {
                    throw new ApplicationException("projectId or rewardStatusId is null or invalid"); //use nameOf
                }
                integration.Fields.Add(new IntegrationField { Key = ATConstants.ATJ_PROJECT_ID, Value = project.Id });
                integration.Fields.Add(new IntegrationField { Key = ATConstants.ATJ_KEY_FOR_PROJECT_ + project.Id, Value = project.Key });
                integration.Fields.Add(new IntegrationField { Key = ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + project.Id, Value = rewardStatus });

                foreach (var statusIntegrationField in AtlassianJiraWkUnStatusesConfiguration.From(activeProject).ExportToIntegrationFields())
                {
                    integration.Fields.Add(statusIntegrationField);    
                }
            }

            integration.Status = integration.Fields.Any(x => x.Key == ATConstants.ATJ_PROJECT_ID) ? IntegrationStatuses.Connected : IntegrationStatuses.NeedsConfiguration;
            
            DbContext.SaveChanges();
            
            if (dto.PullTasksForNewProjects)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://tayra-sync.azurewebsites.net/");
                    client.DefaultRequestHeaders.Add("x-functions-key", "xLVyFfJSbfPl5S9XEuP5heqms1XxO4XzxCzZ81NYXFLy9ZZWOliKxg==");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    foreach (var pId in newProjectIds)
                    {
                        //TODO:make real class instead of anonymous object
                        await client.PostAsync("api/SyncIssuesHttp", new StringContent(JsonConvert.SerializeObject(new { tenantKey = organizationKey, @params = new { jiraProjectId = pId } }), Encoding.UTF8, "application/json"));
                    }
                }
            }
        }

        #endregion
    }
}