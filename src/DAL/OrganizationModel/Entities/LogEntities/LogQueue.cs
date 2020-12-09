using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class LogQueue : ITimeStampedEntity
    {
        public Guid ProfileId { get; set; }
        public Guid LogDeviceId { get; set; }
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
