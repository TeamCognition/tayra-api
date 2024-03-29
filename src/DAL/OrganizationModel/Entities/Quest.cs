﻿using System;
using System.Collections.Generic;
using Cog.DAL;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class Quest : IAuditedEntity
    {
        public int Id { get; set; }

        public QuestStatuses Status { get; set; }

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

        public virtual ICollection<QuestGoal> Goals { get; set; }
        public virtual ICollection<QuestCommit> Commits { get; set; }
        public virtual ICollection<QuestReward> Rewards { get; set; }
        public virtual ICollection<QuestSegment> Segments { get; set; }

        #region IAuditedEntity

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }

        #endregion
    }
}