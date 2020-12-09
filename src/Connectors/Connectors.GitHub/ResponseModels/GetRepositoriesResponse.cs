using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub
{
    //https://docs.github.com/en/rest/reference/apps#list-app-installations-accessible-to-the-user-access-token
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
        }
    }
}