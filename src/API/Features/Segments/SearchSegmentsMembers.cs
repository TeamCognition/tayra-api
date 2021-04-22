using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Tayra.Common;
using Tayra.Models.Organizations;
using Result = Cog.Core.GridData<Tayra.API.Features.Segments.SearchSegmentsMembers.ResultDto>;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpPost("{segmentKey}/searchMembers")]
        public async Task<ActionResult<Result>> SearchMembers([FromRoute] string segmentKey, [FromBody] SearchSegmentsMembers.Query gridParams)
        {
            gridParams.SegmentKey = segmentKey;
            return await _mediator.Send(gridParams);
        }
    }
    
    public class SearchSegmentsMembers
    {
        public class Query : GridParams, IRequest<Result>
        {
            public bool? AnalyticsEnabledOnly { get; set; }
            public string SegmentKey { get; set; }
        }

        public class ResultDto
        {
            public Guid ProfileId { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
            public ProfileRoles Role { get; set; }
            public string Avatar { get; set; }
            public DateTime MemberFrom { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var segment = await _db.Segments.FirstOrDefaultAsync(s => s.Key == msg.SegmentKey, token);

                segment.EnsureNotNull(msg.SegmentKey);

                var scope = _db.ProfileAssignments.Where(x => x.SegmentId == segment.Id);

                if (msg.AnalyticsEnabledOnly.HasValue)
                {
                    scope = scope.Where(x => x.Profile.IsAnalyticsEnabled);
                }

                var query = from s in scope
                    select new ResultDto
                    {
                        ProfileId = s.Profile.Id,
                        Name = s.Profile.FirstName + " " + s.Profile.LastName,
                        Username = s.Profile.Username,
                        Role = s.Profile.Role,
                        Avatar = s.Profile.Avatar,
                        MemberFrom = s.Created
                    };

                GridData<ResultDto> gridData = query.GetGridData(msg);

                gridData.Records = MoreEnumerable.DistinctBy(gridData.Records, x => x.Username).ToList();

                return gridData;
            }
        }
    }
}