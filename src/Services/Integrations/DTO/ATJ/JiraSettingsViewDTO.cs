using System.Collections.Generic;
using Tayra.Connectors.Atlassian.Jira;

namespace Tayra.Services
{
    public class JiraSettingsViewDTO
    {
        public string JiraWebhookSettingsUrl { get; set; }
        public string WebhookUrl { get; set; }
        public ICollection<AppsProjectConfig> AllProjects { get; set; }
        public ICollection<AppsProjectConfig> ActiveProjects { get; set; }
    }
}
