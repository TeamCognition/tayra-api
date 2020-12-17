using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Services.Models.Profiles;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpGet("me")]
        public async Task<View.Result> GetCurrentUser()
        {
            return await _mediator.Send(new View.Query {ProfileId = CurrentUser.ProfileId});
        }

        [HttpGet("{username}")]
        public async Task<View.Result> GetCurrentUser([FromRoute] string username)
        {
            return await _mediator.Send(new View.Query {ProfileId = CurrentUser.ProfileId, Username = username});
        }
    }
    
    public class View
    {
        public record Query : IRequest<Result>
        {
            public Guid ProfileId { get; set; }
            public string Username { get; init; } //nullable
        }

        public class Result
        {
            public Guid ProfileId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }

            public ProfilePulse Pulse { get; set; }

            public ProfileRoles Role { get; set; }
            public TeamDTO[] Teams { get; set; }
            public SegmentDTO[] Segments { get; set; }
            public string Avatar { get; set; }
            public double CompanyTokens { get; set; }
            public int Experience { get; set; }
            public string AssistantSummary { get; set; }

            public IList<ItemActiveDTO> Badges { get; set; }
            public ItemActiveDTO Title { get; set; }
            public ItemActiveDTO Border { get; set; }

            public DateTime? LastUppedAt { get; set; }

            public PraiseDTO[] Praises { get; set; }

            public class TokenDTO
            {
                public TokenType Type { get; set; }
                public double Value { get; set; }
            }

            public class HeatDTO
            {
                public int LastDateId { get; set; }
                public float[] Values { get; set; }
            }

            public class TeamDTO
            {
                public string Key { get; set; }
                public Guid Id { get; set; }
                public string Name { get; set; }
            }

            public class SegmentDTO
            {
                public string Key { get; set; }
                public Guid Id { get; set; }
                public string Name { get; set; }
            }

            public class PraiseDTO
            {
                public PraiseTypes Type { get; set; }
                public int Count { get; set; }
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                IQueryable<Tayra.Models.Organizations.Profile> profileQuery;

                if (msg.Username is not null)
                {
                    profileQuery = _db.Profiles.Where(x => x.Username == msg.Username);
                }
                else
                {
                    profileQuery = _db.Profiles.Where(x => x.Id == msg.ProfileId);
                }
                
                var profileDto = (from p in profileQuery 
                    select new Result
                    {
                        ProfileId = p.Id,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Username = p.Username,
                        Role = p.Role,
                        Avatar = p.Avatar,
                        Segments = p.Assignments.Select(x => new Result.SegmentDTO { Id = x.Segment.Id, Key = x.Segment.Key, Name = x.Segment.Name }).ToArray(),
                        Teams = p.Assignments.Where(x => x.TeamId.HasValue).Select(x => new Result.TeamDTO { Id = x.Team.Id, Key = x.Team.Key, Name = x.Team.Name }).ToArray(),
                        //Praises = p.Praises.GroupBy(x => x.Type).Select(x => new ProfileViewDTO.PraiseDTO { Type = x.Key, Count = x.Count() }).ToArray(),
                        AssistantSummary = p.AssistantSummary
                    }).FirstOrDefault();

                profileDto.EnsureNotNull();

                var praises = _db.ProfilePraises
                    .Where(x => x.ProfileId == profileDto.ProfileId)
                    .GroupBy(x => x.Type)
                    .Select(x => new Result.PraiseDTO {Type = x.Key, Count = x.Count()})
                    .ToArray();

                profileDto.Praises = praises;
                    
                
                var tokens = (from tt in _db.TokenTransactions
                              where !tt.ClaimRequired || tt.ClaimedAt.HasValue
                              where tt.ProfileId == profileDto.ProfileId
                              group tt by tt.TokenType into g
                              select new Result.TokenDTO
                              {
                                  Type = g.Key,
                                  Value = g.Sum(x => x.Value)
                              }).ToArray();

                profileDto.CompanyTokens = Math.Round(tokens.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Value).FirstOrDefault(), 2);
                profileDto.Experience = Convert.ToInt32(tokens.Where(x => x.Type == TokenType.Experience).Select(x => x.Value).FirstOrDefault());

                if (msg.ProfileId != profileDto.ProfileId)
                {
                    profileDto.LastUppedAt = (from u in _db.ProfilePraises
                                              where u.CreatedBy == msg.ProfileId
                                              where u.ProfileId == profileDto.ProfileId
                                              orderby u.DateId descending
                                              select u.Created).FirstOrDefault();
                }
                var activeItems = new ProfilesService().GetProfileActiveItems(_db, profileDto.ProfileId);
                profileDto.Badges = activeItems.Badges;
                profileDto.Title = activeItems.Title;
                profileDto.Border = activeItems.Border;

                profileDto.Pulse = await new ProfilesService().GetProfilePulseDTO(_db, profileDto.ProfileId, token);

                return profileDto;
            }
        }
    }
}