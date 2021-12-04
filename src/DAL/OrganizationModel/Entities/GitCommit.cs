using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class GitCommit : EntityGuidId, ITimeStampedEntity
    {
        public string ExternalRepositoryId { get; set; }
        
        public string Sha { get; set; }

        public string ExternalUrl { get; set; }

        public string Message { get; set; }

        public string AuthorExternalId { get; set; }
        
        public int Additions { get; set; }
        
        public int Deletions { get; set; }

        /// <summary>
        /// Database Id of the first repository pull request that contained the commit
        /// </summary>
        public Guid? FirstPullRequestId { get; set; }
        /// <summary>
        /// First repository pull request that contained the commit
        /// </summary>
        public PullRequest FirstPullRequest { get; set; }

        public Guid? AuthorProfileId { get; set; }
        public Profile AuthorProfile { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}