using System;
using System.Linq;
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
        public int CurrentOrganizationId => int.Parse(this.FindFirstOrDefault(TayraClaimTypes.CurrentOrganizationId).DefaultIfEmpty("0"));

        public ProfileRoles Role => Enum.Parse<ProfileRoles>(this.FindFirstOrDefault(TayraClaimTypes.Role).DefaultIfEmpty(ProfileRoles.Member.ToString()));
        
        public int[] SegmentsIds => this.FindAll(TayraClaimTypes.Segment).Select(x => int.Parse(x.Value)).ToArray();
        public int[] TeamsIds => this.FindAll(TayraClaimTypes.Team).Select(x => int.Parse(x.Value)).ToArray();
    }
}
