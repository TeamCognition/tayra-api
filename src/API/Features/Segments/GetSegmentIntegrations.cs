using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
using Tayra.Common;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpGet("{segmentKey}/integrations")]
        public async Task<GetSegmentIntegrations.Result[]> GetSegmentIntegrations([FromRoute] string segmentKey) => await _mediator.Send(new GetSegmentIntegrations.Query {SegmentKey = segmentKey});
    }

    public class GetSegmentIntegrations
    {
        public record Query : IRequest<Result[]>
        {
            public string SegmentKey { get; init; }
        }

        public record Result
        {
            public IntegrationType Type { get; init; }
            public IntegrationStatuses Status { get; init; }
            public int MembersCount { get; init; }
            public DateTime Created { get; init; }
            public DateTime LastModified { get; init; }
        }


        public class Handler : IRequestHandler<Query, Result[]>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result[]> Handle(Query msg, CancellationToken token)
            {
                var segment = await _db.Segments.Where(x => x.Key == msg.SegmentKey).FirstOrDefaultAsync(token);
                segment.EnsureNotNull(msg.SegmentKey);

                await Task.Delay(1, token);
                return
                    _db.Integrations.Where(x => x.ProfileId == null && x.SegmentId == segment.Id)
                        .Select(x => new Result
                        {
                            Type = x.Type,
                            Created = x.Created,
                            Status = x.Status,
                            LastModified = x.LastModified ?? x.Created,
                            MembersCount = _db.Integrations
                                .Where(y => y.Type == x.Type && y.SegmentId == segment.Id && x.ProfileId != null)
                                .GroupBy(y => y.ProfileId).Count()
                        })
                        .DistinctBy(x => x.Type)
                        .OrderByDescending(x => x.Created)
                        .ToArray();
            }
        }
    }
}