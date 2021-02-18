using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Services.Models.Profiles;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Connectors.GitHub;
using Tayra.Connectors.GitHub.Helper;
using Tayra.Connectors.GitHub.WebhookPayloads;
using Tayra.Models.Organizations;
using Tayra.Services.Contracts;

namespace Tayra.Services.webhooks
{
    public class GithubWebhookServiceService : BaseService<OrganizationDbContext>, IGithubWebhookService
    {
        private readonly ILogsService LogsService;
        
        public GithubWebhookServiceService(ILogsService logsService,
            OrganizationDbContext dbContext) : base(dbContext)
        {
            LogsService = logsService;
        }


        private void SaveWebhookEventLog(JObject jObject, IntegrationType integrationType)
        {
            DbContext.WebhookEventLogs.Add(new WebhookEventLog
            { IntegrationType = integrationType, Data = jObject.ToString(Formatting.None) });
            DbContext.SaveChanges();
        }

        public string HandleWebhook(JObject jObject, string ghEvent)
        {
            if (!new string[] { "push", "pull_request", "pull_request_review", "pull_request_review_comment" }
                .Contains(ghEvent.ToString()))
            {
                return "skipped";
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
                    HandlePush(jObject);
                    break;
                case "pull_request":
                    HandlePullRequest(jObject);
                    break;
                case "pull_request_review":
                    HandlePullRequestReview(jObject);
                    break;
                case "pull_request_review_comment":
                    HandlePullRequestReviewComment(jObject);
                    break;
            }

            DbContext.SaveChanges();
            return "";
        }

        private void HandlePush(JObject jObject)
        {
            PushWebhookPayload payload = jObject.ToObject<PushWebhookPayload>();

            foreach (var commit in payload.Commits)
            {
                if (!commit.Distinct)
                    continue;
                Guid? integrationId = IntegrationHelpers.GetIntegrationId(DbContext, "", IntegrationType.GH);
                if (!integrationId.HasValue)
                {
                    throw new ApplicationException("GH integration token not found");
                }
                string token = ReadAccessToken(integrationId.Value);
                var commitWithChanges = GitHubService.GetCommitBySha(commit.Id, token, "xxxx", "xxxx");
                var authorProfile =
                    new ProfilesService().GetProfileByExternalId(DbContext, commit.Author.Username, IntegrationType.GH);
                DbContext.Add(new GitCommit
                {
                    SHA = commit.Id,
                    AuthorProfile = authorProfile,
                    AuthorExternalId = commit.Author.Username,
                    Message = commit.Message,
                    ExternalUrl = commit.Url,
                    Additions = commitWithChanges.Additions,
                    Deletions = commitWithChanges.Deletions
                });

                CreateLog(LogsService, LogEvents.CodeCommitted, authorProfile, commit.Message, commit.Url,
                    new Dictionary<string, string>
                    {
                        {"committedAt", commit.Timestamp.ToString()},
                        {"externalAuthorUsername", commit.Author.Username},
                        {"sha", commit.Id},
                    });
            }
        }

        private void HandlePullRequest(JObject jObject)
        {
            PullRequestWebhookPayload prPayload = jObject.ToObject<PullRequestWebhookPayload>();
            if (PrWebhooksContstants.PullRequestIgnoredActions.Contains(prPayload.Action))
            {
                return;
            }

            var authorProfile =
                new ProfilesService().GetProfileByExternalId(DbContext,
                    prPayload.PullRequest.Author.Username,
                    IntegrationType.GH);
            if (prPayload.Action == "edited")
            {
                UpdatePullRequest(prPayload, authorProfile, LogsService);
                return;
            }

            PullRequestWebhookPayload.PullRequestDTO pullRequest = prPayload.PullRequest;
            CreatePullRequest(pullRequest, authorProfile);
            CreateLog(LogsService, LogEvents.PullRequestCreated, authorProfile, pullRequest.Title, pullRequest.Url,
                new Dictionary<string, string>
                {
                    {"externalAuthorUsername", pullRequest.Author.Username},
                    {"externalId", pullRequest.Id}
                });
        }

        private void HandlePullRequestReview(JObject jObject)
        {
            PullRequsetReviewWebhookPayload prReviewPayload = jObject.ToObject<PullRequsetReviewWebhookPayload>();
            var reviewerProfile =
                new ProfilesService().GetProfileByExternalId(DbContext,
                    prReviewPayload.PullRequestReview.ReviewedBy.Username,
                    IntegrationType.GH);

            PullRequest pullRequest =
                DbContext.PullRequests.FirstOrDefault(x => x.ExternalId == prReviewPayload.PullRequest.Id);

            if (pullRequest == null)
            {
                throw new Exception(PrWebhooksContstants.COULDNT_FIND_PR);
            }

            if (prReviewPayload.Action == "edited")
            {
                UpdatePullRequestReview(prReviewPayload, reviewerProfile, LogsService);
                return;
            }

            PullRequsetReviewWebhookPayload.PullRequestReviewDTO pullRequestReview = prReviewPayload.PullRequestReview;
            CreatePullRequestReview(pullRequestReview, reviewerProfile, pullRequest);
            
            CreateLog(LogsService, LogEvents.PullRequestReviewCreated, reviewerProfile, pullRequest.Title, pullRequest.ExternalUrl,
                new Dictionary<string, string>
                {
                    {"externalReviewerUsername", pullRequestReview.ReviewedBy.Username},
                    {"externalId", pullRequestReview.Id},
                });
        }

        private void HandlePullRequestReviewComment(JObject jObject)
        {
            PullRequestReviewCommentPayload prReviewCommentPayload =
                jObject.ToObject<PullRequestReviewCommentPayload>();
            var commenterProfile =
            new ProfilesService().GetProfileByExternalId(DbContext,
                prReviewCommentPayload.ReviewComment.CommenterProfile.Username,
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

            PullRequestReviewCommentPayload.PullRequestReviewCommentDTO pullRequestReviewComment =
                prReviewCommentPayload.ReviewComment;
            if (prReviewCommentPayload.Action == "edited")
            {
                UpdatePullRequestReviewComment(pullRequestReviewComment, commenterProfile, LogsService);
                return;
            }

            CreatePullRequestReviewComment(pullRequestReviewComment, commenterProfile,
                pullRequestReview, pullRequest);

            CreateLog(LogsService, LogEvents.PullRequestReviewCommentCreated, commenterProfile, pullRequestReviewComment.Body, pullRequestReviewComment.Url,
                new Dictionary<string, string>
                {
                    {"externalReviewerUsername", pullRequestReviewComment.CommenterProfile.Username},
                    {"externalId", pullRequestReviewComment.Id},
                });
        }


        private void CreatePullRequestReviewComment(
            PullRequestReviewCommentPayload.PullRequestReviewCommentDTO pullRequestReviewComment,
            Profile commenterProfile,
            PullRequestReview pullRequestReview, PullRequest pullRequest)
        {
            DbContext.Add(new PullRequestReviewComment()
            {
                CommenterProfile = commenterProfile,
                PullRequestReview = pullRequestReview,
                ExternalCreatedAt = pullRequestReviewComment.CreatedAt,
                Body = pullRequestReviewComment.Body,
                ExternalId = pullRequestReviewComment.Id,
                ExternalUrl = pullRequestReviewComment.Url,
                PullRequest = pullRequest,
                ExternalUpdatedAt = pullRequestReviewComment.UpdatedAt,
            });
        }

        private void UpdatePullRequestReviewComment(
            PullRequestReviewCommentPayload.PullRequestReviewCommentDTO pullRequestReviewCommentDto,
            Profile commenterProfile,
            ILogsService logsService)
        {
            PullRequestReviewComment pullRequestReviewComment =
                DbContext.PullRequestReviewComments.FirstOrDefault(x => x.ExternalId == pullRequestReviewCommentDto.Id);
            pullRequestReviewComment.Body = pullRequestReviewCommentDto.Body;
            pullRequestReviewComment.ExternalUpdatedAt = pullRequestReviewCommentDto.UpdatedAt;
            DbContext.Update(pullRequestReviewCommentDto);
            
            CreateLog(LogsService, LogEvents.PullRequestReviewCommentUpdated, commenterProfile, pullRequestReviewCommentDto.Body, pullRequestReviewCommentDto.Url,
                new Dictionary<string, string>
                {
                    {"externalReviewerUsername", pullRequestReviewComment.CommenterProfile.Username},
                    {"externalId", pullRequestReviewCommentDto.Id},
                });
        }

        private void CreatePullRequestReview(PullRequsetReviewWebhookPayload.PullRequestReviewDTO pullRequestReview,
            Profile reviewerProfile,
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

        private void UpdatePullRequestReview(PullRequsetReviewWebhookPayload prReviewPayload, Profile reviewerProfile,
            ILogsService logsService)
        {
            PullRequsetReviewWebhookPayload.PullRequestReviewDTO pullRequestReviewDto =
                prReviewPayload.PullRequestReview;
            PullRequestReview pullRequestReview =
                DbContext.PullRequestReviews.FirstOrDefault(x => x.ReviewExternalId == pullRequestReviewDto.Id);
            pullRequestReview.Body = pullRequestReviewDto.Body;
            pullRequestReview.State = pullRequestReviewDto.State;
            pullRequestReview.SubmittedAt = pullRequestReviewDto.SubmittedAt;
            DbContext.Update(pullRequestReview);
            
            CreateLog(LogsService, LogEvents.PullRequestReviewUpdated, reviewerProfile, pullRequestReviewDto.Title, "FIX ME FEZA",
                new Dictionary<string, string>
                {
                    {"externalReviewerUsername", pullRequestReviewDto.ReviewedBy.Username},
                    {"externalId", pullRequestReviewDto.Id},
                });
        }

        private void CreatePullRequest(PullRequestWebhookPayload.PullRequestDTO pullRequest, Profile authorProfile)
        {
            DbContext.Add(new PullRequest
            {
                AuthorProfile = authorProfile,
                ExternalCreatedAt = pullRequest.CreatedAt,
                MergedAt = pullRequest.MergedAt,
                IsLocked = pullRequest.IsLocked,
                CommitsCount = pullRequest.CommitsCount,
                ReviewCommentsCount = pullRequest.ReviewCommentsCount,
                ExternalUpdatedAt = pullRequest.UpdatedAt,
                Title = pullRequest.Title,
                Body = pullRequest.Body,
                ExternalUrl = pullRequest.Url,
                ExternalAuthorId = pullRequest.Author.Id,
                ClosedAt = pullRequest.ClosedAt,
                ExternalId = pullRequest.Id,
                ExternalNumber = pullRequest.Number,
                State = pullRequest.State
            });
        }

        private void UpdatePullRequest(PullRequestWebhookPayload prPayload, Profile authorProfile,
            ILogsService logsService)
        {
            PullRequestWebhookPayload.PullRequestDTO pullRequestDto = prPayload.PullRequest;
            PullRequest pullRequest = DbContext.PullRequests.FirstOrDefault(x => x.ExternalId == pullRequestDto.Id);
            pullRequest.Body = pullRequestDto.Body;
            pullRequest.Title = pullRequestDto.Title;
            pullRequest.ExternalUpdatedAt = pullRequestDto.UpdatedAt;
            pullRequest.IsLocked = pullRequestDto.IsLocked;
            pullRequest.MergedAt = pullRequestDto.MergedAt;
            pullRequest.ClosedAt = pullRequestDto.ClosedAt;
            pullRequest.State = pullRequestDto.State;
            DbContext.Update(pullRequest);
            
            CreateLog(LogsService, LogEvents.PullRequestUpdated, authorProfile, pullRequestDto.Title, pullRequestDto.Url,
                new Dictionary<string, string>
                {
                    {"externalAuthorUsername", pullRequestDto.Author.Username},
                    {"externalId", pullRequestDto.Id},
                    {"prState", pullRequestDto.State},
                });
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
        
        private string ReadAccessToken(Guid integrationId)
        {
            var field = DbContext.IntegrationFields.FirstOrDefault(a => a.IntegrationId == integrationId && a.Key == GHConstants.GH_ACCESS_TOKEN);

            if (string.IsNullOrWhiteSpace(field?.Value))
            {
                throw new ApplicationException("Unable to access the integration account, access token is missing or has expired");
            }

            return field?.Value;
        }
    }
}