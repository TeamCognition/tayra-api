using Newtonsoft.Json;
using System;

namespace Tayra.Connectors.GitHub
{
    //https://docs.github.com/en/rest/reference/apps#list-app-installations-accessible-to-the-user-access-token
    // created_at and updated_at properties are in local server time
    public class UserInstallationsResponse
    {
        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("installations")]
        public Installation[] Installations { get; set; }

        public class Installation
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("app_id")]
            public string AppId { get; set; }

            [JsonProperty("target_id")]
            public long TargetId { get; set; }

            [JsonProperty("target_type")]
            public string TargetType { get; set; }

            [JsonProperty("created_at")]
            public DateTime? CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public DateTime? UpdatedAt { get; set; }
        }
    }
}