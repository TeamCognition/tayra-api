using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Result = Cog.Core.GridData<Tayra.API.Features.Segments.Search.ResultDto>;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpPost("search")]
        public async Task<ActionResult<Result>> Search([FromBody] Search.Query gridParams)
        {
            gridParams.SegmentIds = CurrentUser.SegmentsIds;
            return await _mediator.Send(gridParams);
        }
    }
    
    public class Search
    {
        public class Query : GridParams, IRequest<Result>
        {
            public Guid[] SegmentIds { get; set; }
        }

        public class ResultDto
        {
            public Guid SegmentId { get; set; }
            public string Name { get; set; }
            public string Key { get; set; }
            public string Avatar { get; set; }
            public DateTime Created { get; set; }

            public int QuestsActive { get; set; }
            public int QuestsCompleted { get; set; }
            public int ShopItemsBought { get; set; }
            public IntegrationType[] Integrations { get; set; }
            public int ActionPointsCount { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var query = from s in _db.Segments
                    where msg.SegmentIds.Contains(s.Id)
                    select new ResultDto
                    {
                        SegmentId = s.Id,
                        Name = s.Name,
                        Key = s.Key,
                        Avatar = s.Avatar,
                        Created = s.Created,
                        // QuestsActive = s.Quests.Count(x => x.Status == QuestStatuses.Active),
                        // QuestsCompleted = s.Quests.Count(x => x.Status == QuestStatuses.Ended),
                        ShopItemsBought = s.ShopPurchases.Count(x => x.Status == ShopPurchaseStatuses.Fulfilled),
                        Integrations = s.Integrations.Where(x => x.ProfileId == null).Select(x => x.Type).ToArray(),
                        ActionPointsCount = s.ActionPoints.Where(x => x.ConcludedOn == null).Select(x => x.Type).Distinct().Count()
                    };

                await Task.Delay(1, token);
                
                GridData<ResultDto> gridData = query.GetGridData(msg);

                return gridData;
            }
        }
    }
}