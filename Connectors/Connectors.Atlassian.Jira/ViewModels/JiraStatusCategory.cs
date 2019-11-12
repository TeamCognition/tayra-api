using Newtonsoft.Json;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class JiraStatusCategory
    {
        [JsonProperty("id")]
        public IssueStatusCategories Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
