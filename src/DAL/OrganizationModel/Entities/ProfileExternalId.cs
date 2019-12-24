using System;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileExternalId : ITimeStampedEntity
    {
        //Composite Key
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project  { get; set; }

        public IntegrationType IntegrationType { get; set; }

        [MaxLength(100)]
        public string ExternalId { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}