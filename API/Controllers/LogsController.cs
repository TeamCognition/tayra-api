﻿using System;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    [ApiController]
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

        [HttpPost("")] //TODO: name it search
        public ActionResult<GridData<LogGridDTO>> Search([FromBody] LogGridParams gridParams)
        {
            if (string.IsNullOrEmpty(gridParams.Sidx))
            {
                gridParams.Sidx = nameof(LogGridDTO.Created);
                gridParams.Sord = "DESC";
            }

            if(!gridParams.ProfileId.HasValue && !string.IsNullOrEmpty(gridParams.ProfileNickname))
            {
                var x = ProfilesService.GetByNickname(gridParams.ProfileNickname);
                gridParams.ProfileId = x.Id;
            }

            return LogsService.GetGridData(gridParams);
        }


        #endregion
    }
}