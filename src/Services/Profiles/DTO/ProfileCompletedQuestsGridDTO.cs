using System;
namespace Tayra.Services
{
    public class ProfileCompletedQuestsGridDTO
    {
        public int QuestId { get; set; }
        public string QuestName { get; set; }
        public string QuestImage { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
