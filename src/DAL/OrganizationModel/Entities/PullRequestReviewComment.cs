using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class PullRequestReviewComment : EntityGuidId, ITimeStampedEntity
    {
        public string ExternalId { get; set; }

        public string Body { get; set; }

        public string ExternalUrl { get; set; }

        public DateTime ExternalCreatedAt { get; set; }

        public DateTime ExternalUpdatedAt { get; set; }

        public Guid? CommenterProfileId { get; set; }

        public Profile CommenterProfile { get; set; }

        public Guid PullRequestReviewId { get; set; }

        public PullRequestReview PullRequestReview { get; set; }

        public Guid PullRequestId { get; set; }

        public PullRequest PullRequest { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
