using System;

namespace Tayra.Services
{
    public class SegmentViewDTO
    {
        public Guid SegmentId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Avatar { get; set; }

        public string AssistantSummary { get; set; }

        public double TokensEarned { get; set; }
        public double TokensSpent { get; set; }
        public int QuestsActive { get; set; }
        public int QuestsCompleted { get; set; }
        public int ShopItemsBought { get; set; }
    }
}
