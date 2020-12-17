using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Models.Organizations;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpGet, Route("integrations")]
        public async Task<GetConnectedApps.Result[]> GetProfileIntegrations()
        {
            return await _mediator.Send(new GetConnectedApps.Query { ProfileId = CurrentUser.ProfileId});
        }
    }
    
    public class GetConnectedApps
    {
        public record Query : IRequest<Result[]>
        {
            public Guid ProfileId { get; init; }
            public Guid[] SegmentIds { get; init; }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public Guid SegmentId { get; set; }
            public IntegrationType Type { get; set; }
            public string ExternalId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result[]>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task<Result[]> Handle(Query msg, CancellationToken token)
            {
                var integrations = await _db.Integrations
                    .Where(x => (x.ProfileId == msg.ProfileId || x.ProfileId == null) && msg.SegmentIds.Contains(x.SegmentId))
                    .Select(x => new Result
                    {
                        Id = x.Id,
                        SegmentId = x.SegmentId,
                        Type = x.Type,
                        ExternalId = x.ProfileId != null ? x.Fields.Where(e => e.Key == Constants.PROFILE_EXTERNAL_ID).Select(e => e.Value).FirstOrDefault() : null
                    })
                    .ToListAsync(token);

                foreach (var i in integrations.Where(x => x.ExternalId == null).ToArray())
                {
                    if (integrations.Any(x => x.SegmentId == i.SegmentId && x.Type == i.Type && x.ExternalId != null))
                        integrations.Remove(i);
                }
                return integrations.ToArray(); 
            }
        }
    }
}