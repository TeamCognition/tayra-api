using System;
namespace Tayra.Services
{
    public class SegmentTeamGridDTO
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string AvatarColor { get; set; }
        public int MembersCount { get; set; }
        public DateTime Created { get; set; }
    }
}
