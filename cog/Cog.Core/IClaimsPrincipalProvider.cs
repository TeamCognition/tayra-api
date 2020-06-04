using System.Security.Claims;

namespace Cog.Core
{
    public interface IClaimsPrincipalProvider<T> where T : ClaimsPrincipal
    {
        T Principal { get; set; }
    }
}
