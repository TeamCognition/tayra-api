using System;
using Newtonsoft.Json;

namespace Tayra.Connectors.GitHub.WebhookPayloads
{
    public class PullRequsetReviewWebhookPayload : BaseWebhookPayload 
    {
        [JsonProperty("review")]
        public PullRequestReviewDTO PullRequestReview { get; set; }
        
        [JsonProperty("pull_request")]
        public PullRequestDTO PullRequest { get; set;}
        
    }

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
        public ReviewUser ReviewUser { get; set;}
        
    }

    public class ReviewUser
    {
        [JsonProperty("login")]
        public string Username { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}