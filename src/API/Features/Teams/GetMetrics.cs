using Cog.DAL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tayra.Models.Organizations;


namespace Tayra.API.Features.Teams
{
    public partial class TeamsController
    {
        [HttpGet("{teamId}/getMetrics")]
        public async Task<GetMetrics.Result> GetMetrics(Guid teamId, [FromQuery] GetMetrics.Query query)
        {
            return await _mediator.Send(new GetMetrics.Query { DaysTrailing = query.DaysTrailing, TeamId = teamId });
        }
    }

    public class GetMetrics
    {
        public class Query : IRequest<Result>
        {
            public int DaysTrailing { get; set; }
            public Guid TeamId { get; init; }
        }

        public class Result
        {
            public int DaysTrailing { get; set; }
            public CycleTimeMetrics CycleTimeMetrics { get; set; }
        }

        public class CycleTimeMetrics
        {
            public double AverageCycleTimeHours { get; set; }
            public double DaysTrailingCycleTimeHours { get; set; }
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

                var pullRequests = await GetPullRequestsAsync(team, token);
                var commits = await GetCommitsAsync(pullRequests, token);

                var dateTimeUtcNow = DateTime.UtcNow;

                double averageDurationHours = CalculateAverageCycleTimeInHours(pullRequests, commits, dateTimeUtcNow);
                double daysTrailingCycleTimeHours = CalculateAverageTrailingPeriodCycleTimeInHours(pullRequests, commits, dateTimeUtcNow, msg.DaysTrailing);

                var result = new Result
                {
                    CycleTimeMetrics = new CycleTimeMetrics
                    {
                        AverageCycleTimeHours = averageDurationHours,
                        DaysTrailingCycleTimeHours = daysTrailingCycleTimeHours
                    }
                };

                return result;
            }

            #region Private methods

            private double CalculateAverageCycleTimeInHours(ICollection<PullRequest> pullRequests, ICollection<GitCommit> commits, DateTime dateTimeUtcNow)
            {
                var averageDurationHours = pullRequests.Average(x => GetPullRequestDuration(commits, x, dateTimeUtcNow).TotalHours);

                return averageDurationHours;
            }

            private double CalculateAverageTrailingPeriodCycleTimeInHours(ICollection<PullRequest> pullRequests, ICollection<GitCommit> commits, DateTime dateTimeUtcNow, int daysTrailing)
            {
                var trailingPeriodPullRequests = GetTrailingPeriodPullRequests(pullRequests, dateTimeUtcNow, daysTrailing);

                var averageDurationHours = trailingPeriodPullRequests.Average(x => GetPullRequestDuration(commits, x, dateTimeUtcNow).TotalHours);

                return averageDurationHours;
            }

            private List<PullRequest> GetTrailingPeriodPullRequests(ICollection<PullRequest> pullRequests, DateTime dateTimeUtcNow, int daysTrailing)
            {
                var dateTimeTrailing = dateTimeUtcNow.AddDays(-daysTrailing);

                var trailingPeriodPullRequests = pullRequests.Where(x => !x.MergedAt.HasValue || x.MergedAt >= dateTimeTrailing)
                                                             .ToList();

                return trailingPeriodPullRequests;
            }

            private DateTime GetPullRequestFirstCommitTime(ICollection<GitCommit> commits, PullRequest pullRequest)
            {
                var pullRequestCommits = commits.Where(x => x.FirstPullRequestId == pullRequest.Id);

                var firstCommittedAt = pullRequestCommits.Min(x => x.CommittedAt);
                return firstCommittedAt;
            }

            private TimeSpan GetPullRequestDuration(ICollection<GitCommit> commits, PullRequest pullRequest, DateTime dateTimeUtcNow)
            {
                DateTime firstCommittedAt = GetPullRequestFirstCommitTime(commits, pullRequest);

                TimeSpan duration;

                if (pullRequest.MergedAt.HasValue)
                {
                    duration = pullRequest.MergedAt.Value - firstCommittedAt;
                }
                else
                {
                    duration = dateTimeUtcNow - firstCommittedAt;
                }

                return duration;
            }

            private async Task<List<GitCommit>> GetCommitsAsync(List<PullRequest> pullRequests, CancellationToken token)
            {
                var pullRequestIds = pullRequests.Select(x => x.Id)
                                                 .ToList();

                var commits = await _db.GitCommits.Where(x => x.FirstPullRequestId.HasValue && pullRequestIds.Contains(x.FirstPullRequestId.Value))
                                                  .ToListAsync(token);
                return commits;
            }

            private async Task<List<PullRequest>> GetPullRequestsAsync(Team team, CancellationToken token)
            {
                var teamRepositoryExternalIds = await _db.Repositories.Where(x => x.TeamId == team.Id)
                                                                      .Select(x => x.ExternalId)
                                                                      .ToListAsync(token);

                var pullRequests = await _db.PullRequests.Where(x => teamRepositoryExternalIds.Contains(x.ExternalRepositoryId))
                                                         .ToListAsync(token);
                return pullRequests;
            }

            #endregion
        }
    }
}