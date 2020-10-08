using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class PullRequest : ITimeStampedEntity
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public string ExternalUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public DateTime ClosedAt { get; set; }
        
        public DateTime MergedAt { get; set; }
        
        public int Commits { get; set; }
        
        public int ReviewComments {get; set;}
        
        public string ExternalAuthorId { get; set; }
        
        public int? AuthorProfileId { get; set; }
        
        public Profile AuthorProfile { get; set; }

        #region ITimeStampedEntity
        
        public DateTime Created { get; set; }
        
        public DateTime? LastModified { get; set; }
        
        #endregion
        
    }
}