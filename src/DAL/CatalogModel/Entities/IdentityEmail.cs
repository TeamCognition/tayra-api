using System;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;

namespace Tayra.Models.Catalog
{
    public class IdentityEmail : ITimeStampedEntity
    {
        //Composite Key
        public Guid IdentityId { get; set; }
        public virtual Identity Identity { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        public bool IsPrimary { get; set; }
        public DateTime? DeletedAt { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}