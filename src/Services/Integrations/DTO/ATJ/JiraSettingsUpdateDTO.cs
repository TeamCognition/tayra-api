using System.Collections.Generic;
using Tayra.Connectors.Atlassian.Jira;

namespace Tayra.Services
{
    public class JiraSettingsUpdateDTO
    {
        public ICollection<SetAppsProjectConfig> ActiveProjects { get; set; }
        public bool PullTasksForNewProjects { get; set; }
    }
}
