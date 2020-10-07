using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class PullRequestReviewGit:ITimeStampedEntity
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public string State { get; set; }
        
        public string CommitId { get; set; }
        
        public DateTime SubmittedAt { get; set; }
        
        public string ReviewerExternalId { get; set; }

        public int? ReviewerId { get; set; }
        
        public Profile ReviewerProfile { get; set; }
        

        #region ITimeStampedEntity
        
        public DateTime Created { get; set; }
        
        public DateTime? LastModified { get; set; }
        
        #endregion
    }
}