using System.Collections.Generic;

namespace Tayra.Connectors.Atlassian.Jira
{
    public class SearchResponse
    {
        public string Expand { get; set; }
        public int StartAt { get; set; }
        public int MaxResults { get; set; }
        public int Total { get; set; }
        public List<JiraIssue> Issues { get; set; }
    }
}
