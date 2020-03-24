using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class AdvisorSegmentGridDTO
    {          
        public ActionPointTypes Type { get; set; }
        public DateTime Created { get; set; }
        public ProfileDataDTO[] ImpactedMembers { get; set; }

        public class ProfileDataDTO
        {
            public int? ProfileId { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
        }
    }
}
