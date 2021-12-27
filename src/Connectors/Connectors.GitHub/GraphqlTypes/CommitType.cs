using Newtonsoft.Json;
using System;

namespace Tayra.Connectors.GitHub
{
    public class CommitType
    {
        [JsonProperty("oid")]
        public string Sha { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("additions")]
        public int Additions { get; set; }
        
        [JsonProperty("deletions")]
        public int Deletions { get; set; }
        
        [JsonProperty("commitUrl")]
        public string CommitUrl { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("committedDate")] 
        public DateTime CommittedAt { get; set; }
        
        [JsonProperty("repository")] 
        public GHRepository Repository { get; set; }

        [JsonProperty("associatedPullRequests")]
        public AssociatedPullRequestsArray AssociatedPullRequests { get; set; }

        [JsonProperty("author")] 
        public GitHubUser Author { get; set; }

        public class GitHubUser
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("user")]
            public UserClass User { get; set; }
            
            public class UserClass
            {
                [JsonProperty("login")]
                public string Username { get; set; }
            }
        }

        public class GHRepository
        {
            [JsonProperty("databaseId")] 
            public string ExternalId { get; set; }
            
            [JsonProperty("nameWithOwner")]
            public string NameWithOwner { get; set; }
        }

        public class AssociatedPullRequestsArray
        {
            [JsonProperty("nodes")]
            public AssociatedPullRequest[] AssociatedPullRequests { get; set; }
        }

        public class AssociatedPullRequest
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("mergedAt")]
            public DateTime? MergedAt { get; set; }
        }
    }
}