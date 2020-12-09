using System;
using System.Collections.Generic;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Integration : Entity<Guid>, ITimeStampedEntity, IUserStampedEntity
    {
        public Guid? ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public Guid SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public IntegrationType Type { get; set; }

        public virtual ICollection<IntegrationField> Fields { get; set; }

        #region ITimeStampedEntity and IUserStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}
