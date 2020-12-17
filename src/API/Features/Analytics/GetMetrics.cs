using System;
using System.Collections.Generic;
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

namespace Tayra.API.Features.Analytics
{
    public partial class AnalyticsController
    {
        [HttpGet("metrics")]
        public async Task<GetMetrics.Result> GetMetrics([FromQuery] int[] m, [FromQuery]string period, [FromQuery] GetMetrics.Query query)
        {
            var metricTypes = m.Select(MetricType.FromValue).ToArray();
            var datePeriod = new DatePeriod(period);
            
            return await _mediator.Send(query with {MetricTypes = metricTypes, Period = datePeriod});
        }
    }
    
    public class GetMetrics
    {
        public record Query : IRequest<Result>
        {
            public MetricType[] MetricTypes { get; init; }
            public Guid EntityId { get; init; }
            public EntityTypes EntityType { get; init; }
            public DatePeriod Period { get; init; }
        }
        
        public class Result : Dictionary<int, MetricValue> { }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var rawMetrics = msg.MetricTypes.Concat(msg.MetricTypes.SelectMany(x => x.BuildingMetrics)).ToArray();
                rawMetrics = rawMetrics.Concat(rawMetrics.SelectMany(x => x.BuildingMetrics)).ToArray();

                MetricShard[] metrics = null;

                switch (msg.EntityType)
                {
                    case EntityTypes.Segment:
                        metrics = await (from m in _db.SegmentMetrics
                            where m.DateId >= msg.Period.FromId && m.DateId <= msg.Period.ToId
                            where m.SegmentId == msg.EntityId
                            where rawMetrics.Contains(m.Type)
                            select new MetricShard
                            {
                                Type = m.Type,
                                Value = m.Value,
                                DateId = m.DateId
                            }).ToArrayAsync(token);
                        break;
                    default:
                        metrics = await (from m in _db.ProfileMetrics
                            where m.DateId >= msg.Period.FromId && m.DateId <= msg.Period.ToId
                            where m.ProfileId == msg.EntityId
                            where rawMetrics.Contains(m.Type)
                            select new MetricShard
                            {
                                Type = m.Type,
                                Value = m.Value,
                                DateId = m.DateId
                            }).ToArrayAsync(token);
                        break;
                }

                return (Result) msg.MetricTypes.ToDictionary(type => type.Value,
                    type => new MetricValue(type, msg.Period, metrics, msg.EntityType));
            }
        }
    }
}