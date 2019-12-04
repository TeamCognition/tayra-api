using Newtonsoft.Json;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class JiraTimetracking
    {
        [JsonProperty("originalEstimate")]
        public string OriginalEstimate { get; set; }

        [JsonProperty("remainingEstimate")]
        public string RemainingEstimate { get; set; }

        [JsonProperty("timeSpent")]
        public string TimeSpent { get; set; }

        [JsonProperty("originalEstimateSeconds")]
        public long OriginalEstimateSeconds { get; set; }

        [JsonProperty("remainingEstimateSeconds")]
        public long RemainingEstimateSeconds { get; set; }

        [JsonProperty("timeSpentSeconds")]
        public long TimeSpentSeconds { get; set; }
    }
}