using System;
using System.Collections.Generic;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ActionPoint : ITimeStampedEntity
    {
        public ActionPointTypes? Type { get; set; }
        public string Data { get; set; }
        public int DateId { get; set; }

        public DateTime? ConcludedOn { get; set; }
        
        public virtual ICollection<ActionPointProfile> Profiles { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
