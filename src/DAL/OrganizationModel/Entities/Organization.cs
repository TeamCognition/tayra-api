using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    [TenantSharedEntity]
    public class Organization : Entity<int>, IAuditedEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        
        public bool isCreateSegmentOnboarding { get; set; }
        
        public bool isAddSourcesOnboarding { get; set; }
        
        public bool isInviteUsersOnboarding { get; set; }
        

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}