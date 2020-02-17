using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class LogSetting : ITimeStampedEntity
    {
        //CompositeKey
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int LogDeviceId { get; set; }
        public virtual LogDevice LogDevice { get; set; }

        public LogEvents LogEvent { get; set; }

        //public int? DeliveryDelay { get; set; }
        public bool IsEnabled { get; set; }
        //public int? SegmentId { get; set; }
        //public virtual Segment Segment { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
