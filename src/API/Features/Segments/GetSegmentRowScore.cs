using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpGet("{segmentKey}/rawScore")]
        public async Task<GetSegmentRowScore.Result> GetSegmentRowScore([FromRoute] string segmentKey) =>
            await _mediator.Send(new GetSegmentRowScore.Query {SegmentKey = segmentKey});
    }

    public class GetSegmentRowScore
    {
        public record Query : IRequest<Result>
        {
            public string SegmentKey { get; init; }
        }

        public record Result
        {
            public int TasksCompleted { get; init; }
            public int AssistsGained { get; init; }
            public int TimeWorked { get; init; }
            public int DaysOnTayra { get; init; }
            public float TokensEarned { get; init; }
            public float TokensSpent { get; init; }
            public int ItemsBought { get; init; }
            public int QuestsCompleted { get; init; }
        }


        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var segment = await _db.Segments.FirstOrDefaultAsync(x => x.Key == msg.SegmentKey, token);
                segment.EnsureNotNull(msg.SegmentKey);

                return await (from r in _db.SegmentReportsDaily
                    where r.SegmentId == segment.Id
                    orderby r.DateId descending
                    select new Result
                    {
                        TasksCompleted = r.TasksCompletedTotal,
                        AssistsGained = r.AssistsTotal,
                        TimeWorked = r.TasksCompletionTimeTotal,
                        TokensEarned = r.CompanyTokensEarnedTotal,
                        TokensSpent = r.CompanyTokensSpentTotal,
                        ItemsBought = r.ItemsBoughtTotal,
                        QuestsCompleted = 0,
                        DaysOnTayra = (DateTime.UtcNow - segment.Created).Days
                    }).FirstOrDefaultAsync(token);
            }
        }
    }
}