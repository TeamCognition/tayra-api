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
        [HttpGet("{teamId}/getCycleMetrics")]
        public async Task<GetCycleMetrics.Result> GetMetrics(Guid teamId, [FromQuery] GetCycleMetrics.Query query)
        {
            return await _mediator.Send(new GetCycleMetrics.Query { DaysTrailing = query.DaysTrailing, TeamId = teamId });
        }
    }

    public class GetCycleMetrics
    {

        public class Query : IRequest<Result>
        {
            public int DaysTrailing { get; set; }
            public Guid TeamId { get; init; }
        }

        public class Result
        {
            public int DaysTrailing { get; set; }
            public double? AverageCycleTimeHours { get; set; }
            public IList<TrailingIntervalCycleTimeMetrics> IntervalsMetrics { get; set; }
            public LatestIntervalCycleTimeMetrics LatestIntervalMetrics { get; set; }
        }

        public class TrailingIntervalCycleTimeMetrics
        {
            public DateTime StartedAt { get; set; }
            public DateTime EndedAt { get; set; }
            public double? AverageCycleTimeHours { get; set; }
        }

        public class LatestIntervalCycleTimeMetrics
        {
            public TrailingIntervalCycleTimeMetrics LatestIntervalMetrics { get; set; }
            public TimeToAverageValue TimeToOpen { get; set; }
            public TimeToAverageValue TimeToPickUp { get; set; }
            public TimeToAverageValue TimeToReview { get; set; }
        }

        public class TimeToAverageValue
        {
            public double AverageHours { get; set; }

            /// <summary>
            /// Increase since the previous interval
            /// </summary>
            public double Difference { get; set; }
        }

        private class TimeToAveragesDto
        {
            public double TimeToOpenHours { get; set; }
            public double TimeToReviewHours { get; set; }
            public double TimeToPickUpHours { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly OrganizationDbContext _db;
            private readonly int _numberOfIntervals = 5;

            public Handler(OrganizationDbContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query msg, CancellationToken token)
            {
                var team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == msg.TeamId, token);
                team.EnsureNotNull(msg.TeamId);

                var pullRequests = await GetMergedPullRequestsAsync(team, token);
                var commits = await GetCommitsAsync(pullRequests, token);

                var result = new Result
                {
                    DaysTrailing = msg.DaysTrailing
                };

                if (!ValidatePullRequestsAndCommits(pullRequests, commits))
                {
                    return result;
                }

                result.AverageCycleTimeHours = CalculateAverageCycleTimeHours(pullRequests, commits);

                var dateTimeUtcNow = DateTime.UtcNow;
                var intervalEndDates = GetIntervalEndDates(msg, dateTimeUtcNow);

                var filteredPullRequests = ApplyIntervalsFilterAndReturn(pullRequests, intervalEndDates, dateTimeUtcNow, msg.DaysTrailing);

                result.IntervalsMetrics = CalculateTrailingIntervalsMetrics(filteredPullRequests, commits, intervalEndDates, msg.DaysTrailing);
                result.LatestIntervalMetrics = CalculateLatestIntervalMetrics(filteredPullRequests, commits, result.IntervalsMetrics, dateTimeUtcNow);

                return result;
            }

            #region Private methods

            private List<DateTime> GetIntervalEndDates(Query msg, DateTime dateTimeUtcNow)
            {
                var intervalEndDates = new List<DateTime> { dateTimeUtcNow };
                var dateTimeCalculationTemp = dateTimeUtcNow;

                for (int i = 0; i < _numberOfIntervals - 1; i++)
                {
                    dateTimeCalculationTemp = dateTimeCalculationTemp.AddDays(-msg.DaysTrailing);
                    intervalEndDates.Add(dateTimeCalculationTemp);
                }

                return intervalEndDates;
            }

            private double? CalculateAverageCycleTimeHours(ICollection<PullRequest> pullRequests, ICollection<GitCommit> commits)
            {
                var averageDurationHours = pullRequests.Average(x => GetPullRequestDurationHours(commits, x));

                return averageDurationHours;
            }

            private List<TrailingIntervalCycleTimeMetrics> CalculateTrailingIntervalsMetrics(ICollection<PullRequest> pullRequests, ICollection<GitCommit> commits, ICollection<DateTime> intervalEndDates, int daysTrailing)
            {
                return intervalEndDates.Select(x => CalculateAverageTrailingIntervalCycleTime(pullRequests, commits, CalculateStartedAt(x, daysTrailing), x))
                                       .ToList();
            }

            private LatestIntervalCycleTimeMetrics CalculateLatestIntervalMetrics(ICollection<PullRequest> pullRequests, IList<GitCommit> commits, ICollection<TrailingIntervalCycleTimeMetrics> trailingIntervalsMetrics, DateTime dateTimeUtcNow)
            {
                var latestInterval = trailingIntervalsMetrics.FirstOrDefault(x => x.EndedAt == dateTimeUtcNow);
                var latestTimeToAverages = CalculateTimeToAverages(pullRequests, commits, latestInterval);

                var secondLatestInterval = trailingIntervalsMetrics.FirstOrDefault(x => x.EndedAt == latestInterval.StartedAt);
                var secondLatestTimeToAverages = CalculateTimeToAverages(pullRequests, commits, secondLatestInterval);

                LatestIntervalCycleTimeMetrics latestIntervalMetrics = CalculateIntervalDifferencesAndCreateModel(latestInterval, latestTimeToAverages, secondLatestTimeToAverages);

                return latestIntervalMetrics;
            }

            private LatestIntervalCycleTimeMetrics CalculateIntervalDifferencesAndCreateModel(TrailingIntervalCycleTimeMetrics latestInterval, TimeToAveragesDto latestTimeToAverages, TimeToAveragesDto secondLatestTimeToAverages)
            {
                if (latestTimeToAverages == null || secondLatestTimeToAverages == null)
                {
                    return new LatestIntervalCycleTimeMetrics();
                }

                return new LatestIntervalCycleTimeMetrics
                {
                    LatestIntervalMetrics = latestInterval,
                    TimeToOpen = new TimeToAverageValue
                    {
                        AverageHours = latestTimeToAverages.TimeToOpenHours,
                        Difference = latestTimeToAverages.TimeToOpenHours - secondLatestTimeToAverages.TimeToOpenHours
                    },
                    TimeToPickUp = new TimeToAverageValue
                    {
                        AverageHours = latestTimeToAverages.TimeToPickUpHours,
                        Difference = latestTimeToAverages.TimeToPickUpHours - secondLatestTimeToAverages.TimeToPickUpHours
                    },
                    TimeToReview = new TimeToAverageValue
                    {
                        AverageHours = latestTimeToAverages.TimeToReviewHours,
                        Difference = latestTimeToAverages.TimeToReviewHours - secondLatestTimeToAverages.TimeToReviewHours
                    }
                };
            }

            private TimeToAveragesDto CalculateTimeToAverages(ICollection<PullRequest> pullRequests, IList<GitCommit> commits, TrailingIntervalCycleTimeMetrics intervalMetrics)
            {
                var intervalPullRequests = GetTrailingIntervalPullRequests(pullRequests, intervalMetrics.StartedAt, intervalMetrics.EndedAt);
                var intervalCommits = commits.Where(x => x.FirstPullRequestId.HasValue
                                                      && intervalPullRequests.Select(y => y.Id).Contains(x.FirstPullRequestId.Value))
                                             .ToList();

                if (intervalPullRequests == null || intervalPullRequests.Count == 0)
                {
                    return null;
                }

                if (intervalCommits.Count == 0 || intervalCommits == null)
                {
                    return null;
                }

                var averageTimeToOpen = intervalPullRequests.Select(x => x.ExternalCreatedAt - GetPullRequestFirstCommitTime(intervalCommits, x))
                                                            .Where(x => x.HasValue)
                                                            .Average(x => x.Value.TotalHours);

                var pullRequestsWithReviews = intervalPullRequests.Where(x => x.FirstReviewCreatedAt.HasValue)
                                                                  .ToList();

                var averageTimeToReview = pullRequestsWithReviews.Average(x => (x.FirstReviewCreatedAt.Value - x.ExternalCreatedAt).TotalHours);

                var averageTimeToApprove = pullRequestsWithReviews.Where(x => x.ApprovedAt.HasValue)
                                                                  .Average(x => (x.ApprovedAt.Value - x.FirstReviewCreatedAt.Value).TotalHours);
                var timeToAverages = new TimeToAveragesDto
                {
                    TimeToOpenHours = averageTimeToOpen,
                    TimeToPickUpHours = averageTimeToReview,
                    TimeToReviewHours = averageTimeToApprove
                };

                return timeToAverages;
            }

            private TrailingIntervalCycleTimeMetrics CalculateAverageTrailingIntervalCycleTime(ICollection<PullRequest> pullRequests, ICollection<GitCommit> commits, DateTime intervalStartedAt, DateTime intervalEndedAt)
            {
                var trailingIntervalPullRequests = GetTrailingIntervalPullRequests(pullRequests, intervalStartedAt, intervalEndedAt);

                var averageDurationHours = trailingIntervalPullRequests.Average(x => GetPullRequestDurationHours(commits, x));

                return new TrailingIntervalCycleTimeMetrics
                {
                    StartedAt = intervalStartedAt,
                    EndedAt = intervalEndedAt,
                    AverageCycleTimeHours = averageDurationHours
                };
            }

            private List<PullRequest> GetTrailingIntervalPullRequests(ICollection<PullRequest> pullRequests, DateTime intervalStartedAt, DateTime intervalEndedAt)
            {
                var trailingIntervalPullRequests = pullRequests.Where(x => x.MergedAt.Value >= intervalStartedAt
                                                                        && x.MergedAt.Value < intervalEndedAt)
                                                               .ToList();

                return trailingIntervalPullRequests;
            }

            private DateTime? GetPullRequestFirstCommitTime(ICollection<GitCommit> commits, PullRequest pullRequest)
            {
                var pullRequestCommits = commits.Where(x => x.FirstPullRequestId == pullRequest.Id);

                var firstCommittedAt = pullRequestCommits?.Min(x => x.CommittedAt);
                return firstCommittedAt;
            }

            private double? GetPullRequestDurationHours(ICollection<GitCommit> commits, PullRequest pullRequest)
            {
                DateTime? firstCommittedAt = GetPullRequestFirstCommitTime(commits, pullRequest);

                if (!firstCommittedAt.HasValue)
                {
                    return null;
                }

                TimeSpan duration = pullRequest.MergedAt.Value - firstCommittedAt.Value;

                return duration.TotalHours;
            }

            private DateTime CalculateStartedAt(DateTime endedAt, int daysTrailing)
            {
                return endedAt.AddDays(-daysTrailing);
            }

            private async Task<List<GitCommit>> GetCommitsAsync(List<PullRequest> pullRequests, CancellationToken token)
            {
                var pullRequestIds = pullRequests.Select(x => x.Id)
                                                 .ToList();

                var commits = await _db.GitCommits.AsNoTracking()
                                                  .Where(x => x.FirstPullRequestId.HasValue && pullRequestIds.Contains(x.FirstPullRequestId.Value))
                                                  .ToListAsync(token);
                return commits;
            }

            private async Task<List<PullRequest>> GetMergedPullRequestsAsync(Team team, CancellationToken token)
            {
                var teamRepositoryExternalIds = await _db.Repositories.AsNoTracking()
                                                                      .Where(x => x.TeamId == team.Id)
                                                                      .Select(x => x.ExternalId)
                                                                      .ToListAsync(token);

                var pullRequests = await _db.PullRequests.AsNoTracking()
                                                         .Where(x => x.MergedAt.HasValue
                                                                  && teamRepositoryExternalIds.Contains(x.ExternalRepositoryId))
                                                         .ToListAsync(token);
                return pullRequests;
            }

            private List<PullRequest> ApplyIntervalsFilterAndReturn(ICollection<PullRequest> pullRequests, ICollection<DateTime> intervalEndDates, DateTime dateTimeUtcNow, int daysTrailing)
            {
                var firstIntervalEndDate = intervalEndDates.Min();
                var startedAt = CalculateStartedAt(firstIntervalEndDate, daysTrailing);
                var endedAt = dateTimeUtcNow;

                var filteredPullRequests = pullRequests.Where(x => x.MergedAt.Value >= startedAt
                                                                && x.MergedAt.Value < endedAt)
                                                       .ToList();

                return filteredPullRequests;
            }

            private bool ValidatePullRequestsAndCommits(ICollection<PullRequest> pullRequests, ICollection<GitCommit> commits)
            {
                if (pullRequests == null || pullRequests.Count == 0)
                {
                    return false;
                }

                if (commits == null || commits.Count == 0)
                {
                    return false;
                }

                return true;
            }

            #endregion
        }
    }
}