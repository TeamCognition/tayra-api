using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class QuestCommit : ITimeStampedEntity
    {
        //Composite Key
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int QuestId { get; set; }
        public virtual Quest Quest { get; set; }

        public DateTime? CompletedAt { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
