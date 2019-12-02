using System;
using System.Security.Principal;
using Firdaws.Core;

namespace Tayra.Common
{
    public class TayraPrincipal : FirdawsPrincipal
    {
        public TayraPrincipal(IPrincipal principal) : base(principal)
        {
        }

        /// <summary>
        /// Organization ID in which user is currently logged in.
        /// </summary>
        public int CurrentOrganizationId => Convert.ToInt32(this.FindFirstOrDefault(TayraClaimTypes.CurrentOrganizationId).DefaultIfEmpty("0"));
    }
}
