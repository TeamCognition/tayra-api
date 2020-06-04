using Microsoft.AspNetCore.Http;
using Tayra.Common;

namespace Cog.Core
{
    public class TayraPrincipalProvider : IClaimsPrincipalProvider<TayraPrincipal>
    {
        public TayraPrincipalProvider(IHttpContextAccessor accessor)
        {
            Principal = new TayraPrincipal(accessor.HttpContext?.User);
        }

        public TayraPrincipal Principal { get; set; }
    }
}
