using System.Collections.Generic;
using Newtonsoft.Json;
using Tayra.Connectors.Common;
using Tayra.Connectors.GitHub.Common;

namespace Tayra.Connectors.GitHub
{
    public class GetPullRequestsResponse
    {
        [JsonProperty("search")] 
        public GqSearch Search { get; set; }
        public class GqSearch
        {
            [JsonProperty("edges")]
            public List<Edge<PullRequestType>> Edges { get; set;}
        }
    }
}