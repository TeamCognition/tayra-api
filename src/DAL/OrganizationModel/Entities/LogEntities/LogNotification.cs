using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class Notification : IAuditedEntity
    {
        public int Id { get; set; }
        public DateTime? ReadAt { get; set; }

        public Guid LogId { get; set; }
        public virtual Log Log { get; set; }


        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}
