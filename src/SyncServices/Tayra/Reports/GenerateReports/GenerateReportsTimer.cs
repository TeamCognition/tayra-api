using System;
using Cog.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateReportsTimer
    {
        [FunctionName(nameof(GenerateReportsTimer))] //Runs every hour
        public static void Run([TimerTrigger("0 0 * * * *")] TimerInfo timerInfo, ExecutionContext context, ILogger logger)
        {
            SyncHelper.RunFromSchedule(JobTypes.GenerateReports, timerInfo, context, logger);
        }
    }
}
