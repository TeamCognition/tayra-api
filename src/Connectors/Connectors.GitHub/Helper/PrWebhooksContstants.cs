namespace Tayra.Connectors.GitHub.Helper
{
    public class PrWebhooksContstants
    {
        public static string[] PullRequestIgnoredActions = new[] {"ready_for_review","review_request_removed","assigned",
            "unassigned","review_requested","labeled","unlabeled","synchronize"};
    }
}