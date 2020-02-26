namespace Tayra.Services
{
    public class ProfileSummaryGridDTO
    {
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
        public Team[] Teams { get; set; }
        public int? OneUps { get; set; }
        public int CompletedChallenges { get; set; }
        public float? Speed { get; set; } = 0;
        public float? Power { get; set; } = 0;
        public float? Impact { get; set; } = 0;
        public float? TokensTotal { get; set; } = 0;

        public class Team
        {
            public string Name { get; set; }
            public string Key { get; set; }
        }
    }
}
