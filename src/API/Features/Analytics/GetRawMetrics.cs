using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Analytics
{
    public partial class AnalyticsController
    {
        [HttpGet("rawMetrics")]
        public async Task<GetRawMetrics.Result> GetRawMetrics([FromQuery] int m, [FromQuery]string period, [FromQuery] Guid entityId, EntityTypes entityType)
        {
            var metricType = MetricType.FromValue(m) as PureMetric;
            var datePeriod = new DatePeriod(period);
            
            return await _mediator.Send(new GetRawMetrics.Query() {MetricType = metricType, Period = datePeriod, EntityId = entityId, EntityType = entityType});
        }
    }
    
    public class GetRawMetrics
    {
        public record Query : IRequest<Result>
        {
            [JsonIgnore]
            public PureMetric MetricType { get; init; }
            public Guid EntityId { get; init; }
            public EntityTypes EntityType { get; init; }
            [JsonIgnore]
            public DatePeriod Period { get; init; }
        }

        public class Result : TableData
        {
            public Result(Type dataType, object[] records) : base(dataType, records) { }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                await Task.Delay(1, token);
                return new Result(msg.MetricType.TypeOfRawMetric, msg.MetricType.GetRawMetrics(_db, msg.Period, msg.EntityId, msg.EntityType));
            }
        }
    }
}