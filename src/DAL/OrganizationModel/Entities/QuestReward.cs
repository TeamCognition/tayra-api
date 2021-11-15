using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class QuestReward : ITimeStampedEntity
    {
        public int QuestId { get; set; }
        public virtual Quest Quest { get; set; }

        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int Quantity { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}