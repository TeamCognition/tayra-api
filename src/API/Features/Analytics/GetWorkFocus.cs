using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;

namespace Tayra.API.Features.Analytics
{
    public partial class AnalyticsController
    {
        [HttpGet("workFocus")]
        public async Task<GetWorkFocus.Result> GetWorkFocus([FromQuery] string period, [FromQuery] GetWorkFocus.Query query)
        {
            var datePeriod = new DatePeriod(period);
            
            return await _mediator.Send(query with {Period = datePeriod});
        }
    }
    
    public class GetWorkFocus
    {
        public record Query : IRequest<Result>
        {
            public Guid ProfileId { get; init; }
            public Guid SegmentId { get; init; }
            public DatePeriod Period { get; init; }
        }
        
        public record Result
        {
            public int[] TotalTasks { get; set; }

            public int[] NewWorkBugIndex { get; set; }

            public int[] ComplexityTasksIndex { get; set; }

            public double[] SteadyVolatileIndex { get; set; }

        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var metricService = new MetricService(_db);
                
                var profileHeatMetric = metricService.GetMetricsWithIterationSplit(new []
                {
                    MetricType.Heat
                }, msg.ProfileId, EntityTypes.Profile, msg.Period);

                var teamHeatMetric = metricService.GetMetricsWithIterationSplit(new []
                {
                    MetricType.Heat
                }, msg.SegmentId, EntityTypes.Segment, msg.Period);

                double userHeatStandardDeviation = StandardDeviation(Variance(profileHeatMetric[MetricType.Heat.Value].Iterations.Select(i => (double)i.Value).ToArray()));
                double teamHeatStandardDeviation = StandardDeviation(Variance(teamHeatMetric[MetricType.Heat.Value].Iterations.Select(i => (double)i.Value).ToArray()));

                static double StandardDeviation(double var)
                {
                    return Math.Sqrt(var);
                }

                static double Variance(double[] nums)
                {
                    double avg = nums.Average();
                    if (nums.Length > 1)
                    {
                        var sumOfSquares = 0.0;
                        foreach (var num in nums)
                        {
                            sumOfSquares += Math.Pow((num - avg), 2.0);
                        }
                        return sumOfSquares / (nums.Length - 1);
                    }
                    else { return 0.0; }
                }

                var tasks = await _db.Tasks.Where(x => x.Status == TaskStatuses.Done && x.SegmentId == msg.SegmentId && x.LastModifiedDateId >= msg.Period.FromId && x.LastModifiedDateId <= msg.Period.ToId)
                    .Select(x => new
                    {
                        AssigneeProfileId = x.AssigneeProfileId,
                        TeamId = x.TeamId,
                        Complexity = x.Complexity,
                        Status = x.Status,
                        Type = x.Type
                    }).ToArrayAsync(token);

                var userDoneTasks = tasks.Count(x => x.AssigneeProfileId == msg.ProfileId);
                var teamDoneTasks = tasks.Count(x => x.TeamId == msg.SegmentId);

                var userNewWorkBug = tasks.Count(x => x.Type != TaskTypes.Bug && x.AssigneeProfileId == msg.ProfileId);
                var teamNewWorkBug = tasks.Count(x => x.Type != TaskTypes.Bug && x.TeamId == msg.SegmentId);

                var userComplexityIndex = tasks.Count(x => x.Complexity <= 2 && x.AssigneeProfileId == msg.ProfileId) - tasks.Count(x => x.Complexity >= 3 && x.AssigneeProfileId == msg.ProfileId);
                var teamComplexityIndex = tasks.Count(x => x.Complexity <= 2 && x.TeamId == msg.SegmentId) - tasks.Count(x => x.Complexity >= 3 && x.TeamId == msg.SegmentId);

                return new Result
                {
                    TotalTasks = new[] { userDoneTasks, teamDoneTasks },
                    NewWorkBugIndex = new[] { userNewWorkBug, teamNewWorkBug },
                    ComplexityTasksIndex = new[] { userComplexityIndex, teamComplexityIndex },
                    SteadyVolatileIndex = new[] { userHeatStandardDeviation, teamHeatStandardDeviation }
                };
            }
        }
    }
}