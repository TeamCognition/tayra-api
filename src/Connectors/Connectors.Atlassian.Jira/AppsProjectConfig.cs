using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class AppsProjectConfig
    {
        public AppsProjectConfig(string projectId, string projectName, IEnumerable<WorkUnitStatus> statuses)
        {
            IsActive = false;
            ProjectId = projectId;
            ProjectName = projectName;
            Statuses = statuses;
        }
        
        public AppsProjectConfig(string projectId, string projectName, string rewardStatusId, IEnumerable<WorkUnitStatus> statuses)
        {
            IsActive = true;
            ProjectId = projectId;
            ProjectName = projectName;
            RewardStatusId = rewardStatusId;
            Statuses = statuses;
        }
       
        
        public string ProjectId { get; set; }

        public string ProjectName { get; set; }

        public bool IsActive { get; set; }
        
        public string RewardStatusId { get; set; }
        
        public IEnumerable<WorkUnitStatus> Statuses { get; set; }
        
        public static ICollection<AppsProjectConfig> From(IEnumerable<JiraProject> jiraProjects, ICollection<IntegrationField> fields)
        {
            var activeJiraProjectIds = fields.Where(x => x.Key == ATConstants.ATJ_PROJECT_ID).Select(x => x.Value).ToArray();

            var projects = new List<AppsProjectConfig>();
            foreach (var project in jiraProjects)
            {
                if (activeJiraProjectIds.Contains(project.Id))
                {
                    var rewardStatusId = fields.Where(x => x.Key == ATConstants.ATJ_REWARD_STATUS_FOR_PROJECT_ + project.Id).Select(x => x.Value).FirstOrDefault();
                    var statuses = AtlassianJiraWkUnStatusesConfiguration.From(project.Id, fields).WorkUnitStatuses;

                    var statusesWithName = statuses.Select(s => new WorkUnitStatus(
                        s.ExternalStatusId,
                        project.Statuses.First(x => x.Id == s.ExternalStatusId).Name,
                        s.Status)
                    );
                    
                    projects.Add(new AppsProjectConfig(project.Id, project.Name, rewardStatusId, statusesWithName));

                }
                else
                {
                    var statuses = project.Statuses.Select(s => new WorkUnitStatus(s.Id, s.Name, WorkUnitStatuses.Uncategorized));
                    projects.Add(new AppsProjectConfig(project.Id, project.Name, statuses));
                }
            }

            return projects;
        }
        
        public class WorkUnitStatus
        {
            public WorkUnitStatus(string externalId, string name, WorkUnitStatuses status)
            {
                ExternalId = externalId;
                Name = name;
                Status = status;
            }

            public string ExternalId { get; set; }
            public string Name { get; set; }
            public WorkUnitStatuses Status { get; set; }
        }
    }
}
