using System;

namespace Tayra.Services
{
    public class TeamViewGridDTO
    {
        public Guid SegmentId { get; set; }
        public TeamDTO[] Teams { get; set; }

        public class TeamDTO
        {
            public Guid TeamId { get; set; }
            public string Key { get; set; }
            public string Name { get; set; }
            public string AvatarColor { get; set; }
            public int MembersCount { get; set; }
        }
    }
}
