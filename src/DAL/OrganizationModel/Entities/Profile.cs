using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Profile : IAuditedEntity
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        public string Username { get; set; }

        [MaxLength(2000)]
        public string Avatar { get; set; }

        public ProfileRoles Role { get; set; }

        [MaxLength(100)]
        public string JobPosition { get; set; }

        public DateTime? BornOn { get; set; }
        public DateTime? EmployedOn { get; set; }

        public int IdentityId { get; set; }

        public virtual ICollection<ProfileInventoryItem> InventoryItems { get; set; }
        public virtual ICollection<TeamMember> Teams { get; set; }
        public virtual ICollection<TokenTransaction> Tokens { get; set; }
        public virtual ICollection<ProfileReportDaily> StatsDaily { get; set; }
        public virtual ICollection<ProfileReportWeekly> StatsWeekly { get; set; }
        public virtual ICollection<ChallengeCompletion> CompletedChallenges { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}