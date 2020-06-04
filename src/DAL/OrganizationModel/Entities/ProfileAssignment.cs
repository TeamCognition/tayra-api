using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class ProfileAssignment : ITimeStampedEntity, IUserStampedEntity
    {
        public int Id { get; set; }
        
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}