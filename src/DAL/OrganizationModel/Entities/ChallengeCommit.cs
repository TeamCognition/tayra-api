using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class ChallengeCommit : ITimeStampedEntity
    {
        //Composite Key
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int ChallengeId { get; set; }
        public virtual Challenge Challenge { get; set; }

        public DateTime? CompletedAt { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
