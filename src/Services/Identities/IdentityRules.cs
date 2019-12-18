using System;
namespace Tayra.Services
{
    public static class IdentityRules
    {
        public static bool IsPasswordValid(string password)
        {
            return password.Length >= 6 && !password.Contains(' ');
        }
    }
}
