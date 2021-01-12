using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TaskSync : EntityGuidId, ITimeStampedEntity
    {
        public string ExternalProjectId { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public int DateId { get; set; }

        public Guid SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
