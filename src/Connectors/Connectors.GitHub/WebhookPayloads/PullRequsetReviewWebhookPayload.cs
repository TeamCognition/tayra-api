using System;
using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub.WebhookPayloads
{
    public class PullRequsetReviewWebhookPayload : BaseWebhookPayload 
    {
        [JsonProperty("review")]
        public PullRequestReviewDTO PullRequestReview { get; set; }
        
        [JsonProperty("pull_request")]
        public PullRequestWebhookPayload.PullRequestDTO PullRequest { get; set;}
        
        public class PullRequestReviewDTO
        {
        
            [JsonProperty("id")]
            public string Id { get; set;}
        
            [JsonProperty("body")]
            public string Body { get; set;}
        
            [JsonProperty("title")]
            public string Title { get; set;}
        
            [JsonProperty("commit_id")]
            public string CommitId { get; set;}
        
            [JsonProperty("submitted_at")]
            public DateTime SubmittedAt { get; set;}
        
            [JsonProperty("state")]
            public string State { get; set;}
        
            [JsonProperty("user")]
            public PullRequestWebhookPayload.WebhookGithubUser ReviewedBy { get; set;}
        
        }
    }
}