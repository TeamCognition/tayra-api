using System;
using System.Collections.Generic;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class QuestGoal : ITimeStampedEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCommentRequired { get; set; }

        public int QuestId { get; set; }
        public virtual Quest Quest { get; set; }

        public virtual ICollection<QuestGoalCompletion> Completitions { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}