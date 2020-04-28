using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class ItemDisenchant : ITimeStampedEntity
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int DateId { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
