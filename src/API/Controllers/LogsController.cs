using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class LogsController : BaseController
    {
        #region Constructor

        public LogsController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<LogGridDTO>> Search([FromBody] LogGridParams gridParams)
        {
            if(!gridParams.ProfileId.HasValue && !string.IsNullOrEmpty(gridParams.ProfileUsername))
            {
                var x = ProfilesService.GetByUsername(gridParams.ProfileUsername);
                gridParams.ProfileId = x.Id;
            }

            return LogsService.GetGridData(gridParams);
        }


        #endregion
    }
}