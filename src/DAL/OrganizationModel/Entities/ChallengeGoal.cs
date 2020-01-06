using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ChallengeGoal : ITimeStampedEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCommentRequired { get; set; }

        public int ChallengeId { get; set; }
        public virtual Challenge Challenge { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}