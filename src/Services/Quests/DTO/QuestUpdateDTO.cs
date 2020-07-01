using System;
using System.Collections.Generic;
using Cog.Core;

namespace Tayra.Services
{
    public class QuestUpdateDTO : DTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        //HARD...
        //public int? CompletionsRemaining { get; set; }

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
            public int? GoalId { get; set; }
            public string Title { get; set; }
            public bool IsCommentRequired { get; set; }
        }

        //Doesn't update status
        //Doesn't update Completition Limit
    }
}
