using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class IdentityManageGridDTO
    {
        public Guid ProfileId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public ProfileRoles Role { get; set; }
        public DateTime JoinedAt { get; set; }

        public ICollection<IntegrationDTO> Integrations { get; set; }

        public class IntegrationDTO
        {
            public IntegrationType Type { get; set; }
        }

        public SegmentDataDTO[] Segments;
        public class SegmentDataDTO
        {
            public string SegmentKey { get; set; }
            public string Name { get; set; }
        }

        public TeamDataDTO[] Teams { get; set; }
        public class TeamDataDTO
        {
            public string TeamKey { get; set; }
            public string Name { get; set; }
        }
    }
}
