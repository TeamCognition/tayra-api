using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ActionPointSetting : ITimeStampedEntity
    {
        public ActionPointTypes Type { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        //public ActionPointCategories? Category { get; set; }

        public bool NotifyByEmail { get; set; }
        public bool NotifyByPush { get; set; }
        public bool NotifyByNotification { get; set; }

        public DateTime? MuteUntil { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
