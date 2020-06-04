using System;
using System.Collections.Generic;
using Cog.Core;

namespace Tayra.Services
{
    public class ChallengeCreateDTO : DTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public int? CompletionsLimit { get; set; }

        public bool IsEasterEgg { get; set; }

        public DateTime? ActiveUntil { get; set; }

        public List<int> Segments { get; set; }
        public List<GoalDTO> Goals { get; set; }
        public List<RewardDTO> Rewards { get; set; }        

        public class RewardDTO
        {
            public int ItemId { get; set; }
            public int Quantity { get; set; }
        }

        public class GoalDTO
        {
            public string Title { get; set; }
            public bool IsCommentRequired { get; set; }
        }
    }
}
