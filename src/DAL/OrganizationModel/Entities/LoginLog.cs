using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class LoginLog : ITimeStampedEntity
    {
        public int Id { get; set; }

        public int ProfileId { get; set; }
        public int IdentityId { get; set; }

        public string ClaimsJson { get; set; }

        public string FailReason { get; set; }


        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
