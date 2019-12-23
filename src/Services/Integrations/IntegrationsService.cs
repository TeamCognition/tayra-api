using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
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

        IQueryable<Integration> ProfileIntegrationsScope (int profileId, int projectId) => DbContext.Integrations.Where(x => x.ProfileId == profileId && x.ProjectId == projectId);
        IQueryable<Integration> ProjectIntegrationsScope (int projectId) => DbContext.Integrations.Where(x => x.ProfileId == null && x.ProjectId == projectId);

        #region Public Methods

        //we don't need this?
        public void SetProfileIntegration(int profileId, IntegrationProfileConfigDTO dto)
        {
            var integration = ProfileIntegrationsScope(profileId, dto.ProjectId)
                                .Include(x => x.Fields)
                                .LastOrDefault(x => x.Type == dto.Type);

            if (integration == null)
            {
                integration = new Integration
                {
                    ProfileId = profileId,
                    ProjectId = dto.ProjectId,
                    Type = dto.Type,
                    Fields = new List<IntegrationField>()
                };
                DbContext.Add(integration);
            }

            integration.Fields.ToList().ForEach(x => DbContext.Remove(x));

            integration.Fields.Add(new IntegrationField { Key = IntegrationConstants.ProfileExternalId, Value = dto.ExternalId });
        }

        public void DeleteProfileIntegration(int profileId, int projectId, IntegrationType integrationType)
        {
            var integration = ProfileIntegrationsScope(profileId, profileId)
                                .Include(x => x.Fields)
                                .LastOrDefault(x => x.ProfileId == profileId && x.ProjectId == projectId && x.Type == integrationType);

            integration.EnsureNotNull(projectId, integrationType);

            integration.Fields.ToList().ForEach(x => DbContext.Remove(x));
            DbContext.Remove(integration);
        }

        public List<IntegrationProfileConfigDTO> GetProfileIntegrationsWithPending(int profileId)
        {
            //could this be taken out of access token?
            var profileProjectIds = DbContext.ProjectMembers.Where(x => x.ProfileId == profileId).Select(x => x.ProjectId).ToList();

            return DbContext.Integrations
                .Where(x => (x.ProfileId == profileId || x.ProfileId == null) && profileProjectIds.Contains(x.ProjectId))
                .Select(x => new IntegrationProfileConfigDTO
                {
                    ProjectId = x.ProjectId,
                    Type = x.Type,
                    ExternalId = x.Fields.Where(e => e.Key == IntegrationConstants.ProfileExternalId).Select(e => e.Value).FirstOrDefault()
                })
                .ToList();
        }

        public List<IntegrationProjectViewDTO> GetProjectIntegrations(int projectId)
        {
            return ProjectIntegrationsScope(projectId)
                .Select(x => new IntegrationProjectViewDTO
                {
                    Type = x.Type,
                    Created = x.Created,
                    LastModified = x.LastModified ?? x.Created,
                    CreatedBy = x.CreatedBy
                })
                .DistinctBy(x => x.Type)
                .OrderByDescending(x => x.Created)
                .ToList();
        }

        public JiraSettingsViewDTO GetJiraSettingsViewDTO(int projectId)
        {
            var integration = ProjectIntegrationsScope(projectId)
                                .Include(x => x.Fields)
                                .LastOrDefault(x => x.Type == IntegrationType.ATJ);

            if (integration == null)
            {
                throw new ApplicationException("No Jira integration associated with project " + projectId);
            }

            var jiraConnector = new AtlassianJiraConnector(null, DbContext);

            var allProjects = jiraConnector.GetProjects(integration.Id);
            foreach (var x in allProjects)
            {
                x.Statuses = jiraConnector.GetProjectStatuses(integration.Id, x.Id);
            }

            var projects = integration.Fields.Where(x => x.Key == ATConstants.ATJ_PROJECT_ID);
            var rewardStatus = integration.Fields.Where(x => x.Key.StartsWith(ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_, StringComparison.InvariantCulture));

            var activeProjects = projects
                .Select(p => new ActiveProject(p.Value, rewardStatus.FirstOrDefault(x => x.Key.Contains(p.Value)).Value));


            //TODO: check if data is valid with jira
            //TODO: crashes if not all projects have reward status
            return new JiraSettingsViewDTO
            {
                AllProjects = allProjects,
                ActiveProjects = activeProjects.ToList()
            };
        }

        public void UpdateJiraSettings(int projectId, JiraSettingsUpdateDTO dto)
        {
            var integration = ProjectIntegrationsScope(projectId)
                                .Include(x => x.Fields)
                                .LastOrDefault(x => x.Type == IntegrationType.ATJ);

            if (integration == null)
            {
                throw new ApplicationException("No Jira integration associated with project " + projectId);
            }

            var fields = integration.Fields
                .Where(x => x.Key == ATConstants.ATJ_PROJECT_ID || x.Key.StartsWith(ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_, StringComparison.InvariantCulture));

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

            DbContext.Update(integration);
            DbContext.SaveChanges();
        }

        #endregion


    }
}
