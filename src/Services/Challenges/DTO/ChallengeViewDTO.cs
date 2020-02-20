using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class ChallengeViewDTO
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ChallengeStatuses Status { get; set; }

        public float RewardValue { get; set; }

        public int? CompletionsLimit { get; set; }
        public int? CompletionsRemaining { get; set; }

        public DateTime Created { get; set; }
        public DateTime? ActiveUntil { get; set; }
        public DateTime? EndedAt { get; set; }

        public DateTime? CommittedOn { get; set; }

        public ICollection<int> Segments { get; set; }
        public ICollection<GoalDTO> Goals { get; set; }
        public ICollection<RewardDTO> Rewards { get; set; }

        public class RewardDTO
        {
            public int ItemId { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public ItemTypes Type { get; set; }
            public ItemRarities Rarity { get; set; }
            public float Price { get; set; }
        }

        public class GoalDTO
        {
            public int GoalId { get; set; }
            public string Title { get; set; }
            public string Comment { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsCommentRequired { get; set; }
        }
    }
}
