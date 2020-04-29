using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TaskSync: ITimeStampedEntity
    {
        public int Id { get; set; }

        public string ExternalProjectId { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public int DateId { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
