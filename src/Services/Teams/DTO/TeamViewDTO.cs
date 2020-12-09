using System;

namespace Tayra.Services
{
    public class TeamViewDTO
    {
        public Guid TeamId { get; set; }
        public string TeamKey { get; set; }
        public string Name { get; set; }
        public string AvatarColor { get; set; }
        public string AssistantSummary { get; set; }
        public DateTime Created { get; set; }
    }
}
