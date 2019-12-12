using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ActionPointProject : ITimeStampedEntity
    {
        //Composite Key
        public int ActionPointId { get; set; }
        public virtual ActionPoint ActionPoint { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public DateTime? ConcludedOn { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
