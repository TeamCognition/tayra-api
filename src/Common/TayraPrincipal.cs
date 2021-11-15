using System;
using System.Linq;
using System.Security.Principal;
using Cog.Core;

namespace Tayra.Common
{
    public class TayraPrincipal : CogPrincipal
    {
        public TayraPrincipal(IPrincipal principal) : base(principal)
        {
        }

        public ProfileRoles Role => Enum.Parse<ProfileRoles>(this.FindFirstOrDefault(TayraClaimTypes.Role).DefaultIfEmpty(ProfileRoles.Member.ToString()));

        public Guid[] SegmentsIds => this.FindAll(TayraClaimTypes.Segment).Select(x => Guid.Parse(x.Value)).ToArray();
        public Guid[] TeamsIds => this.FindAll(TayraClaimTypes.Team).Select(x => Guid.Parse(x.Value)).ToArray();
    }
}
