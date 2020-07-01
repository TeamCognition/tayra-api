using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileCommittedQuestsGridDTO
    {
        public int QuestId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public QuestStatuses Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CommittedAt { get; set; }
    }
}
