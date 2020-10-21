using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.GitHub.WebhookPayloads;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.TaskConverters;
using BadHttpRequestException = Microsoft.AspNetCore.Server.IIS.BadHttpRequestException;

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

            if (!new [] {"push", "pull_request", "pull_request_review", "pull_request_review_comment"}
                .Contains(ghEvent.ToString()))
            {
                return Ok("skipped");
            }

            BaseWebhookPayload payload = jObject.ToObject<BaseWebhookPayload>();

            // if (!DbContext.Repositories.Any(x => x.ExternalId == payload.Repository.Id))
            // {
            //     return Ok("skipped - repo not active");
            // }

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
                case "pull_request_review_comment":
                    HandlePullRequestReviewComment(jObject, logsService);
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
            DbContext.Add(new PullRequest
            {
                AuthorProfile = authorProfile,
                CreatedAt = pullRequest.CreatedAt,
                MergedAt = pullRequest.MergedAt,
                CommitsCount = pullRequest.CommitsCount,
                ReviewCommentsCount = pullRequest.ReviewCommentsCount,
                UpdatedAt = pullRequest.UpdatedAt,
                Title = pullRequest.Title,
                Body = pullRequest.Body,
                ExternalUrl = pullRequest.Url,
                ExternalAuthorId = pullRequest.Author.Id,
                ClosedAt = pullRequest.ClosedAt,
                ExternalId = pullRequest.Id
            });
            var logData = new LogCreateDTO
            {
                Event = LogEvents.PullRequestCreated,
                Data = new Dictionary<string, string>
                {
                    {"timestamp", DateTime.UtcNow.ToString()},
                    {"created_at", pullRequest.CreatedAt.ToString()},
                    {"externalUrl", pullRequest.Url},
                    {"externalAuthorUsername", pullRequest.Author.Username},
                    {"externalId", pullRequest.Id},
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

            PullRequest pullRequest =
                DbContext.PullRequests.FirstOrDefault(x => x.ExternalId == prReviewPayload.PullRequest.Id);

            if (pullRequest == null)
            {
                throw new Exception("Couldn't find the PullRequest");
            }
            PullRequestReviewDTO pullRequestReview = prReviewPayload.PullRequestReview;
            DbContext.Add(new PullRequestReview()
            {
                ReviewerProfile = reviewerProfile,
                CommitId = pullRequestReview.CommitId,
                State = pullRequestReview.State,
                SubmittedAt = pullRequestReview.SubmittedAt,
                Title = pullRequestReview.Title,
                Body = pullRequestReview.Body,
                ReviewerExternalId = pullRequestReview.ReviewUser.Id,
                PullRequest = pullRequest
            });
            var logData = new LogCreateDTO
            {
                Event = LogEvents.PullRequestReviewCreated,
                Data = new Dictionary<string, string>
                {
                    {"timestamp", DateTime.UtcNow.ToString()},
                    {"submitted_at", pullRequestReview.SubmittedAt.ToString()},
                    {"externalReviewerUsername", pullRequestReview.ReviewUser.Username},
                    {"externalId", pullRequestReview.Id},
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

        private void HandlePullRequestReviewComment(JObject jObject, ILogsService logsService)
        {
            PullRequestReviewCommentPayload prReviewCommentPayload =
                jObject.ToObject<PullRequestReviewCommentPayload>();
            var userCommentedPullRequestReviewProfile =
                ProfilesService.GetProfileByExternalId(
                    prReviewCommentPayload.ReviewComment.UserCommentedPullRequestReviewProfile.Username,
                    IntegrationType.GH);

            PullRequest pullRequest =
                DbContext.PullRequests.FirstOrDefault(x => x.ExternalId == prReviewCommentPayload.ReviewComment.Id);
            
            PullRequestReview pullRequestReview =
                DbContext.PullRequestReviews.FirstOrDefault(x =>
                    x.ReviewerExternalId == prReviewCommentPayload.ReviewComment.PullRequestReviewId);

            if (pullRequest == null)
            {
                throw new Exception("Couldn't find the PullRequest");
            }
            if (pullRequestReview == null)
            {
                throw new Exception("Couldn't find the PullRequest review");
            }

    PullRequestReviewCommentDTO pullRequestReviewComment = prReviewCommentPayload.ReviewComment;
            DbContext.Add(new PullRequestReviewComment()
            {
                UserCommentedPullRequestReviewProfile = userCommentedPullRequestReviewProfile,
                PullRequestReview = pullRequestReview,
                CreatedAt = pullRequestReviewComment.CreatedAt,
                Body = pullRequestReviewComment.Body,
                ExternalId = pullRequestReviewComment.Id,
                ExternalUrl = pullRequestReviewComment.Url,
                PullRequest = pullRequest,
                UpdatedAt = pullRequestReviewComment.UpdatedAt,
                
            });
            var logData = new LogCreateDTO
            {
                Event = LogEvents.PullRequestReviewCreated,
                Data = new Dictionary<string, string>
                {
                    {"timestamp", DateTime.UtcNow.ToString()},
                    {"created_at", pullRequestReviewComment.CreatedAt.ToString()},
                    {"externalReviewerUsername", pullRequestReviewComment.UserCommentedPullRequestReviewProfile.Username},
                    {"externalId", pullRequestReviewComment.Id},
                    {"external_url",pullRequestReviewComment.Url},
                }
            };
            if (userCommentedPullRequestReviewProfile != null)
            {
                logData.Data.Add("profileUsername", userCommentedPullRequestReviewProfile.Username);
                logData.ProfileId = userCommentedPullRequestReviewProfile.Id;
            }

            logsService.LogEvent(logData);
        }

        #endregion
    }
}