using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Profile : Entity<Guid>, IAuditedEntity, IArchivableEntity
    {

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// unique
        /// </summary>
        [MaxLength(20)]
        public string Username { get; set; }

        [MaxLength(2000)]
        public string Avatar { get; set; }

        public ProfileRoles Role { get; set; }

        public bool IsAnalyticsEnabled { get; set; }

        [MaxLength(100)]
        public string JobPosition { get; set; }

        public DateTime? BornOn { get; set; }
        public DateTime? EmployedOn { get; set; }

        public string AssistantSummary { get; set; }

        public Guid IdentityId { get; set; }
        public bool isCreateProfileOnboarding { get; set; }

        public virtual ICollection<ProfilePraise> Praises { get; set; }
        public virtual ICollection<Integration> Integrations { get; set; }
        public virtual ICollection<ProfileInventoryItem> InventoryItems { get; set; }
        public virtual ICollection<ProfileAssignment> Assignments { get; set; }
        public virtual ICollection<TokenTransaction> Tokens { get; set; }
        public virtual ICollection<ProfileReportDaily> StatsDaily { get; set; }
        public virtual ICollection<ProfileReportWeekly> StatsWeekly { get; set; }
        public virtual ICollection<QuestCompletion> CompletedQuests { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}