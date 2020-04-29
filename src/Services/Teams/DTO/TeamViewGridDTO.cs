namespace Tayra.Services
{
    public class TeamViewGridDTO
    {
        public int SegmentId { get; set; }
        public TeamDTO[] Teams { get; set; }

        public class TeamDTO
        {
            public int TeamId { get; set; }
            public string Key { get; set; }
            public string Name { get; set; }
            public string AvatarColor { get; set; }
            public int MembersCount { get; set; }
        }
    }
}
