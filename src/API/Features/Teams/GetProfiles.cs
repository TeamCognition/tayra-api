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
using Result = Cog.Core.GridData<Tayra.API.Features.Teams.GetProfiles.ResultTeamProfilesDto>;


namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpPost("searchProfiles")]
        public async Task<Result> GetTeamProfiles([FromBody] GetProfiles.Query gridParams)
        {
            return await _mediator.Send(gridParams);
        }
    }

    public class GetProfiles
    {
        public class Query : GridParams, IRequest<Result>
        {
            public string TeamKey { get; init; }
        }

        //'TeamProfile' is a workaround for swagger bug
        public record ResultTeamProfilesDto
        {
            public Guid ProfileId { get; init; }
            public string Name { get; init; }
            public string Username { get; init; }
            public string Avatar { get; init; }
            public double Speed { get; init; }
            public double Power { get; init; }
            public double Impact { get; init; }
            public DateTime MemberFrom { get; init; }
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
                var team = await _db.Teams.FirstOrDefaultAsync(x => x.Key == msg.TeamKey, token);
                team.EnsureNotNull(msg.TeamKey);

                var scope = _db.ProfileAssignments
                    .Where(x => x.TeamId == team.Id);
                
                var query = from t in scope
                    select new ResultTeamProfilesDto
                    {
                        ProfileId = t.ProfileId,
                        Name = t.Profile.FirstName + " " + t.Profile.LastName,
                        Username = t.Profile.Username,
                        Avatar = t.Profile.Avatar,
                        Speed = -999,
                        Power = -999,
                        Impact = -999,
                        MemberFrom = t.Created
                    };

                var gridData = query.GetGridData(msg);

                return gridData;
            }
        }
    }
}