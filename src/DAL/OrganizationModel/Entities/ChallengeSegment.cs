using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class ChallengeSegment : ITimeStampedEntity
    {
        //Composite Key
        public int ChallengeId { get; set; }
        public virtual Challenge Challenge { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
