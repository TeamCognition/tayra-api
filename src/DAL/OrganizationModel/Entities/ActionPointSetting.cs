using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ActionPointSetting : ITimeStampedEntity
    {
        //public int ProfileId { get; set; }
        //public virtual Profile Profile { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        //public ActionPointCategories? Category { get; set; }
        public ActionPointTypes? Type { get; set; }

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
