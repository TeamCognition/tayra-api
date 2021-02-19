using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;

namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpGet("{teamId}/swarmPlot")]
        public async Task<GetTeamSwarmPlot.Result> GetTeamSwarmPlot([FromUri] Guid teamId)
            => await _mediator.Send(new GetTeamSwarmPlot.Query {TeamId = teamId});
    }

    public class GetTeamSwarmPlot
    {
        public record Query : IRequest<Result>
        {
            public Guid TeamId { get; init; }
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
                var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == msg.TeamId, token);
                team.EnsureNotNull(team,msg.TeamId);
                return await (from r in _db.TeamReportsDaily
                    where r.TeamId == msg.TeamId
                    orderby r.DateId descending
                    select new Result
                    {
                        TasksCompleted = r.TasksCompletedTotal,
                        AssistsGained = r.AssistsTotal,
                        TimeWorked = r.TasksCompletionTimeTotal,
                        TokensEarned = r.CompanyTokensEarnedTotal,
                        TokensSpent = r.CompanyTokensSpentTotal,
                        ItemsBought = r.ItemsBoughtTotal,
                        QuestsCompleted = r.QuestsCompletedTotal,
                        DaysOnTayra = (DateTime.UtcNow - team.Created).Days
                    }).FirstOrDefaultAsync(token);
            }
        }
    }
}