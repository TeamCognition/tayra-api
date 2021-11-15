using System;
using System.Collections.Generic;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    [ApiController]
    public class SegmentsController : BaseController
    {
        #region Constructor

        public SegmentsController(OrganizationDbContext dbContext, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext DbContext { get; set; }

        #endregion

        #region Action Methods


        #endregion
    }
}