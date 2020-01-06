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

        public static bool IsCompletionLimitValid(int? completitionLimit, int? itemRewardQuantity)
        {
            return (!completitionLimit.HasValue || completitionLimit > 0)
                && (!itemRewardQuantity.HasValue || (!completitionLimit.HasValue && completitionLimit.Value >= itemRewardQuantity.Value));
        }

        public static bool CanGoalBeCompleted(ChallengeStatuses status)
        {
            return ChallengeStatuses.Active == status;
        }
    }
}
