using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class PullRequestReview : ITimeStampedEntity
    {
        public int Id { get; set; }
        
        public string Body { get; set; }

        public string State { get; set; }

        public string CommitId { get; set; }

        public DateTime SubmittedAt { get; set; }


        public int PullRequestId { get; set; }

        public PullRequest PullRequest { get; set; }

        public string ReviewExternalId { get; set; }


        public int? ReviewerProfileId { get; set; }

        public Profile ReviewerProfile { get; set; }


        #region ITimeStampedEntity
        
        public DateTime Created { get; set; }
        
        public DateTime? LastModified { get; set; }
        
        #endregion
    }
}