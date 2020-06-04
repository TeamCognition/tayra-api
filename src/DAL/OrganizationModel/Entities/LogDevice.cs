using System;
using System.Collections.Generic;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class LogDevice : ITimeStampedEntity
    {
        public int Id { get; set; }

        public virtual int? ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public LogDeviceTypes Type { get; set; }
        public string Address { get; set; }

        //public bool IsEnabled { get; set; }
        //public int IdentityId { get; set; }
        //public virtual Identity Identity { get; set; }

        public virtual ICollection<LogSetting> Settings { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
