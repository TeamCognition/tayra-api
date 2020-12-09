using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub
{
    public class GetCommitsByShaResponse
    {
        [JsonProperty("repository")]
        public GithubRepository Repository { get; set; }

        public class GithubRepository
        {
            [JsonProperty("object")]
            
            public CommitType GhObject { get; set; }
        }
    }
}