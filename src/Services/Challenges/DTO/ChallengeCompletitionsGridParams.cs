using System;

namespace Tayra.Services
{
    public class ChallengeCompletitionsGridDTO
    {
        public int ProfileId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
