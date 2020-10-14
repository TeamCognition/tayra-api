namespace Tayra.Connectors.GitHub.Helper
{
    public class PrWebhooksContstants
    {
        public static string[] PullRequestIgnoredActions = new[] {"ready_for_review","review_request_removed","assigned",
            "unassigned","review_requested","labeled","unlabeled","synchronize"};

        public static string PR_ACTION_IGNORED = "Skipped : PullRequest Action Ignored";
        public static string PR_UPDATED = "PullRequest Updated";
        public static string PR_CREATED = "PullRequest created successfully";
        public static string COULDNT_FIND_PR = "Couldn't find the PullRequest";    
        public static string PR_REVIEW_UPDATED = "PullRequestReview Updated successfully";
        public static string PR_REVIEW_CREATED = "PullRequest review created successfully";
        public static string COULDNT_FIND_PR_REVIEW= "Couldn't find the pullRequest review";
        public static string COMMENT_UPDATED= "Comment updated successfully";
        public static string COMMENT_CREATED ="Comment created successfully";
            
    }
}