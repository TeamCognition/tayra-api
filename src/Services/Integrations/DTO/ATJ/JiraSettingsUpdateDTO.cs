using System.Collections.Generic;
using Tayra.Connectors.Atlassian.Jira;

namespace Tayra.Services
{
    public class JiraSettingsUpdateDTO
    {
        public ICollection<AppsProjectConfig> ActiveProjects { get; set; }
        public bool PullTasksForNewProjects { get; set; }
    }
}
