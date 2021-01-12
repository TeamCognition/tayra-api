using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class Shop : EntityGuidId, IAuditedEntity
    {
        public string Name { get; set; }

        public DateTime? ClosedAt { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}