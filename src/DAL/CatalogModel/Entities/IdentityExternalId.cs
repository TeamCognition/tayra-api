using System;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Catalog
{
    public class IdentityExternalId : ITimeStampedEntity
    {
        //Composite Key
        public int IdentityId { get; set; }
        public virtual Identity Identity { get; set; }

        public IntegrationType IntegrationType { get; set; }

        [MaxLength(100)]
        public string ExternalId { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}