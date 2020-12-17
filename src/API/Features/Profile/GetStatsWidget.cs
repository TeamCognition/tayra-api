using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Cog.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpGet("statsWidget/{profileId:int}")]
        public async Task<GetStatsWidget.Result> GetProfileStatsData([FromUri] Guid profileId)
            => await _mediator.Send(new GetStatsWidget.Query { ProfileId = profileId});
    }
    
    public class GetStatsWidget
    {
        public record Query : IRequest<Result>
        {
            public Guid ProfileId { get; init; }
        }

        public record Result
        {
            public DateTime? LastRefreshAt { get; set; }
            public Dictionary<int, MetricService.AnalyticsMetricWithIterationSplitDto> EntityMetrics { get; set; }
            public Dictionary<int, MetricService.AnalyticsMetricWithIterationSplitDto> ComparatorMetrics { get; set; }
        }
        
            
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var metricService = new MetricService(_db);

                var metricList = new []
                {
                    MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists,
                    MetricType.TasksCompleted, MetricType.Complexity, MetricType.CommitRate
                };

                var profileMetrics = metricService.GetMetricsWithIterationSplit(
                    metricList, msg.ProfileId, EntityTypes.Profile, new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow));

                var firstSegmentId = _db.ProfileAssignments.Where(x => x.ProfileId == msg.ProfileId)
                    .Select(x => x.SegmentId).FirstOrDefault();

                var segmentMetrics = metricService.GetMetricsWithIterationSplit(
                    metricList, firstSegmentId, EntityTypes.Segment, new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow));
                
                await Task.Delay(1, token);
                return new Result
                {
                    LastRefreshAt = _db.ProfileMetrics.OrderByDescending(x => x.DateId).Select(x => x.Created).FirstOrDefault(),
                    EntityMetrics = profileMetrics,
                    ComparatorMetrics = segmentMetrics
                };
            }
        }
    }
}