using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Competition : IAuditedEntity
    {
        public int Id { get; set; }

        public bool IsIndividual { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime? StartedAt { get; set; }
        public DateTime? ScheduledEndAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public double TokenRewardValue { get; set; }

        public CompetitionType Type { get; set; }
        public CompetitionTheme Theme { get; set; }
        public CompetitionStatus Status { get; set; }

        public bool RepeatWhenCompleted { get; set; }
        public int RepeatCount { get; set; }

        public int? PreviousCompetitionId { get; set; }
        public virtual Competition PreviousCompetition { get; set; }

        public int TokenId { get; set; }
        public virtual Token Token { get; set; }

        public int SegmentId { get; set; }
        public virtual Segment Segment { get; set; }

        public int? WinnerId { get; set; }

        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Competitor> Competitors { get; set; }
        public virtual ICollection<CompetitionReward> Rewards { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        #endregion
    }
}