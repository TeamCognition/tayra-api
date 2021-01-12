using System;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfilePraise : EntityGuidId, ITimeStampedEntity, IUserStampedEntity
    {
        //Composite Key
        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public Guid PraiserProfileId { get; set; }
        //public virtual Profile PraiserProfile { get; set; }

        public int DateId { get; set; }
        //public virtual Date Date { get; set; }

        public PraiseTypes Type { get; set; }

        [MaxLength(140)]
        public string Message { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}
