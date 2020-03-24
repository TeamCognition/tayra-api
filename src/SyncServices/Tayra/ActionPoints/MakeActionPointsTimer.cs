using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices
{
    public class MakeActionPointsTimer
    {
        //[FunctionName(nameof(MakeActionPointsTimer))] //Runs every hour
        //public static void Run([TimerTrigger("0 0 * * * *")]TimerInfo timerInfo, ExecutionContext context, ILogger logger)
        //{
        //    SyncHelper.RunFromSchedule(JobTypes.MakeActionPoints, timerInfo, context, logger);
        //}
    }
}
