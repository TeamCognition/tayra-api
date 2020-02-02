using System;

namespace Tayra.Services
{
    public class TeamViewGridDTO
    {
        public int TeamId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string AvatarColor { get; set; }
        public int MembersCount { get; set; }
        public SegmentDTO Segment { get; set; }
    }

    public class SegmentDTO
    {
        public int SegmentId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}
