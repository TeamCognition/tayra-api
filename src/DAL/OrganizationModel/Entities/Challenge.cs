using System;
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

        public double TokenRewardValue { get; set; }
        public string CustomReward { get; set; }
        
        public int? CompletionsLimit { get; set; }
        public int? CompletionsRemaining { get; set; }

        public bool IsEasterEgg { get; set; }
        public bool IsArchived { get; set; }

        public DateTime? ActiveUntil { get; set; }
        public DateTime? EndedAt { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}