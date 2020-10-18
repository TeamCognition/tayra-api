using System;
using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub.WebhookPayloads
{
    public class PullRequestReviewCommentPayload :BaseWebhookPayload
    {
        [JsonProperty("comment")]
        public PullRequestReviewCommentDTO ReviewComment { get; set; }
        
        [JsonProperty("pull_request")]
        public PullRequestWebhookPayload.PullRequestDTO PullRequest { get; set; }
        
        
        public class PullRequestReviewCommentDTO
        {
            [JsonProperty("id")]
            public string Id { get; set;}
        
            [JsonProperty("body")]
            public string Body { get; set;}
        
            [JsonProperty("url")]
            public string Url { get; set;}
        
            [JsonProperty("pull_request_review_id")]
            public string PullRequestReviewId { get; set;}
        
            [JsonProperty("commit_id")]
            public string CommitId { get; set;}
        
            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set;}
  
            [JsonProperty("updated_at")]
            public DateTime UpdatedAt { get; set;}
        
            [JsonProperty("user")]
            public PullRequestWebhookPayload.WebhookGithubUser GithubUserCommentedPullRequestReviewProfile { get; set;}
        }
        
    }

}