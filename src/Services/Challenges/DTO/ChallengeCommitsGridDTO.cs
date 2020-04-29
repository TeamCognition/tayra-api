using System;

namespace Tayra.Services
{
    public class ChallengeCommitsGridDTO
    {
        public int ProfileId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public DateTime CommittedOn { get; set; }
    }
}
