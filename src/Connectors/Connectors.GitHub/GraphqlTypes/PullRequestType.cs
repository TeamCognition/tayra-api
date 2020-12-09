using System;
using Newtonsoft.Json;

namespace Tayra.Connectors.Common
{
    public class PullRequestType
    {

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("merged")]
        public bool IsMerged { get; set; }
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        [JsonProperty("merged_at")]
        public string MergedAt { get; set; }
        [JsonProperty("author")]
        public PullRequestUser Author { get; set; }

        public class PullRequestUser
        {
            [JsonProperty("login")]
            public string Username { get; set; }

            [JsonProperty("Id")]
            public string Id { get; set; }
        }
    }
}