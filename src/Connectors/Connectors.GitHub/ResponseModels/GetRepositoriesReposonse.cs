using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub
{
    public class GetRepositoriesResponse
    {
        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("repositories")]
        public Repository[] Repositories { get; set; }

        public class Repository
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
            
            [JsonProperty("full_name")]
            public string FullName { get; set; }
            
            [JsonProperty("url")]
            public string Url { get; set; }
        }
    }
}