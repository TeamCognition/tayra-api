using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class LogSetting : ITimeStampedEntity
    {
        //CompositeKey
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public LogType LogType { get; set; }
        public bool NotifyByEmail { get; set; }
        public bool NotifyByPush { get; set; }
        public bool NotifyByNotification { get; set; }

        public int DelayEmail { get; set; }
        public int DelayPush { get; set; }
        public int DelayNotification { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
