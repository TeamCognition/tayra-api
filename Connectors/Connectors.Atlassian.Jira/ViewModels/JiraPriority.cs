using System;
using Newtonsoft.Json;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class JiraPriority
    {
        [JsonProperty("iconUrl")]
        public Uri IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}