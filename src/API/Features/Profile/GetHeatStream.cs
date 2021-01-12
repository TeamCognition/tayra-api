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
        [HttpGet("heatStream/{profileId:int}")]
        public async Task<GetHeatStream.Result> GetProfileHeatStream([FromUri]Guid profileId)
            => await _mediator.Send(new GetHeatStream.Query { ProfileId = profileId});
    }
    
    public class GetHeatStream
    {
        public record Query : IRequest<Result>
        {
            public Guid ProfileId { get; init; }
        }

        public class Result : Dictionary<int, MetricService.AnalyticsMetricWithIterationSplitDto> {}
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(GetHeatStream.Query msg, CancellationToken token)
            {
                var metricService = new MetricService(_db);

                var metricList = new[]
                {
                    MetricType.Heat
                };
                await Task.Delay(1, token);
                return (Result)metricService.GetMetricsWithIterationSplit(
                    metricList, msg.ProfileId, EntityTypes.Profile,
                    new DatePeriod(DateTime.UtcNow.AddDays(-1 * (8 * 7 - 1)), DateTime.UtcNow));
            }
        }
    }
}