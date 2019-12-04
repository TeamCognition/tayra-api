using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ProjectMember : ITimeStampedEntity, IUserStampedEntity
    {
        //Composite 
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}