using System;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileExternalId : EntityGuidId, ITimeStampedEntity
    {
        //Composite Key
        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public Guid SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public IntegrationType IntegrationType { get; set; }

        [MaxLength(100)]
        public string ExternalId { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}