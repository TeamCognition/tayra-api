using System;
using System.Collections.Generic;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class ChallengeGoal : ITimeStampedEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCommentRequired { get; set; }

        public int ChallengeId { get; set; }
        public virtual Challenge Challenge { get; set; }

        public virtual ICollection<ChallengeGoalCompletion> Completitions { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}