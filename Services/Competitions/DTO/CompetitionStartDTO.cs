using System;
using System.ComponentModel.DataAnnotations;

namespace Tayra.Services
{
    public class CompetitionStartDTO
    {
        public bool StartNow { get; set; }
        public DateTime? ScheduledStartAt { get; set; }

        [Required]
        public DateTime? ScheduledEndAt { get; set; }

        public TimeSpan Duration()
        {
            ScheduledStartAt = StartNow ? DateTime.UtcNow : ScheduledStartAt;
            if(!ScheduledStartAt.HasValue || !ScheduledEndAt.HasValue)
            {
                return new TimeSpan(0);
            }
            return ScheduledEndAt.Value - ScheduledStartAt.Value;
        }
    }
}
