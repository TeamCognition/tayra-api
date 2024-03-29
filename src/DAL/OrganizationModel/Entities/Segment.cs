﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class Segment : EntityGuidId, IAuditedEntity, IArchivableEntity
    {
        [Required, MaxLength(50)]
        public string Key { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Avatar { get; set; }

        public decimal AllocatedBudget { get; set; }

        [MaxLength(50)]
        public string Timezone { get; set; }

        [MaxLength(4000)]
        public string DataStore { get; set; }

        [MaxLength(4000)]
        public string DataWarehouse { get; set; }

        public bool IsReportingUnlocked { get; set; }

        public string AssistantSummary { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Quest> Quests { get; set; }
        public virtual ICollection<Integration> Integrations { get; set; }
        public virtual ICollection<ProfileAssignment> Members { get; set; }
        public virtual ICollection<ProfileExternalId> MembersLinked { get; set; }
        public virtual ICollection<ShopPurchase> ShopPurchases { get; set; }
        public virtual ICollection<ActionPoint> ActionPoints { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}