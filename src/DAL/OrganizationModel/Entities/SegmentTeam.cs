using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class SegmentTeam : ITimeStampedEntity, IUserStampedEntity
    {
        //Composite Key
        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}