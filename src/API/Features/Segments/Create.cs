using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Segments
{
    public partial class SegmentsController
    {
        [HttpPost]
        public async Task<Unit> Create([FromBody] Create.Command command)
            => await _mediator.Send(command with { ProfileRole = CurrentUser.Role , ProfileId =  CurrentUser.ProfileId});
    }

    public static class Create
    {
        public record Command : IRequest
        {
            public string Key { get; init; }
            public string Name { get; init; }
            public string Avatar { get; init; }
            public decimal? AllocatedBudget { get; init; }

            public Guid? ProfileId { get; init; }

            public ProfileRoles ProfileRole { get; init; }

        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public Handler(OrganizationDbContext db) => _db = db;

            public async Task HandleHack(Command msg, CancellationToken token)
            {
                await Handle(msg, token);
            }
            
            protected override async Task Handle(Command msg, CancellationToken token)
            {
                if (!IsSegmentKeyUnique(_db, msg.Key))
                {
                    throw new ApplicationException($"A segment exists with the same key");
                }

                var segment = _db.Add(new Segment
                {
                    Name = msg.Name.Trim(),
                    Key = msg.Key.Trim(),
                    Avatar = msg.Avatar,
                    AllocatedBudget = msg.AllocatedBudget ?? 0m
                }).Entity;

                var team = _db.Add(new Team
                {
                    Segment = segment,
                    Name = "Team 1",
                    Key = "T1"
                }).Entity;

                var integrations = CreateDefaultIntegrations(segment);
                _db.AddRange(integrations);

                if (msg.ProfileId != null && msg.ProfileRole != ProfileRoles.Admin)
                {
                    _db.Add(new ProfileAssignment
                    {
                        ProfileId = msg.ProfileId.Value,
                        SegmentId = segment.Id,
                        Team = team,
                    });
                }

                await _db.SaveChangesAsync(token);
            }
        }

        private static List<Integration> CreateDefaultIntegrations(Segment segment)
        {
            var gitHubIntegration = CreateDefaultIntegration(segment, IntegrationType.GH);
            var jiraIntegration = CreateDefaultIntegration(segment, IntegrationType.ATJ);
            var slackIntegration = CreateDefaultIntegration(segment, IntegrationType.SL);

            var integrations = new List<Integration> { gitHubIntegration, jiraIntegration, slackIntegration };
            return integrations;
        }

        private static Integration CreateDefaultIntegration(Segment segment, IntegrationType integrationType)
        {
            return new Integration
            {
                Segment = segment,
                Type = integrationType,
                Status = IntegrationStatuses.NotConnected
            };
        }

        private static bool IsSegmentKeyUnique(OrganizationDbContext dbContext, string segmentKey)
        {
            return !dbContext.Segments.Any(x => x.Key == segmentKey);
        }
    }
}