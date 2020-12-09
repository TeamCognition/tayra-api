using System;

namespace Tayra.Services
{
    public class TeamCreateDTO
    {
        public Guid SegmentId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string AvatarColor { get; set; }
    }
}
