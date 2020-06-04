using System.Security.Claims;
using System.Security.Principal;

namespace Cog.Core
{
    public class CogPrincipal : ClaimsPrincipal
    {
        public int IdentityId => int.Parse(this.FindFirstOrDefault(CogClaimTypes.IdentityId).DefaultIfEmpty("0"));
        
        public int ProfileId => int.Parse(this.FindFirstOrDefault(CogClaimTypes.ProfileId).DefaultIfEmpty("0"));

        public string EmailAddress => this.FindFirstOrDefault(CogClaimTypes.EmailAddress);

        public string CurrentTenantKey => this.FindFirstOrDefault(CogClaimTypes.CurrentTenantKey).DefaultIfEmpty("-1");

        public CogPrincipal(IPrincipal principal)
            : base(principal)
        {
        }
    }
}
