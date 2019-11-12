﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices
{
    public static class GenerateProjectReportsHttp
    {
        [FunctionName(nameof(GenerateProjectReportsHttp))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request,
            ExecutionContext context, ILogger logger)
        {
            SyncHelper.RunFromHttp(JobTypes.GenerateReportProject, request, context, logger);
            return new OkResult();
        }
    }
}
