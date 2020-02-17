using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileCommittedChallengesGridDTO
    {
        public int ChallengeId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public ChallengeStatuses Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CommittedAt { get; set; }
    }
}
