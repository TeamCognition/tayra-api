﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices
{
    public class SyncIssuesHttp
    {
        [FunctionName(nameof(SyncIssuesHttp))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request,
            ExecutionContext context, ILogger logger)
        {
            SyncHelper.RunFromHttp(JobTypes.SyncIssues, request, context, logger);
            return new OkResult();
        }

    }
}