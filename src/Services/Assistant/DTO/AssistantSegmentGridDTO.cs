using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class AssistantSegmentGridDTO
    {
        public ActionPointTypes Type { get; set; }
        public ProfileDTO[] ImpactedMembers { get; set; }
        public int Created { get; set; }
        public class ProfileDTO
        {
            public Guid ActionPointId { get; set; }
            public string FullName { get; set; }
            public string Username { get; set; }
            public string Avatar { get; set; }
            public DateTime Created { get; set; }
        }
    }
}
