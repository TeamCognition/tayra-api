using System;
using Tayra.Common;

namespace Tayra.Services
{
    public static class SegmentRules
    {
        public static bool CanAddProfileToSegment(ProfileRoles role, int? teamId)
        {
            return role == ProfileRoles.Manager || (role == ProfileRoles.Member && teamId.HasValue && teamId > 0);
        }

        public static bool CanRemoveProfileToSegment(ProfileRoles role, int? teamId)
        {
            return role == ProfileRoles.Manager || (role == ProfileRoles.Member && teamId.HasValue && teamId > 0);
        }
    }
}
