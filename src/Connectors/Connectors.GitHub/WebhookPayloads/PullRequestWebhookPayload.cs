using System;
using Newtonsoft.Json;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.GitHub.WebhookPayloads
{
    public class PullRequestWebhookPayload : BaseWebhookPayload
    {
        [JsonProperty("pull_request")]
        public PullRequestDTO PullRequest { get; set; }
    }

    public class PullRequestDTO
    {
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("body")]
        public string Body { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("closed_at")]
        public DateTime ClosedAt { get; set; }
        [JsonProperty("merged_at")]
        public DateTime MergedAt { get; set; }
        [JsonProperty("commits")]
        public int Commits { get; set; }
        [JsonProperty("review_comments")]
        public int ReviewComments {get; set;}
        [JsonProperty("user")]
        public WebhookUser Author { get; set; }

    }

    public class WebhookUser
    {
        [JsonProperty("login")]
        public string Username { get; set;}
        
        [JsonProperty("Id")]
        public string Id { get; set;}
    }
}