using System;
using Firdaws.Core;

namespace Tayra.Services
{
    public static class ProfileRules
    {
        public static bool CanUpProfile(int upperId, int profileToUpId, int? lastUppedAt)
        {
            return upperId != profileToUpId
                &&
                (!lastUppedAt.HasValue || DateHelper2.ToDateId(DateTime.UtcNow) > lastUppedAt);
        }
    }
}
