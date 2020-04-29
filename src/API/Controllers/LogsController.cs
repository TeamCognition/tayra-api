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
        public GridData<LogGridDTO> Search([FromBody] LogGridParams gridParams)
        {
            return LogsService.GetGridData(gridParams);
        }


        #endregion
    }
}