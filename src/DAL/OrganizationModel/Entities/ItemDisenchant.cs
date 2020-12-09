using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class ItemDisenchant : Entity<Guid>, ITimeStampedEntity
    {
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int DateId { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}
