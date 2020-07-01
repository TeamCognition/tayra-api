using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class QuestSegment : ITimeStampedEntity
    {
        //Composite Key
        public int QuestId { get; set; }
        public virtual Quest Quest { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
