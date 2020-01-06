using System;

namespace Tayra.Services
{
    public class TeamViewGridDTO
    {
        public string TeamKey { get; set; }
        public string Name { get; set; }
        public string AvatarColor { get; set; }
        public string Subtitle { get; set; }
        public DateTime Created { get; set; }
    }
}
