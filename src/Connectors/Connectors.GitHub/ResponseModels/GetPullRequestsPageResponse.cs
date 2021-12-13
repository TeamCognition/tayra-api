using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub
{
    public class GetPullRequestsPageResponse
    {
        [JsonProperty("repository")]
        public GithubRepository Repository { get; set; }

        public class GithubRepository
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            
            [JsonProperty("pullRequests")]
            public PullRequestNodes PullRequestsNodes { get; set; }
        }

        public class PullRequest
        {    
            [JsonProperty("id")]
            public string ExternalId { get; set; }
            
             [JsonProperty("number")]
             public int Number { get; set; }
             
             [JsonProperty("title")]
             public string Title { get; set; }
             
             [JsonProperty("bodyText")]
             public string BodyText { get; set; }
             
             [JsonProperty("url")]
             public string Url { get; set; }
             
             [JsonProperty("state")]
             public string State { get; set; }
             
             [JsonProperty("additions")]
             public int Additions { get; set; }
             
             [JsonProperty("deletions")]
             public int Deletions { get; set; }
             
             [JsonProperty("createdAt")]
             public string CreatedAt { get; set; }
             
             [JsonProperty("merged")]
             public bool IsMerged { get; set; }
             
             [JsonProperty("locked")]
             public bool IsLocked { get; set; }
             
             [JsonProperty("updatedAt")]
             public string UpdatedAt { get; set; }

             [JsonProperty("closedAt")]
             public string ClosedAt { get; set; }
             
             [JsonProperty("mergedAt")]
             public string MergedAt { get; set; }
             
             [JsonProperty("repository")] 
             public GHRepository Repository { get; set; }
             
             [JsonProperty("author")]
             public Author Author { get; set; }
             
             [JsonProperty("reviews")]
             public ReviewNodes ReviewNodes { get; set; }
             
             [JsonProperty("commits")]
             public CommitNodes CommitNodes { get; set; }
        }

        public class Review
        {
            [JsonProperty("state")]
            public string State { get; set; }
            
            [JsonProperty("author")]
            public Author Author { get; set; }
        }
        
        public class Author
        {
            [JsonProperty("login")]
            public string Username { get; set; }

            [JsonProperty("Id")]
            public string Id { get; set; }
        }
        
        public class PullRequestNodes
        {
            [JsonProperty("pageInfo")]
            public PageInfoType PageInfo { get; set; }

            [JsonProperty("nodes")]
            public PullRequest[] PullRequests { get; set; }
        }
        
        public class ReviewNodes
        {
            [JsonProperty("totalCount")] 
            public int TotalCount { get; set; }
            
            [JsonProperty("nodes")]
            public Review[] PullRequests { get; set; }
        }

        public class CommitNodes
        {
            [JsonProperty("totalCount")] 
            public int TotalCount { get; set; }
        }
        
        public class GHRepository
        {
            [JsonProperty("databaseId")]
            public string ExternalId { get; set; }
            
            [JsonProperty("nameWithOwner")]
            public string NameWithOwner { get; set; }
        }
    }
}

