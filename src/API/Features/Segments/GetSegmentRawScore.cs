using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpGet("{segmentKey}/rawScore")]
        public async Task<GetSegmentRawScore.Result> GetSegmentRowScore([FromRoute] string segmentKey) =>
            await _mediator.Send(new GetSegmentRawScore.Query {SegmentKey = segmentKey});
    }

    public class GetSegmentRawScore
    {
        public record Query : IRequest<Result>
        {
            public string SegmentKey { get; init; }
        }

        public record Result
        {
            public Dictionary<int, MetricValue> Metrics { get; init; }
            public int DaysOnTayra { get; init; }
        }


        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var segment = await _db.Segments.FirstOrDefaultAsync(x => x.Key == msg.SegmentKey, token);
                segment.EnsureNotNull(msg.SegmentKey);
            
                var metricTypes = new[] { MetricType.TasksCompleted, MetricType.Assists, MetricType.TimeWorked, MetricType.TokensEarned, MetricType.TokensSpent, MetricType.ItemsBought };
                
                var shards = (from m in _db.SegmentMetrics
                    where m.SegmentId == segment.Id
                    where metricTypes.Contains(m.Type)
                    select new MetricShard
                    {
                        Type = m.Type,
                        Value = m.Value,
                        DateId = m.DateId
                    }).ToArray();

                return new Result
                {
                    Metrics = metricTypes.ToDictionary(type => type.Value,
                        type => new MetricValue(type, new DatePeriod(segment.Created, DateTime.UtcNow), shards,
                            EntityTypes.Segment)),
                    DaysOnTayra = (DateTime.UtcNow - segment.Created).Days
                };
            }
        }
    }
}