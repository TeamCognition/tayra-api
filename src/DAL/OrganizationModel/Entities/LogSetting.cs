using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class LogSetting : EntityGuidId, ITimeStampedEntity
    {
        //CompositeKey
        public Guid LogDeviceId { get; set; }
        public virtual LogDevice LogDevice { get; set; }

        public LogEvents LogEvent { get; set; }

        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        //public int? DeliveryDelay { get; set; }
        public bool IsEnabled { get; set; }
        //public Guid? SegmentId { get; set; }
        //public virtual Segment Segment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
