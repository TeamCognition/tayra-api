using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.API.Helpers;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Connectors.Slack;
using Tayra.Models.Organizations;
using Result = System.Collections.Generic.List<Tayra.API.Features.Apps.GetUsers.ResultDto>;

namespace Tayra.API.Features.Apps
{
    public partial class AppsController
    {
        [HttpGet("{appType}/{segmentId}")]
        public async Task<Result> GetUsers([FromQuery] IntegrationType appType, [FromQuery] Guid segmentId)
            => await _mediator.Send(new GetUsers.Query {AppType = appType, SegmentId = segmentId});
    }

    public class GetUsers
    {
        public record Query : IRequest<Result>
        {
            public IntegrationType AppType { get; init; }
            public Guid SegmentId { get; init; }

        }

        public record ResultDto
        {
            public string EmailAddress { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;
            private readonly IConnectorResolver _connectorResolver;

            public Handler(OrganizationDbContext db, IConnectorResolver connectorResolver)
            {
                _db = db;
                _connectorResolver = connectorResolver;
            }

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var integrationId = _db.Integrations.Where(x => x.Type == IntegrationType.SL && x.SegmentId == msg.SegmentId && x.ProfileId == null).Select(x => x.Id).FirstOrDefault();
                if (integrationId == default)
                {
                    throw new ApplicationException("no slack integration found for this segment");
                }
                var slackConnector = (SlackConnector) _connectorResolver.Get<IOAuthConnector>(IntegrationType.SL);
                var slackResponse = await slackConnector.GetUsersList(Guid.Empty);

                if (!slackResponse.Ok)
                    return new Result();
                
                return slackResponse.Members.Where(x => x.Deleted == false && x.IsBot == false).Select(x => new ResultDto
                {
                    EmailAddress = x.Profile.Email,
                    FirstName = x.Profile.FirstName,
                    LastName = x.Profile.LastName,
                }).ToList();
            }
        }
    }
}