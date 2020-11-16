using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class PullRequest : ITimeStampedEntity
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string State { get; set; }

        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public string ExternalUrl { get; set; }
        
        public DateTime ExternalCreatedAt { get; set; }

        public bool IsLocked { get; set; }
        
        public DateTime ExternalUpdatedAt { get; set; }
        
        public DateTime? ClosedAt { get; set; }
        
        public DateTime? MergedAt { get; set; }
        
        public int CommitsCount { get; set; }
        
        public int ReviewCommentsCount {get; set;}
        
        public string ExternalAuthorId { get; set; }
        
        public int? AuthorProfileId { get; set; }
        
        public Profile AuthorProfile { get; set; }

        #region ITimeStampedEntity
        
        public DateTime Created { get; set; }
        
        public DateTime? LastModified { get; set; }
        
        #endregion
        
    }
}