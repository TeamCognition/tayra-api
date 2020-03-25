using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class AdvisorSegmentGridDTO
    {          
        public ActionPointTypes Type { get; set; }
        public ProfileDTO[] ImpactedMembers { get; set; }
        public DateTime Created { get; set; }

        public class ProfileDTO
        {
            public int? ProfileId { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
        }
    }
}
