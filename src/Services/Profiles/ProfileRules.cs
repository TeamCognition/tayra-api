using System;
using Firdaws.Core;

namespace Tayra.Services
{
    public static class ProfileRules
    {
        public static bool CanPraiseProfile(int upperId, int profileToUpId, int? lastUppedAt, string message)
        {
            return upperId != profileToUpId
                && (!lastUppedAt.HasValue || DateHelper2.ToDateId(DateTime.UtcNow) > lastUppedAt)
                && (string.IsNullOrEmpty(message) || message.Length <= 140);
        }
    }
}
