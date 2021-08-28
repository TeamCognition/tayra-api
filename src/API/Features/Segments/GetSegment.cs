using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public async Task<GetSegment.Result> GetSegment([FromQuery] string segmentKey) =>
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
            public int AllocatedBudget { get; init; }
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
                var segmentDto = await (from s in _db.Segments
                    where s.Key == msg.SegmentKey
                    select new Result
                    {
                        SegmentId = s.Id,
                        Name = s.Name,
                        Key = s.Key,
                        Avatar = s.Avatar,
                        AllocatedBudget = s.AllocatedBudget,
                        AssistantSummary = s.AssistantSummary,
                        TokensEarned = 0,
                        TokensSpent = 0,
                        QuestsActive = s.Quests.Count(x => x.Status == QuestStatuses.Active),
                        QuestsCompleted = s.Quests.Count(x => x.Status == QuestStatuses.Ended),
                        ShopItemsBought = s.ShopPurchases.Count(x => x.Status == ShopPurchaseStatuses.Fulfilled),
                    }).FirstOrDefaultAsync(token);

                segmentDto.EnsureNotNull(msg.SegmentKey);

                return segmentDto;
            }
        }
    }
}