using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpGet]
        public async Task<GetSegment.Result> GetSegment([FromUri] string segmentKey) =>
            await _mediator.Send(new GetSegment.Query {SegmentKey = segmentKey});
    }

    public class GetSegment
    {
        public record Query : IRequest<Result>
        {
            public string SegmentKey { get; init; }
        }

        public record Result
        {
            public Guid SegmentId { get; init; }
            public string Name { get; init; }
            public string Key { get; init; }
            public string Avatar { get; init; }
            public string AssistantSummary { get; init; }

            public double TokensEarned { get; init; }
            public double TokensSpent { get; init; }
            public int QuestsActive { get; init; }
            public int QuestsCompleted { get; init; }
            public int ShopItemsBought { get; init; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var segment = await _db.Segments
                    .Where(x => x.Key == msg.SegmentKey)
                    .Select(x => new Result
                    {
                        SegmentId = x.Id,
                        Name = x.Name,
                        Key = x.Key,
                        Avatar = x.Avatar,
                        AssistantSummary = x.AssistantSummary,
                        TokensEarned =
                            Math.Round(
                                x.ReportsDaily.OrderByDescending(r => r.DateId).Select(r => r.CompanyTokensEarnedTotal)
                                    .FirstOrDefault(), 2),
                        TokensSpent =
                            Math.Round(
                                x.ReportsDaily.OrderByDescending(r => r.DateId).Select(r => r.CompanyTokensSpentTotal)
                                    .FirstOrDefault(), 2),
                        QuestsActive = x.Quests.Count(r => r.Status == QuestStatuses.Active),
                        QuestsCompleted = x.Quests.Count(r => r.Status == QuestStatuses.Ended),
                        ShopItemsBought = x.ShopPurchases.Count(r => r.Status == ShopPurchaseStatuses.Fulfilled)
                    }).FirstOrDefaultAsync(token);
                segment.EnsureNotNull(msg.SegmentKey);

                return segment;
            }
        }
    }
}