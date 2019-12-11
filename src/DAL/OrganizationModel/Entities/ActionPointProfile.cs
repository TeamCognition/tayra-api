using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ActionPointProfile : ITimeStampedEntity
    {
        //Composite Key
        public int ActionPointId { get; set; }
        public virtual ActionPoint ActionPoint { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int ProfileOnly { get; set; }

        public DateTime? ConcludedOn { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
