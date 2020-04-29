using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ChallengeViewGridDTO
    {
        public int ChallengeId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public ChallengeStatuses Status { get; set; }

        public float RewardValue { get; set; }

        public int? CompletionsRemaining { get; set; }

        public DateTime Created { get; set; }
        public DateTime? ActiveUntil { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
