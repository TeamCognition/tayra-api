using System.Collections.Generic;
using Newtonsoft.Json;
using Tayra.Connectors.GitHub.Common;
namespace Tayra.Connectors.GitHub.ResponseModels
{
    public class GetBranchesByRepository
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
            [JsonProperty("edges")]
            public List<Edge<Branch>> Edges { get; set; }
        }
        public class Branch
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}