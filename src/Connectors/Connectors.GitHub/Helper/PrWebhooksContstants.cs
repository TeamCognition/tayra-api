namespace Tayra.Connectors.GitHub.Helper
{
    public class PrWebhooksContstants
    {
        public static string[] PullRequestIgnoredActions = {"ready_for_review","review_request_removed","assigned",
            "unassigned","review_requested","labeled","unlabeled","synchronize"};
        
        public static string COULDNT_FIND_PR = "Couldn't find the PullRequest";
        public static string COULDNT_FIND_PR_REVIEW= "Couldn't find the pullRequest review";
    }
}