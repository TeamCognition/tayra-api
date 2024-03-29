﻿using System;
using Cog.DAL;

namespace Tayra.Models.Catalog
{
    public class TenantIdentity : ITimeStampedEntity
    {
        //Composite Key
        public string TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }

        public Guid IdentityId { get; set; }
        public virtual Identity Identity { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}