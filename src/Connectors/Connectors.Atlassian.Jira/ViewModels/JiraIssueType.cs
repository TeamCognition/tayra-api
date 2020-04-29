using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class JiraIssueType
    {
        [DeserializeAs(Name = "id")]
        public string Id { get; set; }

        [DeserializeAs(Name = "name")]
        public string Name { get; set; }

        [DeserializeAs(Name = "subtask")]
        public bool IsSubtask { get; set; }

        [DeserializeAs(Name = "iconUrl")]
        public string IconUrl { get; set; }

        [DeserializeAs(Name = "statuses")]
        public List<JiraStatus> Statuses { get; set; }
    }
}
