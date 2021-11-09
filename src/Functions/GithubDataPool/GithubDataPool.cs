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
                            var commits = githubConnector.GetCommitsByPeriodFromAllBranches(integration.Id, command.Since, (repo.Name, repo.Owner.Login));
                            AddOrUpdateCommits(organizationDb, commits.ToArray());

                            organizationDb.SaveChanges();
                            var prs = githubConnector.GetPullRequestsByPeriod(integration.Id, repo.Name,
                                repo.Owner.Login);
                            AddOrUpdatePullRequests(organizationDb, prs.Repository.PullRequestsNodes.PullRequests);
                        }   
                    }
                }    
            }
        }
        
        private void AddOrUpdateCommits(OrganizationDbContext organizationDb, CommitType[] commits)
        {
            var commitsShaAlreadyInDatabase = organizationDb.GitCommits.Where(x => commits.Select(c => c.Sha).Contains(x.Sha)).Select(x => x.Sha).ToArray();

            var firstPullRequests = GetFirstPullRequests(commits);

            var pullRequestsInTheDatabase = organizationDb.PullRequests.Where(x => firstPullRequests.Select(y => y.Value.Id).Contains(x.ExternalId))
                                                                             .ToList();

            foreach (var commit in commits.Where(x => !commitsShaAlreadyInDatabase.Contains(x.Sha)))
            {
                var authorProfile = commit.Author.User != null
                    ? new ProfilesService().GetProfileByExternalId(organizationDb, commit.Author.User.Username, IntegrationType.GH)
                    : null;

                var firstAssociatedPullRequest = firstPullRequests[commit.Sha];
                var firstPullRequest = firstAssociatedPullRequest != null
                    ? pullRequestsInTheDatabase.FirstOrDefault(x => x.ExternalId == firstAssociatedPullRequest.Id)
                    : null;

                organizationDb.Add(new GitCommit
                {
                    Sha = commit.Sha,
                    AuthorProfile = authorProfile,
                    AuthorExternalId = commit.Author.User?.Username ?? commit.Author.Email,
                    Message = commit.Message,
                    ExternalUrl = commit.Url,
                    Additions = commit.Additions,
                    Deletions = commit.Deletions,
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

        private void AddOrUpdatePullRequests(OrganizationDbContext organizationDb, Tayra.Connectors.GitHub.GetPullRequestsResponse.PullRequest[] pullRequests)
        {
            var prAlreadyInDatabase = organizationDb.PullRequests.Where(x => pullRequests.Select(p => p.ExternalId).Contains(x.ExternalId)).ToArray();
            var prExternalIdsAlreadyInDatabase = prAlreadyInDatabase.Select(x => x.ExternalId).ToArray();
            var logService = new LogsService(organizationDb, new MailerService());
            
            foreach (var pr in pullRequests.Where(x => !prExternalIdsAlreadyInDatabase.Contains(x.ExternalId)))
            {
                var authorProfile = new ProfilesService().GetProfileByExternalId(organizationDb, pr.Author.Username, IntegrationType.GH);
                
                if (!prExternalIdsAlreadyInDatabase.Contains(pr.ExternalId)) //new
                {
                    organizationDb.Add(new PullRequest
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
                    });                    
                    
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

                organizationDb.SaveChanges();
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