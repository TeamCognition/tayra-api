using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ChallengeGoalCompletion : ITimeStampedEntity
    {
        //Composite Key
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int GoalId { get; set; }
        public virtual ChallengeGoal Goal { get; set; }

        public string Comment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
