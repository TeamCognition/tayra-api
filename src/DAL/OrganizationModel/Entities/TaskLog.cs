using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TaskLog : Entity<Guid>, ITimeStampedEntity
    {
        public string ExternalId { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public TaskStatuses Status { get; set; }

        public int ReporterProfileId { get; set; }

        public int AssigneeProfileId { get; set; }
        public virtual Profile AssigneeProfile { get; set; }

        public Guid TeamId { get; set; }
        public virtual Team Team { get; set; }

        public Guid SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
