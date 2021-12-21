using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoreLinq;
using Tayra.Common;
using Tayra.Connectors.GitHub;
using Tayra.Mailer;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.Models.Profiles;
using static Tayra.Connectors.GitHub.CommitType;

namespace Tayra.Functions.GithubDataPool
{
    public class GithubDataPool
    {
        private readonly CatalogDbContext _catalogDb;
        private readonly IConfiguration _config;
        
        public GithubDataPool(CatalogDbContext catalogDb, IConfiguration config)
        {
            _catalogDb = catalogDb;
            _config = config;
        }
        
        [Function(nameof(GithubDataPool) + "Http")]
        public async Task<bool> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(GenerateMetrics));
            var command = new Command();
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                command = await JsonSerializer.DeserializeAsync<Command>(req.Body, options);
            }
            catch (Exception _)
            {
                return false;
            }

            var tenants = await _catalogDb.TenantIntegrations.Include(x => x.Tenant)
                .Where(x => x.Type == IntegrationType.GH)
                .Select(x => x.Tenant)
                .ToArrayAsync();

            tenants = tenants.DistinctBy(x => x.Id).ToArray();
            
            Handle(command, tenants, new LogService(logger));
            return true;
        }

        public record Command
        {
            public DateTime Since { get; init; }
        }
        
        // [Function(nameof(GenerateMetrics) + "Timer")]
        // public void Run([TimerTrigger("0 */5 * * * *")] MyTimerInfo timerInfo, FunctionContext context)
        // {
        //     
        // }

        public void Handle(Command command, Tenant[] tenants, LogService logService)
        {
            foreach (var tenant in tenants)
            {
                using (var organizationDb = new OrganizationDbContext(tenant, null))
                {
                    var integrations = organizationDb.Integrations
                        .Where(x => x.Type == IntegrationType.GH && x.ProfileId == null)
                        .ToArray();
                    
                    foreach (var integration in integrations)
                    {
                        var githubConnector = new GitHubConnector(null, organizationDb, _catalogDb, _config);
                        var installationId = _catalogDb.TenantIntegrations
                            .First(x => x.Type == IntegrationType.GH && x.SegmentId == integration.SegmentId)
                            .InstallationId;
                        
                        var repositories = githubConnector.GetRepositories(installationId);
                        githubConnector.AddOrUpdateRepositoriesByIntegration(installationId, repositories);

                        foreach (var repo in repositories)
                        {
                            var prs = githubConnector.GetPullRequestsByPeriod(integration.Id, repo.Name, repo.Owner.Login);
                            AddOrUpdatePullRequestsWithReviewsAndComments(organizationDb, prs);

                            organizationDb.SaveChanges();

                            var commits = githubConnector.GetCommitsByPeriodFromAllBranches(integration.Id, command.Since, (repo.Name, repo.Owner.Login));
                            AddOrUpdateCommits(organizationDb, commits.ToArray());

                            organizationDb.SaveChanges();
                        }
                    }
                }    
            }
        }

        private void AddOrUpdateCommits(OrganizationDbContext organizationDb, CommitType[] commits)
        {
            var firstPullRequests = GetFirstPullRequests(commits);

            var pullRequestsInTheDatabase = organizationDb.PullRequests.Where(x => firstPullRequests.Select(y => y.Value.Id).Contains(x.ExternalId))
                                                                       .ToList();

            var commitsAlreadyInDatabase = organizationDb.GitCommits.Where(x => commits.Select(c => c.Sha).Contains(x.Sha))
                                                                    .ToList();

            var commitsShaAlreadyInDatabase = commitsAlreadyInDatabase.Select(x => x.Sha)
                                                                      .ToList();

            AddNewCommits(organizationDb, commits, firstPullRequests, pullRequestsInTheDatabase, commitsShaAlreadyInDatabase);
            UpdateCommitsAlreadyInDatabase(commits, firstPullRequests, pullRequestsInTheDatabase, commitsAlreadyInDatabase);
        }

        private void UpdateCommitsAlreadyInDatabase(CommitType[] commits, Dictionary<string, AssociatedPullRequest> firstPullRequests, List<PullRequest> pullRequestsInTheDatabase, List<GitCommit> commitsAlreadyInDatabase)
        {
            foreach (var commit in commitsAlreadyInDatabase)
            {
                var commitDto = commits.FirstOrDefault(x => x.Sha == commit.Sha); 

                if (!commit.FirstPullRequestId.HasValue)
                {
                    PullRequest firstPullRequest = GetFirstPullRequestForCommit(commit.Sha, firstPullRequests, pullRequestsInTheDatabase);

                    commit.FirstPullRequest = firstPullRequest;
                }

                commit.CommittedAt = commitDto.CommittedAt;
            }
        }

        private void AddNewCommits(OrganizationDbContext organizationDb, CommitType[] commits, Dictionary<string, AssociatedPullRequest> firstPullRequests, List<PullRequest> pullRequestsInTheDatabase, List<string> commitsShaAlreadyInDatabase)
        {
            var newCommits = commits.Where(x => !commitsShaAlreadyInDatabase.Contains(x.Sha))
                                    .ToList();

            foreach (var commit in newCommits)
            {
                var authorProfile = commit.Author.User != null
                    ? new ProfilesService().GetProfileByExternalId(organizationDb, commit.Author.User.Username, IntegrationType.GH)
                    : null;

                PullRequest firstPullRequest = GetFirstPullRequestForCommit(commit.Sha, firstPullRequests, pullRequestsInTheDatabase);

                organizationDb.Add(new GitCommit
                {
                    Sha = commit.Sha,
                    AuthorProfile = authorProfile,
                    AuthorExternalId = commit.Author.User?.Username ?? commit.Author.Email,
                    Message = commit.Message,
                    ExternalUrl = commit.Url,
                    Additions = commit.Additions,
                    Deletions = commit.Deletions,
                    CommittedAt = commit.CommittedAt,
                    ExternalRepositoryId = commit.Repository.ExternalId,
                    FirstPullRequest = firstPullRequest
                });

                var logService = new LogsService(organizationDb, new MailerService());
                CreateLog(logService, LogEvents.CodeCommitted, authorProfile, commit.Message, commit.Url,
                    new Dictionary<string, string>
                    {
                        {"committedAt", commit.CommittedAt.ToString()},
                        {"externalAuthorUsername", commit.Author.User?.Username},
                        {"sha", commit.Sha},
                    });
            }
        }

        private Dictionary<string, AssociatedPullRequest> GetFirstPullRequests(CommitType[] commits)
        {
            var firstPullRequests = new Dictionary<string, AssociatedPullRequest>();

            foreach (var commit in commits)
            {
                if (commit.AssociatedPullRequests.AssociatedPullRequests.Length > 0)
                {
                    DateTime? minPullRequestMergedDateTime = commit.AssociatedPullRequests.AssociatedPullRequests.Min(x => (DateTime?)x.MergedAt ?? null);

                    if (minPullRequestMergedDateTime.HasValue)
                    {
                        var firstPullRequest = commit.AssociatedPullRequests.AssociatedPullRequests.FirstOrDefault(x => x.MergedAt == minPullRequestMergedDateTime);
                        firstPullRequests.Add(commit.Sha, firstPullRequest);
                    }
                }
            }

            return firstPullRequests;
        }

        private PullRequest GetFirstPullRequestForCommit(string commitSha, Dictionary<string, AssociatedPullRequest> firstPullRequests, List<PullRequest> pullRequestsInTheDatabase)
        {
            var firstAssociatedPullRequest = firstPullRequests.GetValueOrDefault(commitSha);

            if (firstAssociatedPullRequest == null)
            {
                return null;
            }

            var firstPullRequest = pullRequestsInTheDatabase.FirstOrDefault(x => x.ExternalId == firstAssociatedPullRequest.Id);

            return firstPullRequest;
        }

        private void AddOrUpdatePullRequestsWithReviewsAndComments(OrganizationDbContext organizationDb, List<Tayra.Connectors.GitHub.GetPullRequestsPageResponse.PullRequest> pullRequests)
        {
            var prAlreadyInDatabase = organizationDb.PullRequests.Where(x => pullRequests.Select(p => p.ExternalId).Contains(x.ExternalId)).ToArray();
            var prExternalIdsAlreadyInDatabase = prAlreadyInDatabase.Select(x => x.ExternalId).ToArray();
            var logService = new LogsService(organizationDb, new MailerService());

            foreach (var pr in pullRequests.Where(x => !prExternalIdsAlreadyInDatabase.Contains(x.ExternalId)))
            {
                var authorProfile = new ProfilesService().GetProfileByExternalId(organizationDb, pr.Author.Username, IntegrationType.GH);

                if (!prExternalIdsAlreadyInDatabase.Contains(pr.ExternalId)) //new
                {
                    var pullRequest = new PullRequest
                    {
                        AuthorProfile = authorProfile,
                        ExternalRepositoryId = pr.Repository.ExternalId,
                        ExternalCreatedAt = DateTime.Parse(pr.CreatedAt),
                        MergedAt = string.IsNullOrEmpty(pr.MergedAt) ? null : DateTime.Parse(pr.MergedAt),
                        IsLocked = pr.IsLocked,
                        ExternalUpdatedAt = DateTime.Parse(pr.UpdatedAt),
                        Title = pr.Title,
                        Body = pr.BodyText,
                        ExternalUrl = pr.Url,
                        ExternalAuthorUsername = pr.Author.Username,
                        ClosedAt = string.IsNullOrEmpty(pr.ClosedAt) ? null : DateTime.Parse(pr.ClosedAt),
                        ExternalId = pr.ExternalId,
                        ExternalNumber = pr.Number,
                        State = pr.State,
                        Additions = pr.Additions,
                        Deletions = pr.Deletions,
                        CommitsCount = pr.CommitNodes.TotalCount,
                        ReviewCommentsCount = 0,
                        ReviewsCount = pr.ReviewNodes.TotalCount
                    };

                    pullRequest.Reviews = pr.ReviewNodes.Reviews.Select(x => new PullRequestReview
                    {
                        Body = x.Body,
                        State = x.State,
                        SubmittedAt = x.SubmittedAt,
                        CommitId = x.Commit?.Sha,
                        Comments = x.CommentNodes.Comments.Select(y => new PullRequestReviewComment
                        {
                            Body = y.Body,
                            ExternalCreatedAt = y.CreatedAt,
                            ExternalUpdatedAt = y.UpdatedAt,
                            ExternalUrl = y.Url,
                            ExternalId = y.ExternalId,
                            PullRequest = pullRequest
                        }).ToList()
                    }).ToList();

                    organizationDb.Add(pullRequest);

                    CreateLog(logService, LogEvents.PullRequestCreated, authorProfile, pr.Title, pr.Url,
                        new Dictionary<string, string>
                        {
                            {"externalAuthorUsername", pr.Author.Username},
                            {"externalId", pr.ExternalId}
                        });
                }
                else //already exists in db
                {
                    var prInDb = prAlreadyInDatabase.First(x => x.ExternalId == pr.ExternalId);

                    prInDb.Body = pr.BodyText;
                    prInDb.Title = pr.Title;
                    prInDb.ExternalUpdatedAt = DateTime.Parse(pr.UpdatedAt);
                    prInDb.IsLocked = pr.IsLocked;
                    prInDb.MergedAt = string.IsNullOrEmpty(pr.MergedAt) ? null : DateTime.Parse(pr.MergedAt);
                    prInDb.ClosedAt = string.IsNullOrEmpty(pr.ClosedAt) ? null : DateTime.Parse(pr.ClosedAt);
                    prInDb.State = pr.State;

                    CreateLog(logService, LogEvents.PullRequestUpdated, authorProfile, pr.Title, pr.Url,
                        new Dictionary<string, string>
                        {
                            {"externalAuthorUsername", pr.Author.Username},
                            {"externalId", pr.ExternalId},
                            {"prState", pr.State},
                        });
                }
            }

            UpdateReviewsAndComments(organizationDb, pullRequests);

            organizationDb.SaveChanges();
        }

        private void UpdateReviewsAndComments(OrganizationDbContext organizationDb, List<GetPullRequestsPageResponse.PullRequest> pullRequests)
        {
            var allGitHubReviews = pullRequests.SelectMany(x => x.ReviewNodes.Reviews)
                                               .ToList();

            var allReviewsExternalIds = allGitHubReviews.Select(x => x.ExternalId)
                                                        .ToList();

            var allGitHubComments = allGitHubReviews.SelectMany(x => x.CommentNodes.Comments)
                                                    .ToList();

            var reviewsAlreadyInDatabase = organizationDb.PullRequestReviews.Include(x => x.Comments)
                                                                            .Where(x => allReviewsExternalIds.Contains(x.ReviewExternalId))
                                                                            .ToList();

            foreach (var review in reviewsAlreadyInDatabase)
            {
                var gitHubReview = allGitHubReviews.FirstOrDefault(x => x.ExternalId == review.ReviewExternalId);

                review.Body = gitHubReview.Body;
                review.State = gitHubReview.State;

                foreach (var reviewComment in review.Comments)
                {
                    var gitHubComment = allGitHubComments.FirstOrDefault(x => x.ExternalId == reviewComment.ExternalId);

                    reviewComment.Body = gitHubComment.Body;
                    reviewComment.ExternalUpdatedAt = gitHubComment.UpdatedAt;
                }
            }
        }

        private void CreateLog(ILogsService logsService, LogEvents eventType, Profile profile, string description, string externalUrl, Dictionary<string, string> data, DateTime? timestamp = null)
        {
            logsService.LogEvent(new LogCreateDTO
            (
                eventType: eventType,
                timestamp: timestamp ?? DateTime.UtcNow,
                description: description,
                externalUrl: externalUrl,
                data: data,
                profileId: profile?.Id
           ));
        }
    }
}