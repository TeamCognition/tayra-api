using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Models.Organizations;

namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpGet("{teamId}/pulse")]
        public async Task<GetTeamPulse.Result> GetTeamPulse([FromQuery] Guid teamId)
        {
            return await _mediator.Send(new GetTeamPulse.Query {TeamId = teamId});
        }
    }

    public class GetTeamPulse
    {
        public record Query : IRequest<Result>
        {
            public Guid TeamId { get; init; }
        }

        public record Result
        {
            public int InProgress { get; init; }
            public int RecentlyDone { get; init; }
            public string JiraBoardUrl { get; init; }
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
                var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == msg.TeamId, token);

                team.EnsureNotNull(msg.TeamId);

                var teamMembers = await  _db.ProfileAssignments.Where(x => x.TeamId == team.Id).Select(x => x.ProfileId)
                    .ToArrayAsync(token);

                var yesterdayDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(-1));

                string jiraBoardUrl = null;

                //var segmentId = _db.Teams.FirstOrDefault(x => x.Id == msg.TeamId)?.SegmentId;
                //// can we use segmentId from team variable ?
                if (team?.SegmentId != null)
                {
                    var sFields = await _db.Integrations
                        .Where(x => x.SegmentId == team.SegmentId && x.ProfileId == null &&
                                    x.Type == IntegrationType.ATJ)
                        .Select(x => x.Fields)
                        .FirstOrDefaultAsync(token);
                    if (sFields != null)
                    {
                        var jiraSiteName = sFields.FirstOrDefault(x => x.Key == ATConstants.AT_SITE_NAME)?.Value;
                        jiraBoardUrl = $"https://{jiraSiteName}.atlassian.net/secure/RapidBoard.jspa?rapidView=6";
                    }
                }

                return await (from t in _db.Tasks
                    where teamMembers.Contains(t.AssigneeProfileId.Value)
                    where t.Status == WorkUnitStatuses.InProgress ||
                          t.Status == WorkUnitStatuses.Done && t.LastModifiedDateId >= yesterdayDateId
                    group t by 1
                    into g
                    select new Result
                    {
                        InProgress = g.Count(x => x.Status == WorkUnitStatuses.InProgress),
                        RecentlyDone = g.Count(x => x.Status == WorkUnitStatuses.Done),
                        JiraBoardUrl = jiraBoardUrl
                    }).FirstOrDefaultAsync(token);
            }
        }
    }
}