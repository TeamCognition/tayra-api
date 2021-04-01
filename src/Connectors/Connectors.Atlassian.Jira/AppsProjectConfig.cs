using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class AppsProjectConfig
    {
        private AppsProjectConfig()
        {
        }

        //for 1 hack
        public AppsProjectConfig(string projectId)
        {
            ProjectId = projectId;
        }
        
        public AppsProjectConfig(string projectId, string rewardStatusId, IEnumerable<AtlassianJiraWkUnStatusesConfiguration.WorkUnitStatusConfiguration> statuses)
        {
            ProjectId = projectId;
            RewardStatusId = rewardStatusId;
            Statuses = statuses;
        }

        [Required]
        public string ProjectId { get; set; }

        [Required]
        public string RewardStatusId { get; set; }

        public IEnumerable<AtlassianJiraWkUnStatusesConfiguration.WorkUnitStatusConfiguration> Statuses { get; set; }
        
        public static AppsProjectConfig[] From(IEnumerable<JiraProject> jiraProjects)
        {
            return jiraProjects.Select(x => new AppsProjectConfig
            {
                ProjectId = x.Id,
                Statuses = x.Statuses.Select(s => new AtlassianJiraWkUnStatusesConfiguration.WorkUnitStatusConfiguration(s.Id, WorkUnitStatuses.Uncategorized))
            }).ToArray();
        }
        
        public static List<AppsProjectConfig> From(ICollection<IntegrationField> fields)
        {
            var activeJiraProjectIds = fields.Where(x => x.Key == ATConstants.ATJ_PROJECT_ID).Select(x => x.Value).ToArray();

            var activeProjects = new List<AppsProjectConfig>();
            foreach (var projectId in activeJiraProjectIds)
            {
                var rewardStatusId = fields.Where(x => x.Key == ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + projectId).Select(x => x.Value).FirstOrDefault();
                var jiraConfig = AtlassianJiraWkUnStatusesConfiguration.From(projectId, fields);
                activeProjects.Add(new AppsProjectConfig(projectId, rewardStatusId, jiraConfig.WorkUnitStatuses));
            }

            return activeProjects;
        }
    }
}
