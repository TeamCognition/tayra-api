using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ProfileOneUp : ITimeStampedEntity, IUserStampedEntity
    {
        //Composite Key
        public int UppedProfileId { get; set; }
        public virtual Profile UppedProfile { get; set; }

        public int DateId { get; set; }
        //public virtual Date Date { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}
