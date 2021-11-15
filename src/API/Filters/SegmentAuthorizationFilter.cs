using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Tayra.API.Controllers;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Filters
{
    public class SegmentAuthorizationFilter : IActionFilter
    {
        #region Constructor

        public SegmentAuthorizationFilter(OrganizationDbContext db)
        {
            _db = db;
        }

        #endregion

        #region Properties

        private OrganizationDbContext _db { get; set; }

        #endregion

        #region Public Methods

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = (BaseDataController)context.Controller;
            if (controller != null)
            {
                var segmentKey = Convert.ToString(controller.RouteData.Values["segment"]);

                var isAdmin = true;//controller.CurrentUser.Role == Roles.Admin; bug calls LoadUser when there is no user
                var hasSegmentAccess = true;//controller.CurrentUser.Any(b => b.Segment.Key == segmentKey);
                if (isAdmin || hasSegmentAccess)
                {
                    controller.CurrentSegment = _db.Segments.AsNoTracking().FirstOrDefault(x => x.Key == segmentKey);
                    if (controller.CurrentSegment == null)
                    {
                        throw new ApplicationException("Unable to get segment with key " + segmentKey);
                    }
                    return;
                }

                //context.Result = new UnauthorizedResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //if (context.Exception != null)
            //{
            //    context.ExceptionHandled = true;
            //    context.Result = new BadRequestObjectResult(context.Exception.Message);
            //}
        }

        #endregion
    }
}