using System;
using Cog.Core;

namespace Tayra.Services
{
    public static class ProfileRules
    {
        public static bool CanPraiseProfile(Guid upperId, Guid profileToUpId, int? lastUppedAt, string message)
        {
            return upperId != profileToUpId
                && (!lastUppedAt.HasValue || DateHelper2.ToDateId(DateTime.UtcNow) > lastUppedAt)
                && (string.IsNullOrEmpty(message) || message.Length <= 140);
        }
    }
}
