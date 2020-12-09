using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Cog.Core
{
    public class CogPrincipal : ClaimsPrincipal
    {
        public CogPrincipal(IPrincipal principal)
            : base(principal)
        {
        }

        public Guid IdentityId => Guid.Parse(this.FindFirstOrDefault(CogClaimTypes.IdentityId)
            .DefaultIfEmpty("00000000-0000-0000-0000-000000000000"));

        public Guid ProfileId => Guid.Parse(this.FindFirstOrDefault(CogClaimTypes.ProfileId)
            .DefaultIfEmpty("00000000-0000-0000-0000-000000000000"));

        public string EmailAddress => this.FindFirstOrDefault(CogClaimTypes.EmailAddress);

        public string CurrentTenantKey => this.FindFirstOrDefault(CogClaimTypes.CurrentTenantKey).DefaultIfEmpty("-1");
    }
}