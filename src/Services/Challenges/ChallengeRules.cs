using System;
using Tayra.Common;

namespace Tayra.Services
{
    public static class ChallengeRules
    {
        public static bool IsActiveUntilValid(DateTime? activeUntil)
        {
            return activeUntil == null || activeUntil > DateTime.UtcNow;
        }

        public static bool CanBeCompleted(DateTime? activeUntil, int? rewardsLeft, ChallengeStatuses status)
        {
            return (activeUntil == null || activeUntil > DateTime.UtcNow)
                && (rewardsLeft == null || rewardsLeft > 0)
                && ChallengeStatuses.Active == status;
        }

        public static bool CanBeEnded(ChallengeStatuses status)
        {
            return status == ChallengeStatuses.Active;
        }
    }
}
