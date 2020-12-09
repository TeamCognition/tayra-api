using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class GitCommit : Entity<Guid>, ITimeStampedEntity
    {
        public string SHA { get; set; }

        public string ExternalUrl { get; set; }

        public string Message { get; set; }

        public string AuthorExternalId { get; set; }

        public Guid? AuthorProfileId { get; set; }
        public Profile AuthorProfile { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}