using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class SegmentMember : ITimeStampedEntity, IUserStampedEntity
    {
        //Composite 
        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}