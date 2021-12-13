using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub
{
    public class GetBranchesByRepositoryPageResponse
    {
        [JsonProperty("repository")]
        public GithubRepository Repository { get; set; }

        public class GithubRepository
        {
            [JsonProperty("refs")]
            public Refs Refs { get; set; }
        }
        public class Refs
        {
            [JsonProperty("pageInfo")]
            public PageInfoType PageInfo { get; set; }

            [JsonProperty("nodes")]
            public Branch[] Nodes { get; set; }
        }
        public class Branch
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}