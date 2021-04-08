using System.Collections.Generic;
using System.Linq;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class SetAppsProjectConfig
    {
        //for API
        private SetAppsProjectConfig()
        {
            
        }
        //for 1 hack
        public SetAppsProjectConfig(string projectId)
        {
            ProjectId = projectId;
        }
    
        public SetAppsProjectConfig(string projectId, string rewardStatusId, IEnumerable<AtlassianJiraWkUnStatusesConfiguration.WorkUnitStatusConfiguration> statuses)
        {
            ProjectId = projectId;
            RewardStatusId = rewardStatusId;
            Statuses = statuses;
        }

        public string ProjectId { get; set; }
       
        public string RewardStatusId { get; set; }

        public IEnumerable<AtlassianJiraWkUnStatusesConfiguration.WorkUnitStatusConfiguration> Statuses { get; set; }

        public static List<SetAppsProjectConfig> From(ICollection<IntegrationField> fields)
        {
            var activeJiraProjectIds = fields.Where(x => x.Key == ATConstants.ATJ_PROJECT_ID).Select(x => x.Value).ToArray();

            var activeProjects = new List<SetAppsProjectConfig>();
            foreach (var projectId in activeJiraProjectIds)
            {
                var rewardStatusId = fields.Where(x => x.Key == ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + projectId).Select(x => x.Value).FirstOrDefault();
                var jiraConfig = AtlassianJiraWkUnStatusesConfiguration.From(projectId, fields);
                activeProjects.Add(new SetAppsProjectConfig(projectId, rewardStatusId, jiraConfig.WorkUnitStatuses));
            }

            return activeProjects;
        }
    }
}