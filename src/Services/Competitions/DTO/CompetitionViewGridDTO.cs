using System;
using Tayra.Common;

namespace Tayra.Services
{
    public class CompetitionViewGridDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIndividual { get; set; }
        public CompetitionStatus Status { get; set; }
        public DateTime? EndedAt { get; set; }
        public DateTime Created { get; set; }
        public CompetitionWinner Winner { get; set; }

        public class CompetitionWinner
        {
            public string Username { get; set; }
            public string Avatar { get; set; }
            public double? Score { get; set; }
        }
    }
}
