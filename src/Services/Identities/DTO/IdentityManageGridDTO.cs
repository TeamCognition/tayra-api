using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public class IdentityManageGridDTO
    {
        public int ProfileId { get; set; }
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
            public int SegmentId { get; set; }
            public string Name { get; set; }
            public string Key { get; set; }
            public string Avatar { get; set; }
            public DateTime Created { get; set; }

            public int ChallengesActive { get; set; }
            public int ChallengesCompleted { get; set; }
            public int ShopItemsBought { get; set; }
            public IntegrationType[] Integrations { get; set; }
            public int ActionPointsCount { get; set; }
        }

        public TeamDataDTO[] Teams { get; set; }
        public class TeamDataDTO
        {
            public int TeamId { get; set; }
            public string TeamKey { get; set; }
            public string Name { get; set; }
            public string AvatarColor { get; set; }
            public DateTime Created { get; set; }
        }
    }
}
