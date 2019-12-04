using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Public Methods

        public List<IntegrationViewDTO> GetProjectIntegrations(int projectId)
        {
            return DbContext.Integrations
                .Where(x => x.ProjectId == projectId)
                .Select(x => new IntegrationViewDTO
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
            var integration = DbContext
                                .Integrations
                                .Include(x => x.Fields)
                                .LastOrDefault(x => x.ProjectId == projectId && x.Type == IntegrationType.ATJ);

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
            var integration = DbContext
                                .Integrations
                                .Include(x => x.Fields)
                                .LastOrDefault(x => x.ProjectId == projectId && x.Type == IntegrationType.ATJ);

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
