using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.Models.Profiles;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpGet("sessionCache")]
        public async Task<GetSessionCache.Result> GetSessionCache()
            => await _mediator.Send(new GetSessionCache.Query { IdentityId = CurrentUser.IdentityId, CurrentTenantKey = CurrentUser.CurrentTenantIdentifier});
    }
    
    public class GetSessionCache
    {
        public record Query : IRequest<Result>
        {
            public Guid IdentityId { get; init; }
            public string CurrentTenantKey { get; init; }
        }
        
        public record Result
        {
            public Guid ProfileId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }
            public string Avatar { get; set; }
            public ProfileRoles Role { get; set; }
            public bool IsAnalyticsEnabled { get; set; }

            public IList<ItemActiveDTO> Badges { get; set; }
            public ItemActiveDTO Title { get; set; }
            public ItemActiveDTO Border { get; set; }

            public ICollection<Team> Teams { get; set; }
            public ICollection<Segment> Segments { get; set; }

            public string TenantHost { get; set; }

            public class Segment
            {
                public Guid Id { get; set; }
                public string Key { get; set; }
                public string Name { get; set; }
                public string Avatar { get; set; }
            }

            public class Team
            {
                public Guid Id { get; set; }
                public string Key { get; set; }
                public string Name { get; set; }
                public string AvatarColor { get; set; }
                public Guid SegmentId { get; set; }
            }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var cache = await (from p in _db.Profiles.Where(x => x.IdentityId == msg.IdentityId)
                    select new Result
                    {
                        ProfileId = p.Id,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Username = p.Username,
                        Role = p.Role,
                        Avatar = p.Avatar,
                        IsAnalyticsEnabled = p.IsAnalyticsEnabled
                    }).FirstOrDefaultAsync(token);

                (IQueryable<Segment> qs, IQueryable<Team> qt) = Auth.Controllers.AuthorizationController.GetSegmentAndTeamQueries(_db, cache.ProfileId, cache.Role);

                cache.Segments = qs.Select(s => new Result.Segment
                {
                    Id = s.Id,
                    Key = s.Key,
                    Name = s.Name,
                    Avatar = s.Avatar
                }).ToArray();

                cache.Teams = qt.Select(t => new Result.Team
                {
                    Id = t.Id,
                    Key = t.Key,
                    Name = t.Name,
                    AvatarColor = t.AvatarColor,
                    SegmentId = t.SegmentId
                }).ToArray().ToArray();

                var activeItems = new ProfilesService().GetProfileActiveItems(_db, cache.ProfileId);

                cache.Title = activeItems.Title;
                cache.Badges = activeItems.Badges;
                cache.Border = activeItems.Border;

                cache.TenantHost = msg.CurrentTenantKey;
                return cache;
            }
        }
    }
}