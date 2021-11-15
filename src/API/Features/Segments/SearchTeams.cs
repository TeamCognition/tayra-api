using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;
using Result = Cog.Core.GridData<Tayra.API.Features.Segments.SearchTeams.ResultDto>;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpPost("{segmentKey}/searchTeams")]
        public async Task<ActionResult<Result>> SearchMembers([FromRoute] string segmentKey, [FromBody] SearchTeams.Query gridParams)
        {
            gridParams.SegmentKey = segmentKey;
            return await _mediator.Send(gridParams);
        }
    }
    
    public class SearchTeams
    {
        public class Query : GridParams, IRequest<Result>
        {
            public string SegmentKey { get; set; }
        }

        public class ResultDto
        {
            public string Name { get; set; }
            public string Key { get; set; }
            public string AvatarColor { get; set; }
            public int MembersCount { get; set; }
            public DateTime Created { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var segment = await _db.Segments.FirstOrDefaultAsync(s => s.Key == msg.SegmentKey, token);

                segment.EnsureNotNull(msg.SegmentKey);
                
                var query = from t in _db.Teams.Where(x => x.SegmentId == segment.Id)
                    select new ResultDto
                    {
                        Name = t.Name,
                        Key = t.Key,
                        AvatarColor = t.AvatarColor,
                        MembersCount = t.Members.Count(),
                        Created = t.Created
                    };

                var gridData = query.GetGridData(msg);

                return gridData;
            }
        }
    }
}