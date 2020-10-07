using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.GitHub.WebhookPayloads;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.TaskConverters;

namespace Tayra.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebhooksController : BaseController
    {
        #region Constructor

        public WebhooksController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(
            serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        private readonly string[] _ghEvent =
            {"push", "pull_request", "pull_request_review", "pull_request_review_comment"};

        private OrganizationDbContext DbContext { get; }

        #endregion

        #region Action Methods

        private void SaveWebhookEventLog(JObject jObject, IntegrationType integrationType)
        {
            DbContext.WebhookEventLogs.Add(new WebhookEventLog
                {IntegrationType = integrationType, Data = jObject.ToString(Formatting.None)});
            DbContext.SaveChanges();
        }

        [HttpPost("atjissueupdate")]
        [AllowAnonymous]
        public ActionResult JiraIssueUpdate([FromBody] JObject jObject)
        {
            SaveWebhookEventLog(jObject, IntegrationType.ATJ);
            JiraWebhookEvent we = jObject.ToObject<JiraWebhookEvent>();

            TaskConverterJira taskConverter = new TaskConverterJira(
                DbContext,
                ProfilesService,
                we);
            if (TaskHelpers.DoStandardStuff(taskConverter, TasksService, TokensService, LogsService, AssistantService))
            {
                DbContext.SaveChanges();
            }

            return Ok();
        }

        [HttpPost("gh")]
        [AllowAnonymous]
        public ActionResult GithubWebhook([FromBody] JObject jObject, [FromServices] ILogsService logsService)
        {
            Request.Headers.TryGetValue("X-GitHub-Event", out StringValues ghEvent);

            if (_ghEvent.Contains(ghEvent.ToString()))
            {
                return Ok("skipped");
            }

            PushWebhookPayload payload = jObject.ToObject<PushWebhookPayload>();

            if (!DbContext.Repositories.Any(x => x.ExternalId == payload.Repository.Id))
            {
                return Ok("skipped - repo not active");
            }

            SaveWebhookEventLog(jObject, IntegrationType.GH);

            var now = DateTime.UtcNow;

            switch (ghEvent.ToString())
            {
                case "push":
                    HandlePush(jObject, logsService);
                    break;
                case "pull_request":
                    HandlePullRequest(jObject, logsService);
                    break;
                case "pull_request_review":
                    HandlePullRequestReview(jObject, logsService);
                    break;
            }
            DbContext.SaveChanges();
            return Ok();
        }

        private void HandlePush(JObject jObject, ILogsService logsService)
        {
            PushWebhookPayload payload = jObject.ToObject<PushWebhookPayload>();

            foreach (var commit in payload.Commits)
            {
                if (!commit.Distinct)
                    continue;

                var authorProfile =
                    ProfilesService.GetProfileByExternalId(commit.Author.Username, IntegrationType.GH);
                DbContext.Add(new GitCommit
                {
                    SHA = commit.Id,
                    AuthorProfile = authorProfile,
                    AuthorExternalId = commit.Author.Username,
                    Message = commit.Message,
                    ExternalUrl = commit.Url
                });

                var logData = new LogCreateDTO
                {
                    Event = LogEvents.CodeCommitted,
                    Data = new Dictionary<string, string>
                    {
                        {"timestamp", DateTime.UtcNow.ToString()},
                        {"committedAt", commit.Timestamp.ToString()},
                        {"externalUrl", commit.Url},
                        {"externalAuthorUsername", commit.Author.Username},
                        {"sha", commit.Id},
                        {"message", commit.Message},
                    }
                };

                if (authorProfile != null)
                {
                    logData.Data.Add("profileUsername", authorProfile.Username);
                    logData.ProfileId = authorProfile.Id;
                }

                logsService.LogEvent(logData);
            }
        }

        private void HandlePullRequest(JObject jObject, ILogsService logsService)
        {
            PullRequestWebhookPayload prPayload = jObject.ToObject<PullRequestWebhookPayload>();
            var authorProfile =
                ProfilesService.GetProfileByExternalId(prPayload.PullRequest.Author.Username, IntegrationType.GH);
            PullRequestDTO pullRequest = prPayload.PullRequest;
            DbContext.Add(new PullRequestGit
            {
                AuthorProfile = authorProfile,
                CreatedAt = pullRequest.CreatedAt,
                MergedAt = pullRequest.MergedAt,
                Commits = pullRequest.Commits,
                ReviewComments = pullRequest.ReviewComments,
                UpdatedAt = pullRequest.UpdatedAt,
                Title = pullRequest.Title,
                Body = pullRequest.Body,
                ExternalUrl = pullRequest.Url
            });
            var logData = new LogCreateDTO
            {
                Event = LogEvents.CodePullRequest,
                Data = new Dictionary<string, string>
                {
                    {"timestamp", DateTime.UtcNow.ToString()},
                    {"created_at", pullRequest.CreatedAt.ToString()},
                    {"externalUrl", pullRequest.Url},
                    {"externalAuthorUsername", pullRequest.Author.Username},
                    {"sha", pullRequest.Id},
                    {"title", pullRequest.Title},
                }
            };
            if (authorProfile != null)
            {
                logData.Data.Add("profileUsername", authorProfile.Username);
                logData.ProfileId = authorProfile.Id;
            }

            logsService.LogEvent(logData);
        }

        private void HandlePullRequestReview(JObject jObject, ILogsService logsService)
        {
            PullRequsetReviewWebhookPayload prReviewPayload = jObject.ToObject<PullRequsetReviewWebhookPayload>();
            var reviewerProfile =
                ProfilesService.GetProfileByExternalId(prReviewPayload.PullRequestReview.ReviewUser.Username,
                    IntegrationType.GH);
            PullRequestReviewDTO pullRequestReview = prReviewPayload.PullRequestReview;
            DbContext.Add(new PullRequestReviewGit()
            {
                ReviewerProfile = reviewerProfile,
                CommitId = pullRequestReview.CommitId,
                State = pullRequestReview.State,
                SubmittedAt = pullRequestReview.SubmittedAt,
                Title = pullRequestReview.Title,
                Body = pullRequestReview.Body,
                ReviewerExternalId = pullRequestReview.ReviewUser.Id
            });
            var logData = new LogCreateDTO
            {
                Event = LogEvents.CodePullRequest,
                Data = new Dictionary<string, string>
                {
                    {"timestamp", DateTime.UtcNow.ToString()},
                    {"submitted_at", pullRequestReview.SubmittedAt.ToString()},
                    {"externalReviewerUsername", pullRequestReview.ReviewUser.Username},
                    {"sha", pullRequestReview.Id},
                    {"title", pullRequestReview.Title},
                }
            };
            if (reviewerProfile != null)
            {
                logData.Data.Add("profileUsername", reviewerProfile.Username);
                logData.ProfileId = reviewerProfile.Id;
            }

            logsService.LogEvent(logData);
        }

        #endregion
    }
}