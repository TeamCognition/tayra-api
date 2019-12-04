using System;
using Firdaws.Core;

namespace Tayra.Services
{
    public class ChallengeCreateDTO : DTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public double TokenRewardValue { get; set; }
        public string CustomReward { get; set; }

        public int? CompletionsLimit { get; set; }

        public bool IsEasterEgg { get; set; }

        public DateTime? ActiveUntil { get; set; }
    }
}
