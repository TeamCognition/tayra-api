using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    public class Notification : IAuditedEntity
    {
        public int Id { get; set; }
        public DateTime? ReadAt { get; set; }

        public int LogId { get; set; }
        public virtual Log Log { get; set; }


        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}
