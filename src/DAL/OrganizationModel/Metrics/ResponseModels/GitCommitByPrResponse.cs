using System.Collections.Generic;
using Newtonsoft.Json;
using Tayra.Connectors.GitHub.Common;
using Tayra.Models.Organizations.Metrics.GraphqlTypes;

namespace Tayra.Models.Organizations.Metrics.ResponseModels
{
    public class GitCommitByPrResponse
    {
        [JsonProperty("repository")]
        public GithubRepository Repository { get; set; }
        public class GithubRepository
        {
            [JsonProperty("pullRequest")] 
            public PullRequest pullRequest { get; set; }
        }

        public class PullRequest
        {
            [JsonProperty("commits")]
            public GqlCommits Commits { get; set; }
        } 
        public class GqlCommits
        {
            public List<Edge<PullRequestCommit>> edges { get; set; }
        }
        public class PullRequestCommit
        {
            [JsonProperty("commit")]

            public CommitType Commit { get; set; }
        }
    }
}