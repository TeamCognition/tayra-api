using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tayra.Common
{
    [ApiController, Authorize, Route("[controller]")]
    public class TayraBaseController : ControllerBase
    {
        private TayraPrincipal _currentUser;
        public TayraPrincipal CurrentUser => _currentUser ??= new TayraPrincipal(User);
    }
}