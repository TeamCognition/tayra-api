﻿using System;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ProfileInventoryItem : EntityGuidId, IClaimableEntity, ITimeStampedEntity
    {
        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }

        public bool IsActive { get; set; }
        public ItemTypes ItemType { get; set; }

        public InventoryAcquireMethods AcquireMethod { get; set; }
        public string AcquireDetail { get; set; }

        #region IClaimableEntity

        public bool ClaimRequired { get; set; }
        public DateTime? ClaimedAt { get; set; }

        #endregion

        #region ITimeStampedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion
    }
}