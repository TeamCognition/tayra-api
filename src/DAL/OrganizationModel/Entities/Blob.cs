using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Blob : EntityGuidId, ITimeStampedEntity, IUserStampedEntity
    {
        public BlobTypes Type { get; set; }
        public BlobPurposes Purpose { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public long Filesize { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}
