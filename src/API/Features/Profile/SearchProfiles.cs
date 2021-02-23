using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Cog.Core;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;
using Result = Cog.Core.GridData<Tayra.API.Features.Profile.SearchProfiles.ResultDto>;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpPost("search")]
        public async Task<Result> Search([FromBody] SearchProfiles.Query gridParams)
        {
            gridParams.ProfileId = CurrentUser.ProfileId;
            return await _mediator.Send(gridParams);
        }
    }
    
    public class SearchProfiles
    {
        public class Query : GridParams, IRequest<Result>
        {
            public Guid ProfileId { get; set; }
            
            public Guid[] SegmentIdsToInclude { get; set; }
            public string UsernameQuery { get; set; } = string.Empty; //prevent null reference exception
            public string NameQuery { get; set; } = string.Empty; //prevent null reference exception
            public bool? AnalyticsEnabledOnly { get; set; }
            public bool IncludeSearcher { get; set; } = false;
        }

        public record ResultDto
        {
            public Guid ProfileId { get; init; }
            public string Name { get; init; }
            public string Username { get; init; }
            public string Avatar { get; init; }
            public DateTime Created { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                IQueryable<Models.Organizations.Profile> scope = _db.Profiles;

                if (!msg.IncludeSearcher)
                {
                    scope = _db.Profiles.Where(x => x.Id != msg.ProfileId);
                } 

                Expression<Func<Models.Organizations.Profile, bool>> byUsername = x => x.Username.Contains(msg.UsernameQuery.RemoveAllWhitespaces());
                Expression<Func<Models.Organizations.Profile, bool>> byName = x => (x.FirstName + x.LastName).Contains(msg.NameQuery.RemoveAllWhitespaces());

                if (msg.SegmentIdsToInclude != null && msg.SegmentIdsToInclude.Any())
                {
                    var profileIds = await _db.ProfileAssignments.Where(x => msg.SegmentIdsToInclude.Contains(x.SegmentId)).Select(x => x.ProfileId).ToArrayAsync(token);
                    scope = scope.Where(x => profileIds.Contains(x.Id));
                }

                if (msg.AnalyticsEnabledOnly.HasValue)
                {
                    scope = scope.Where(x => x.IsAnalyticsEnabled);
                }
            
                if (!string.IsNullOrEmpty(msg.UsernameQuery) && !string.IsNullOrEmpty(msg.NameQuery))
                    scope = scope.Chain(ChainType.OR, byUsername, byName);
                else
                {
                    if (!string.IsNullOrEmpty(msg.UsernameQuery))
                        scope = scope.Where(byUsername);

                    if (!string.IsNullOrEmpty(msg.NameQuery))
                        scope = scope.Where(byName);
                }

                var query = from p in scope
                    select new ResultDto
                    {
                        ProfileId = p.Id,
                        Name = p.FirstName + " " + p.LastName,
                        Username = p.Username,
                        Avatar = p.Avatar,
                        Created = p.Created
                    };

                GridData<ResultDto> gridData = query.GetGridData(msg);

                return gridData;
            }
        }
    }
}