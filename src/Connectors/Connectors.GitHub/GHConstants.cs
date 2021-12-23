namespace Tayra.Connectors.GitHub
{
    public static class GHConstants
    {
        public const string GH_INSTALLATION_ID = "GHInstallationId";
        public const string GH_INSTALLATION_TARGET_TYPE = "GHInstallationTargetType";
        public const string GH_INSTALLATION_TARGET_NAME = "GHInstallationTargetName";
        public const string GH_ACCESS_TOKEN = "AccessToken";

        public static class PullRequestReviewStates
        {
            public const string Pending = "PENDING";
            public const string Commented = "COMMENTED";
            public const string Approved = "APPROVED";
            public const string ChangesRequested = "CHANGES_REQUESTED";
            public const string Dismissed = "DISMISSED";
        }
    }
}