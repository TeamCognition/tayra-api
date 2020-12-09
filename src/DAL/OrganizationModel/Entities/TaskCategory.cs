using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    [TenantSharedEntity]
    public class TaskCategory : Entity<Guid>, IAuditedEntity
    {
        public string Name { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}
