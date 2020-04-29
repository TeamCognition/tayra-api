using System;
using System.Collections.Generic;
using Firdaws.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Challenge : IAuditedEntity
    {
        public int Id { get; set; }

        public ChallengeStatuses Status { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        
        public int? CompletionsLimit { get; set; }
        public int? CompletionsRemaining { get; set; }

        public bool IsEasterEgg { get; set; }
        public bool IsArchived { get; set; }

        public DateTime? ActiveUntil { get; set; }
        public DateTime? EndedAt { get; set; }

        public float RewardValue { get; set; }

        public virtual ICollection<ChallengeGoal> Goals { get; set; }
        public virtual ICollection<ChallengeCommit> Commits { get; set; }
        public virtual ICollection<ChallengeReward> Rewards { get; set; }
        public virtual ICollection<ChallengeSegment> Segments { get; set; }
        
        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}