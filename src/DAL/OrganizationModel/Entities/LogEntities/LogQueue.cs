using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class LogQueue : ITimeStampedEntity
    {
        public int ProfileId { get; set; }
        public int LogDeviceId { get; set; }
        public LogEvents LogEvent { get; set; }
        public DateTime DeliverAt { get; set; }

        public DateTime? CompletedAt { get; set; }
        public string Status { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
