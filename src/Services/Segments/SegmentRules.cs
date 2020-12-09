using System;
using Tayra.Common;

namespace Tayra.Services
{
    public static class SegmentRules
    {
        public static bool CanAddProfileToSegment(ProfileRoles role, Guid? teamId)
        {
            if (teamId.HasValue)
                return true;
            return role == ProfileRoles.Manager;
        }

        public static bool CanRemoveProfileToSegment(ProfileRoles role, Guid? teamId)
        {
            return role == ProfileRoles.Manager || (role == ProfileRoles.Member && teamId.HasValue);
        }
    }
}
