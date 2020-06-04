﻿using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TaskLog : ITimeStampedEntity
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public TaskStatuses Status { get; set; }

        public int ReporterProfileId { get; set; }

        public int AssigneeProfileId { get; set; }
        public virtual Profile AssigneeProfile { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
