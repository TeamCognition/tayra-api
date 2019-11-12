using System;
using Microsoft.AspNetCore.Mvc;

namespace Tayra.API.Controllers
{
    public class TokensController : BaseController
    {
        #region Constructor

        public TokensController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Action Methods

        [HttpGet("")]
        public IActionResult GetCurrentUser()
        {
            return Ok(TokensService.GetTokenLookupDTO());
        }

        #endregion
    }
}