using System;
using Tayra.Common;

namespace Tayra.Services
{
    public static class QuestRules
    {
        public static bool IsActiveUntilValid(DateTime? activeUntil)
        {
            return activeUntil == null || activeUntil > DateTime.UtcNow;
        }

        public static bool CanBeCompleted(DateTime? activeUntil, int? rewardsLeft, QuestStatuses status)
        {
            return (activeUntil == null || activeUntil > DateTime.UtcNow)
                && (rewardsLeft == null || rewardsLeft > 0)
                && QuestStatuses.Active == status;
        }

        public static bool CanBeEnded(QuestStatuses status)
        {
            return status == QuestStatuses.Active;
        }

        public static bool IsCompletionLimitValid(int? completitionLimit)
        {
            return !completitionLimit.HasValue || completitionLimit > 0;
                //&& (!itemRewardQuantity.HasValue || (!completitionLimit.HasValue && completitionLimit.Value >= itemRewardQuantity.Value));
        }

        public static bool CanGoalBeCompleted(QuestStatuses status)
        {
            return QuestStatuses.Active == status;
        }
    }
}
