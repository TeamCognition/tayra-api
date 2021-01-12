using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    [TenantSharedEntity]
    public class LocalTenant : IAuditedEntity
    {
        public Guid TenantId { get; set; }
        public string Identifier { get; set; }
        public string DisplayName { get; set; }
        
        public bool IsSegmentOnboardingCompleted { get; set; }
        
        public bool IsAppsOnboardingCompleted { get; set; }
        
        public bool IsMembersOnboardingCompleted { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}