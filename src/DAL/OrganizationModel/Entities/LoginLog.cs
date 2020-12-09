using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class LoginLog : Entity<Guid>, ITimeStampedEntity
    {
        public Guid ProfileId { get; set; }
        public Guid IdentityId { get; set; }

        public string ClaimsJson { get; set; }

        public string FailReason { get; set; }


        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
