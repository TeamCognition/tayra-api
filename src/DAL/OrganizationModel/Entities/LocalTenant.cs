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
        
        public bool IsCreateSegmentOnboarding { get; set; }
        
        public bool IsAddSourcesOnboarding { get; set; }
        
        public bool IsInviteUsersOnboarding { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}