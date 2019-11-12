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

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }

        [MaxLength(2000)]
        public string Avatar { get; set; }

        public ProfileRoles Role { get; set; }

        public int IdentityId { get; set; }
        public virtual Identity Identity { get; set; }

        public virtual ICollection<ProfileInventoryItem> InventoryItems { get; set; }
        public virtual ICollection<ProjectMember> Projects { get; set; }
        public virtual ICollection<TeamMember> Teams { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
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