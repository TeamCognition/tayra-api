using System;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Blob : ITimeStampedEntity, IUserStampedEntity
    {
        public Guid Id { get; set; }

        public BlobTypes Type { get; set; }
        public BlobPurposes Purpose { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public long Filesize { get; set; }

        #region ITimeAndIUser

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}
