using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ActionPoint : EntityGuidId, ITimeStampedEntity
    {
        public Guid? SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public Guid? ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
        //public bool IsMemberOnly { get; set; }

        public ActionPointTypes Type { get; set; }
        public string Data { get; set; }
        public int DateId { get; set; }

        public DateTime? ConcludedOn { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
