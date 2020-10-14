﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.GitHub.Helper;
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

            if (!new string[] {"push", "pull_request", "pull_request_review", "pull_request_review_comment"}
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

            string returnMessage = null;
            switch (ghEvent.ToString())
            {
                case "push":
                    HandlePush(jObject, logsService);
                    break;
                case "pull_request":
                    returnMessage = HandlePullRequest(jObject, logsService);
                    break;
                case "pull_request_review":
                    returnMessage = HandlePullRequestReview(jObject, logsService);
                    break;
                case "pull_request_review_comment":
                    returnMessage = HandlePullRequestReviewComment(jObject, logsService);
                    break;
            }

            if (returnMessage == null)
            {
                returnMessage = "";
            }

            DbContext.SaveChanges();
            return Ok(returnMessage);
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

                CreateLog(new Dictionary<string, string>
                {
                    {"timestamp", DateTime.UtcNow.ToString()},
                    {"committedAt", commit.Timestamp.ToString()},
                    {"externalUrl", commit.Url},
                    {"externalAuthorUsername", commit.Author.Username},
                    {"sha", commit.Id},
                    {"message", commit.Message},
                }, LogEvents.CodeCommitted, authorProfile, logsService);
            }
        }

        private string HandlePullRequest(JObject jObject, ILogsService logsService)
        {
            PullRequestWebhookPayload prPayload = jObject.ToObject<PullRequestWebhookPayload>();
            if (PrWebhooksContstants.PullRequestIgnoredActions.Contains(prPayload.Action))
            {
                return PrWebhooksContstants.PR_ACTION_IGNORED;
            }

            var authorProfile =
                ProfilesService.GetProfileByExternalId(prPayload.PullRequest.Author.Username, IntegrationType.GH);
            if (prPayload.Action == "edited")
            {
                UpdatePullRequest(prPayload,authorProfile,logsService);
                return PrWebhooksContstants.PR_UPDATED;
            }

          
            PullRequestDTO pullRequest = prPayload.PullRequest;
            CreatePullRequest(pullRequest, authorProfile);
            CreateLog(new Dictionary<string, string>
            {
                {"timestamp", DateTime.UtcNow.ToString()},
                {"created_at", pullRequest.CreatedAt.ToString()},
                {"externalUrl", pullRequest.Url},
                {"externalAuthorUsername", pullRequest.Author.Username},
                {"externalId", pullRequest.Id},
                {"title", pullRequest.Title},
            }, LogEvents.PullRequestCreated, authorProfile, logsService);
            return PrWebhooksContstants.PR_CREATED;
        }


        private string HandlePullRequestReview(JObject jObject, ILogsService logsService)
        {
            PullRequsetReviewWebhookPayload prReviewPayload = jObject.ToObject<PullRequsetReviewWebhookPayload>();
            var reviewerProfile =
                ProfilesService.GetProfileByExternalId(prReviewPayload.PullRequestReview.ReviewUser.Username,
                    IntegrationType.GH);

            PullRequest pullRequest =
                DbContext.PullRequests.FirstOrDefault(x => x.ExternalId == prReviewPayload.PullRequest.Id);

            if (pullRequest == null)
            {
                throw new Exception(PrWebhooksContstants.COULDNT_FIND_PR);
            }

            if (prReviewPayload.Action == "edited")
            {
                UpdatePullRequestReview(prReviewPayload,reviewerProfile,logsService);
                return PrWebhooksContstants.PR_REVIEW_UPDATED;
            }

            PullRequestReviewDTO pullRequestReview = prReviewPayload.PullRequestReview;
            CreatePullRequestReview(pullRequestReview, reviewerProfile, pullRequest);
            CreateLog(new Dictionary<string, string>
            {
                {"timestamp", DateTime.UtcNow.ToString()},
                {"submitted_at", pullRequestReview.SubmittedAt.ToString()},
                {"externalReviewerUsername", pullRequestReview.ReviewUser.Username},
                {"externalId", pullRequestReview.Id},
                {"title", pullRequestReview.Title},
            }, LogEvents.PullRequestReviewCreated, reviewerProfile, logsService);
            return PrWebhooksContstants.PR_REVIEW_CREATED;
        }


        private string HandlePullRequestReviewComment(JObject jObject, ILogsService logsService)
        {
            PullRequestReviewCommentPayload prReviewCommentPayload =
                jObject.ToObject<PullRequestReviewCommentPayload>();
            var userCommentedPullRequestReviewProfile =
                ProfilesService.GetProfileByExternalId(
                    prReviewCommentPayload.ReviewComment.UserCommentedPullRequestReviewProfile.Username,
                    IntegrationType.GH);
            PullRequest pullRequest =
                DbContext.PullRequests.FirstOrDefault(x => x.ExternalId == prReviewCommentPayload.PullRequest.Id);

            PullRequestReview pullRequestReview =
                DbContext.PullRequestReviews.FirstOrDefault(x =>
                    x.ReviewExternalId == prReviewCommentPayload.ReviewComment.PullRequestReviewId);

            if (pullRequest == null)
            {
                throw new Exception(PrWebhooksContstants.COULDNT_FIND_PR);
            }

            if (pullRequestReview == null)
            {
                throw new Exception(PrWebhooksContstants.COULDNT_FIND_PR_REVIEW);
            }

            PullRequestReviewCommentDTO pullRequestReviewComment = prReviewCommentPayload.ReviewComment;
            if (prReviewCommentPayload.Action == "edited")
            {
                UpdatePullRequestReviewComment(pullRequestReviewComment,userCommentedPullRequestReviewProfile,logsService);
                return PrWebhooksContstants.COMMENT_UPDATED;
            }

            CreatePullRequestReviewComment(pullRequestReviewComment, userCommentedPullRequestReviewProfile,
                pullRequestReview, pullRequest);
            CreateLog(new Dictionary<string, string>
            {
                {"timestamp", DateTime.UtcNow.ToString()},
                {"created_at", pullRequestReviewComment.CreatedAt.ToString()},
                {"externalReviewerUsername", pullRequestReviewComment.UserCommentedPullRequestReviewProfile.Username},
                {"externalId", pullRequestReviewComment.Id},
                {"external_url", pullRequestReviewComment.Url},
            }, LogEvents.PullRequestReviewCommentCreated, userCommentedPullRequestReviewProfile, logsService);
            return PrWebhooksContstants.COMMENT_CREATED;
        }


        private void CreatePullRequestReviewComment(PullRequestReviewCommentDTO pullRequestReviewComment,
            Profile userCommentedPullRequestReviewProfile,
            PullRequestReview pullRequestReview, PullRequest pullRequest)
        {
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
        }

        private void UpdatePullRequestReviewComment(PullRequestReviewCommentDTO pullRequestReviewCommentDto,Profile userCommentedPullRequestReviewProfile,
            ILogsService logsService)
        {
            PullRequestReviewComment pullRequestReviewComment =
                DbContext.PullRequestReviewComments.FirstOrDefault(x => x.ExternalId == pullRequestReviewCommentDto.Id);
            pullRequestReviewComment.Body = pullRequestReviewCommentDto.Body;
            pullRequestReviewComment.UpdatedAt = pullRequestReviewCommentDto.UpdatedAt;
            DbContext.Update(pullRequestReviewCommentDto);
            CreateLog(new Dictionary<string, string>
            {
                {"timestamp", DateTime.UtcNow.ToString()},
                {"created_at", pullRequestReviewCommentDto.CreatedAt.ToString()},
                {"externalReviewerUsername", pullRequestReviewComment.UserCommentedPullRequestReviewProfile.Username},
                {"externalId", pullRequestReviewCommentDto.Id},
                {"external_url", pullRequestReviewCommentDto.Url},
            }, LogEvents.PullRequestReviewCommentCreated, userCommentedPullRequestReviewProfile, logsService);
            
        }

        private void CreatePullRequestReview(PullRequestReviewDTO pullRequestReview, Profile reviewerProfile,
            PullRequest pullRequest)
        {
            DbContext.Add(new PullRequestReview()
            {
                ReviewerProfile = reviewerProfile,
                CommitId = pullRequestReview.CommitId,
                State = pullRequestReview.State,
                SubmittedAt = pullRequestReview.SubmittedAt,
                Body = pullRequestReview.Body,
                ReviewExternalId = pullRequestReview.Id,
                PullRequest = pullRequest
            });
        }

        private void UpdatePullRequestReview(PullRequsetReviewWebhookPayload prReviewPayload,Profile reviewerProfile, ILogsService logsService)
        {
            PullRequestReviewDTO pullRequestReviewDto = prReviewPayload.PullRequestReview;
            PullRequestReview pullRequestReview =
                DbContext.PullRequestReviews.FirstOrDefault(x => x.ReviewExternalId == pullRequestReviewDto.Id);
            pullRequestReview.Body = pullRequestReviewDto.Body;
            pullRequestReview.State = pullRequestReviewDto.State;
            pullRequestReview.SubmittedAt = pullRequestReviewDto.SubmittedAt;
            DbContext.Update(pullRequestReview);
            CreateLog(new Dictionary<string, string>
            {
                {"timestamp", DateTime.UtcNow.ToString()},
                {"submitted_at", pullRequestReviewDto.SubmittedAt.ToString()},
                {"externalReviewerUsername", pullRequestReviewDto.ReviewUser.Username},
                {"externalId", pullRequestReviewDto.Id},
                {"title", pullRequestReviewDto.Title},
            }, LogEvents.PullRequestReviewCreated, reviewerProfile, logsService);
        }

        private void CreatePullRequest(PullRequestDTO pullRequest, Profile authorProfile)
        {
            DbContext.Add(new PullRequest
            {
                AuthorProfile = authorProfile,
                CreatedAt = pullRequest.CreatedAt,
                MergedAt = pullRequest.MergedAt,
                IsLocked = pullRequest.IsLocked,
                CommitsCount = pullRequest.CommitsCount,
                ReviewCommentsCount = pullRequest.ReviewCommentsCount,
                UpdatedAt = pullRequest.UpdatedAt,
                Title = pullRequest.Title,
                Body = pullRequest.Body,
                ExternalUrl = pullRequest.Url,
                ExternalAuthorId = pullRequest.Author.Id,
                ClosedAt = pullRequest.ClosedAt,
                ExternalId = pullRequest.Id,
                State = pullRequest.State,
            });
        }

        private void UpdatePullRequest(PullRequestWebhookPayload prPayload,Profile authorProfile,ILogsService logsService)
        {
            PullRequestDTO pullRequestDto = prPayload.PullRequest;
            PullRequest pullRequest = DbContext.PullRequests.FirstOrDefault(x => x.ExternalId == pullRequestDto.Id);
            pullRequest.Body = pullRequestDto.Body;
            pullRequest.Title = pullRequestDto.Title;
            pullRequest.UpdatedAt = pullRequestDto.UpdatedAt;
            pullRequest.IsLocked = pullRequestDto.IsLocked;
            pullRequest.MergedAt = pullRequestDto.MergedAt;
            pullRequest.ClosedAt = pullRequestDto.ClosedAt;
            pullRequest.State = pullRequestDto.State;
            DbContext.Update(pullRequest);
            CreateLog(new Dictionary<string, string>
            {
                {"timestamp", DateTime.UtcNow.ToString()},
                {"created_at", pullRequestDto.CreatedAt.ToString()},
                {"externalUrl", pullRequestDto.Url},
                {"externalAuthorUsername", pullRequestDto.Author.Username},
                {"externalId", pullRequestDto.Id},
                {"title", pullRequestDto.Title},
            }, LogEvents.PullRequestCreated, authorProfile, logsService);
        }

        private void CreateLog(Dictionary<string, string> log, LogEvents events, Profile profile,
            ILogsService logsService)
        {
            var logData = new LogCreateDTO {Event = events, Data = log};
            if (profile != null)
            {
                logData.Data.Add("profileUsername", profile.Username);
                logData.ProfileId = profile.Id;
            }

            logsService.LogEvent(logData);
        }

        #endregion
    }
}