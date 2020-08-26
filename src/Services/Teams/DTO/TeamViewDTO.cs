using System;

namespace Tayra.Services
{
    public class TeamViewDTO
    {
        public int TeamId { get; set; }
        public string TeamKey { get; set; }
        public string Name { get; set; }
        public string AvatarColor { get; set; }
        public string AssistantSummary { get; set; }
        public DateTime Created { get; set; }
    }
}
