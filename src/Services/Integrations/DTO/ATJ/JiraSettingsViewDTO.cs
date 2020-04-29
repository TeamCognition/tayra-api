using System.Collections.Generic;
using Tayra.Connectors.Atlassian.Jira;

namespace Tayra.Services
{
    public class JiraSettingsViewDTO
    {
        public string JiraWebhookSettingsUrl { get; set; }
        public string WebhookUrl { get; set; }
        public ICollection<JiraProject> AllProjects { get; set; }
        public ICollection<ActiveProject> ActiveProjects { get; set; }
    }
}
