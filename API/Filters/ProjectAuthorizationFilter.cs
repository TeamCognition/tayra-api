using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tayra.API.Controllers;
using Tayra.Common;
using Tayra.Services;

namespace Tayra.API.Filters
{
    public class ProjectAuthorizationFilter : IActionFilter
    {
        #region Constructor

        public ProjectAuthorizationFilter(IProjectsService projectsService)
        {
            ProjectsService = projectsService;
        }

        #endregion

        #region Properties

        public IProjectsService ProjectsService { get; set; }

        #endregion

        #region Public Methods

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = (BaseDataController)context.Controller;
            if (controller != null)
            {
                var projectKey = Convert.ToString(controller.RouteData.Values["project"]);

                var isAdmin = true;//controller.CurrentUser.Role == Roles.Admin; bug calls LoadUser when there is no user
                var hasProjectAccess = true;//controller.CurrentUser.Any(b => b.Project.Key == projectKey);
                if (isAdmin || hasProjectAccess)
                {
                    controller.CurrentProject = ProjectsService.Get(projectKey);
                    if (controller.CurrentProject == null)
                    {
                        throw new ApplicationException("Unable to get project with key " + projectKey);
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