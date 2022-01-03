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
            public double? AverageCycleTimeHours { get; set; }
            public IList<TrailingIntervalCycleTimeMetrics> TrailingIntervalsMetrics { get; set; }
            public LatestIntervalCycleTimeMetrics LatestTrailingIntervalMetrics { get; set; }
        }

        public class TrailingIntervalCycleTimeMetrics
        {
            public DateTime StartedAt { get; set; }
            public DateTime EndedAt { get; set; }
            public double AverageCycleTimeHours { get; set; }
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
            public double AverageHoursValue { get; set; }
            public double DifferenceValue { get; set; }
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

                var result = new Result
                {
                    DaysTrailing = msg.DaysTrailing                  
                };

                var pullRequests = await GetMergedPullRequestsAsync(team, token);
                var commits = await GetCommitsAsync(pullRequests, token);

                if (pullRequests == null || pullRequests.Count == 0)
                {
                    return result;
                }

                if (commits == null || commits.Count == 0)
                {
                    return result;
                }

                // var dateTimeUtcNow = DateTime.UtcNow;
                var dateTimeUtcNow = new DateTime(2020, 9, 21, 14, 29, 29);

                var intervalEndDates = new List<DateTime> { dateTimeUtcNow };
                var dateTimeCalculationTemp = dateTimeUtcNow;

                for (int i = 0; i < _numberOfIntervals - 1; i++)
                {
                    dateTimeCalculationTemp = dateTimeCalculationTemp.AddDays(-msg.DaysTrailing);
                    intervalEndDates.Add(dateTimeCalculationTemp);
                }

                double averageDurationHours = CalculateAverageCycleTimeHours(pullRequests, commits);

                var trailingIntervalsMetrics = new List<TrailingIntervalCycleTimeMetrics>();

                foreach (var endDate in intervalEndDates)
                {
                    var startDate = endDate.AddDays(-msg.DaysTrailing);
                    var intervalCycleTimeMetrics = CalculateAverageTrailingIntervalCycleTime(pullRequests, commits, startDate, endDate);
                    trailingIntervalsMetrics.Add(intervalCycleTimeMetrics);
                }

                var latestIntervalMetrics = GetLatestIntervalMetrics(pullRequests, commits, trailingIntervalsMetrics, dateTimeUtcNow);

                result.AverageCycleTimeHours = averageDurationHours;
                result.TrailingIntervalsMetrics = trailingIntervalsMetrics;
                result.LatestTrailingIntervalMetrics = latestIntervalMetrics;
                
                return result;
            }

            #region Private methods

            private LatestIntervalCycleTimeMetrics GetLatestIntervalMetrics(ICollection<PullRequest> pullRequests, IList<GitCommit> commits, ICollection<TrailingIntervalCycleTimeMetrics> trailingIntervalsMetrics, DateTime dateTimeUtcNow)
            {
                var latestInterval = trailingIntervalsMetrics.FirstOrDefault(x => x.EndedAt == dateTimeUtcNow);

                var latestIntervalTimeToAverages = CalculateTimeToAverages(pullRequests, commits, latestInterval);

                var secondLatestInterval = trailingIntervalsMetrics.FirstOrDefault(x => x.EndedAt == latestInterval.StartedAt);

                var secondLatestIntervalTimeToAverages = CalculateTimeToAverages(pullRequests, commits, secondLatestInterval);

                var latestIntervalMetrics = new LatestIntervalCycleTimeMetrics
                {
                    LatestIntervalMetrics = latestInterval,
                    TimeToOpen = new TimeToAverageValue 
                    {
                        AverageHoursValue = latestIntervalTimeToAverages.TimeToOpenHours,
                        DifferenceValue = latestIntervalTimeToAverages.TimeToOpenHours - secondLatestIntervalTimeToAverages.TimeToOpenHours 
                    },
                    TimeToPickUp = new TimeToAverageValue
                    {
                        AverageHoursValue = latestIntervalTimeToAverages.TimeToPickUpHours,
                        DifferenceValue = latestIntervalTimeToAverages.TimeToPickUpHours - secondLatestIntervalTimeToAverages.TimeToPickUpHours
                    },
                    TimeToReview = new TimeToAverageValue
                    {
                        AverageHoursValue = latestIntervalTimeToAverages.TimeToReviewHours,
                        DifferenceValue = latestIntervalTimeToAverages.TimeToReviewHours - secondLatestIntervalTimeToAverages.TimeToReviewHours
                    }
                };

                return latestIntervalMetrics;
            }

            private TimeToAveragesDto CalculateTimeToAverages(ICollection<PullRequest> pullRequests, IList<GitCommit> commits, TrailingIntervalCycleTimeMetrics intervalMetrics)
            {
                var intervalPullRequests = GetTrailingIntervalPullRequests(pullRequests, intervalMetrics.StartedAt, intervalMetrics.EndedAt);
                var intervalCommits = commits.Where(x => x.FirstPullRequestId.HasValue
                                                      && intervalPullRequests.Select(y => y.Id).Contains(x.FirstPullRequestId.Value))
                                             .ToList();

                var averageTimeToOpen = intervalPullRequests.Select(x => x.ExternalCreatedAt - intervalCommits.Where(y => y.FirstPullRequestId.Value == x.Id)
                                                                                                              .Min(y => y.CommittedAt))
                                                            .Average(x => x.TotalHours);

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

            private double CalculateAverageCycleTimeHours(ICollection<PullRequest> pullRequests, ICollection<GitCommit> commits)
            {
                var averageDurationHours = pullRequests.Average(x => GetPullRequestDurationHours(commits, x));

                return averageDurationHours;
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
                var trailingIntervalPullRequests = pullRequests.Where(x => x.MergedAt.HasValue 
                                                                        && x.MergedAt >= intervalStartedAt
                                                                        && x.MergedAt < intervalEndedAt)
                                                               .ToList();

                return trailingIntervalPullRequests;
            }

            private DateTime GetPullRequestFirstCommitTime(ICollection<GitCommit> commits, PullRequest pullRequest)
            {
                var pullRequestCommits = commits.Where(x => x.FirstPullRequestId == pullRequest.Id);

                var firstCommittedAt = pullRequestCommits.Min(x => x.CommittedAt);
                return firstCommittedAt;
            }

            private double GetPullRequestDurationHours(ICollection<GitCommit> commits, PullRequest pullRequest)
            {
                DateTime firstCommittedAt = GetPullRequestFirstCommitTime(commits, pullRequest);

                TimeSpan duration = pullRequest.MergedAt.Value - firstCommittedAt;

                return duration.TotalHours;
            }

            private async Task<List<GitCommit>> GetCommitsAsync(List<PullRequest> pullRequests, CancellationToken token)
            {
                var pullRequestIds = pullRequests.Select(x => x.Id)
                                                 .ToList();

                var commits = await _db.GitCommits.Where(x => x.FirstPullRequestId.HasValue && pullRequestIds.Contains(x.FirstPullRequestId.Value))
                                                  .ToListAsync(token);
                return commits;
            }

            private async Task<List<PullRequest>> GetMergedPullRequestsAsync(Team team, CancellationToken token)
            {
                var teamRepositoryExternalIds = await _db.Repositories.Where(x => x.TeamId == team.Id)
                                                                      .Select(x => x.ExternalId)
                                                                      .ToListAsync(token);

                var pullRequests = await _db.PullRequests.Where(x => x.MergedAt.HasValue 
                                                                  && teamRepositoryExternalIds.Contains(x.ExternalRepositoryId))
                                                         .ToListAsync(token);
                return pullRequests;
            }

            #endregion
        }
    }
}