namespace Tayra.Services
{
    public class SegmentViewDTO
    {
        public int SegmentId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Avatar { get; set; }

        public double TokensEarned { get; set; }
        public double TokensSpent { get; set; }
        public int ChallengesActive { get; set; }
        public int ChallengesCompleted { get; set; }
        public int ShopItemsBought { get; set; }
    }
}
