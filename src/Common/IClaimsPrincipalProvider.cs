using System.Security.Claims;

namespace Tayra.Common
{
    public interface IClaimsPrincipalProvider<T> where T : ClaimsPrincipal
    {
        T Principal { get; set; }
    }
}
