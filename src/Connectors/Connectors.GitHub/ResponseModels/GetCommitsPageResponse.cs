using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tayra.Connectors.GitHub
{
    public class GetCommitsPageResponse
    {
        [JsonProperty("pageInfo")]
        public PageInfoType PageInfo { get; set; }

        [JsonProperty("nodes")]
        public IList<CommitType> Commits { get; set; }
    }
}
