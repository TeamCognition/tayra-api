using Tayra.Common;

namespace Tayra.Services
{
    public static class IdentityRules
    {
        public static bool IsPasswordValid(string password)
        {
            return password.Length >= 6 && !password.Contains(' ');
        }

        public static bool CanChangeRole(ProfileRoles role, ProfileRoles newRole)
        {
            return role != ProfileRoles.Member && role <= newRole;
        }

        public static bool CanArchiveProfile(ProfileRoles role, ProfileRoles toArchiveRole)
        {
            return role != ProfileRoles.Member && role <= toArchiveRole;
        }
    }
}
