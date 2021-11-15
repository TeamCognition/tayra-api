using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class ProfileAssignment : EntityGuidId, ITimeStampedEntity, IUserStampedEntity
    {
        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public Guid SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public Guid TeamId { get; set; }
        public virtual Team Team { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}