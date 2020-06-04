using System;
using System.Security.Claims;

namespace Cog.Core
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Returns value of first found claim based on provided type, or null if not found.
        /// </summary>
        /// <param name="principal">Provided principal that contains claims collection to query against.</param>
        /// <param name="type">Type of claim to query.</param>
        /// <returns>Claim value.</returns>
        public static string FindFirstOrDefault(this ClaimsPrincipal principal, string type)
        {
            if (!principal.HasClaim((Claim x) => x.Type == type))
            {
                return null;
            }
            return principal.FindFirst(type).Value;
        }

        /// <summary>
        /// Checks whether the principal has value of claim set to boolean value.
        /// </summary>
        /// <param name="principal">ClaimsPrincipal instance.</param>
        /// <param name="type">Type of claim.</param>
        /// <param name="value">Value to be expected.</param>
        /// <returns>Indication whether claim has the required value.</returns>
        public static bool HasBoolClaim(this ClaimsPrincipal principal, string type, bool value)
        {
            string value2 = principal.FindFirstOrDefault(type);
            if (string.IsNullOrWhiteSpace(value2))
            {
                return false;
            }
            return value2.ToBool() == value;
        }
    }
}