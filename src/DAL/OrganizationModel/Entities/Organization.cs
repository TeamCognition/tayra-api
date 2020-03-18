using System;
using Firdaws.DAL;

namespace Tayra.Models.Organizations
{
    [TenantSharedEntity]
    public class Organization : IAuditedEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        
        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}