using System;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    [TenantSharedEntity]
    public class Organization : IAuditedEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        
        public bool isCreateSegmentOnboarding { get; set; }
        
        public bool isAddSourcesOnboarding { get; set; }
        
        public bool isInviteUsersOnboarding { get; set; }
        

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}