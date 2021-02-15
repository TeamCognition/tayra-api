using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Result = Cog.Core.GridData<Tayra.API.Features.Teams.Search.ResultDto>;
using Task = System.Threading.Tasks.Task;


namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpPost("search")]
        public async Task<Result> Search([FromBody] Search.Query gridParams)
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

        public record ResultDto
        {
            public Guid SegmentId { get; init; }
            public Team[] Teams { get; init; }

            public record Team
            {
                public Guid TeamId { get; init; }
                public string Key { get; init; }
                public string Name { get; init; }
                public string AvatarColor { get; init; }
                public int MembersCount { get; init; }
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                await Task.Delay(1, token);
                var query = from s in _db.Segments
                    where msg.SegmentIds.Contains(s.Id)
                    select new ResultDto
                    {
                        SegmentId = s.Id,
                        Teams = s.Teams.Select(x => new ResultDto.Team
                        {
                            TeamId = x.Id,
                            Key = x.Key,
                            Name = x.Name,
                            AvatarColor = x.AvatarColor,
                            MembersCount = x.Members.Count()
                        }).ToArray()
                    };

                var gridData = query.GetGridData(msg);
                return gridData;
            }
        }
    }
}