using System;
namespace Tayra.Connectors.Atlassian.Jira
{
    public class TaskChangelog
    {
        public DateTime Created { get; set; }
        public string Field { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public AuthorMeta Author { get; set; }

        public class AuthorMeta
        {
            public string AccountId { get; set; }
            public string EmailAddress { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
