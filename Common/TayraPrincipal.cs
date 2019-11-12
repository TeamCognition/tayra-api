using System;
using System.Security.Claims;
using System.Security.Principal;
using Firdaws.Core;

namespace Tayra.Common
{
    public class TayraPrincipal : FirdawsPrincipal
    {
        public static new TayraPrincipal Current
        {
            get { return new TayraPrincipal(ClaimsPrincipal.Current); }
        }

        public TayraPrincipal(IPrincipal principal) : base(principal)
        {
        }

        /// <summary>
        /// ID organizacije u kojoj je korisnik trenutno logovan.
        /// </summary>
        public int CurrentOrganizationId => Convert.ToInt32(this.FindFirstOrDefault(TayraClaimTypes.CurrentOrganizationId).DefaultIfEmpty("0"));
    }
}
