using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class ChallengeViewGridDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public ChallengeStatuses Status { get; set; }

        public double TokenRewardValue { get; set; }
        public string CustomReward { get; set; }

        public int? CompletionsRemaining { get; set; }

        public DateTime Created { get; set; }
        public DateTime? ActiveUntil { get; set; }
        public DateTime? EndedAt { get; set; }

        public int TotalCompletions { get; set; }
        public ICollection<Completion> Completions { get; set; }

        public class Completion
        {
            public int ProfileId { get; set; }
            public string ProfileNickname { get; set; }
            public string ProfileAvatar { get; set; }
            public DateTime CompletedAt { get; set; }
        }
    }
}
