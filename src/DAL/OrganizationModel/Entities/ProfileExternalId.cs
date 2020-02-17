using System;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileExternalId : ITimeStampedEntity
    {
        //dont delete
        public int OrganizationId { get; set; }

        //Composite Key
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment  { get; set; }

        public IntegrationType IntegrationType { get; set; }

        [MaxLength(100)]
        public string ExternalId { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}