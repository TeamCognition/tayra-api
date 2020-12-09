using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class QuestGoalCompletion : ITimeStampedEntity
    {
        //Composite Key
        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int GoalId { get; set; }
        public virtual QuestGoal Goal { get; set; }

        public string Comment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
