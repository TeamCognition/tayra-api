using System;
using Newtonsoft.Json;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class JiraWebhookEvent
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("webhookEvent")]
        public string Event { get; set; }

        [JsonProperty("issue")]
        public JiraIssue JiraIssue { get; set; }
    }
}
