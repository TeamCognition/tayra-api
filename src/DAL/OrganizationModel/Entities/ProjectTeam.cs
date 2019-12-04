using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ProjectTeam : ITimeStampedEntity, IUserStampedEntity
    {
        //Composite Key
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}