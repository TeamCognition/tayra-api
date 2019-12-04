using System;
namespace Tayra.Services
{
    public class ProfileCompletedChallengesGridDTO
    {
        public int ChallengeId { get; set; }
        public string ChallengeName { get; set; }
        public string ChallengeImage { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
