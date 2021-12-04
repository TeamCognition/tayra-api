using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tayra.Models.Organizations;


namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpGet("{teamId}/getMetrics")]
        public async Task<GetMetrics.Result> GetMetrics(Guid teamId)
        {
            return await _mediator.Send(new GetMetrics.Query { TeamId = teamId });
        }
    }

    public class GetMetrics
    {
        public class Query : IRequest<Result>
        {
            public Guid TeamId { get; init; }
        }

        public class Result
        {
            public CycleTimeMetrics CycleTimeMetrics { get; set; }
        }

        public class CycleTimeMetrics
        {
            public double AverageCycleTimeHours { get; set; }
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

                var teamRepositoryExternalIds = await _db.Repositories.Where(x => x.TeamId == team.Id)
                                                                      .Select(x => x.ExternalId)
                                                                      .ToListAsync(token);

                var pullRequests = await _db.PullRequests.Where(x => teamRepositoryExternalIds.Contains(x.ExternalRepositoryId))
                                                         .ToListAsync(token);

                var pullRequestIds = pullRequests.Select(x => x.Id)
                                                 .ToList();

                var commits = await _db.GitCommits.Where(x => x.FirstPullRequestId.HasValue && pullRequestIds.Contains(x.FirstPullRequestId.Value))
                                                  .ToListAsync(token);

                var dateTimeNow = DateTime.UtcNow;
                double totalDurationHours = 0;

                foreach (var pullRequest in pullRequests)
                {
                    var pullRequestCommits = commits.Where(x => x.FirstPullRequestId == pullRequest.Id);

                    var firstCommittedAt = pullRequestCommits.Min(x => x.CommittedAt);
                    var firstCommit = pullRequestCommits.FirstOrDefault(x => x.CommittedAt == firstCommittedAt);

                    double durationHours = 0;

                    if (pullRequest.MergedAt.HasValue)
                    {
                        var duration = pullRequest.MergedAt.Value - firstCommittedAt;
                        durationHours = duration.TotalHours;
                    }
                    else
                    {
                        var duration = dateTimeNow - firstCommittedAt;
                        durationHours = duration.TotalHours;
                    }

                    totalDurationHours += durationHours;
                }

                var averageDurationHours = totalDurationHours / pullRequests.Count;

                var result = new Result
                {
                    CycleTimeMetrics = new CycleTimeMetrics
                    {
                        AverageCycleTimeHours = averageDurationHours
                    }
                };

                return result;            
            }
        }
    }
}