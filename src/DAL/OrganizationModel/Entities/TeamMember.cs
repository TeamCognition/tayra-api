using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class TeamMember : ITimeStampedEntity, IUserStampedEntity
    {
        //Composite Key
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

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