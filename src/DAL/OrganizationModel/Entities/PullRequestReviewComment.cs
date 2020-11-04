using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class PullRequestReviewComment:ITimeStampedEntity
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }
        public string Body { get; set; }
        
        public string ExternalUrl { get; set; }
        
        public DateTime ExternalCreatedAt { get; set; }
        
        public DateTime ExternalUpdatedAt { get; set; }

        public int? CommenterProfileId { get; set; }

        public Profile CommenterProfile { get; set; }

        public int PullRequestReviewId { get; set; }
        
        public PullRequestReview PullRequestReview { get; set; }

        public int PullRequestId { get; set; }
        
        public PullRequest PullRequest { get; set; }
        
        #region ITimeStampedEntity
        
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        
        #endregion
    }
}
