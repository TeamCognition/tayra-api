using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
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

        public WebhooksController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
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
            DbContext.WebhookEventLogs.Add(new WebhookEventLog { IntegrationType = integrationType, Data = jObject.ToString(Formatting.None) });
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
            if (ghEvent.ToString() != "push")
            {
                return Ok("skipped");
            }
            
            SaveWebhookEventLog(jObject, IntegrationType.GH);
            PushWebhookPayload payload = jObject.ToObject<PushWebhookPayload>();

            var now = DateTime.UtcNow;
            foreach (var commit in payload.Commits)
            {
                if(!commit.Distinct)
                    continue;

                var authorProfile = ProfilesService.GetMemberByExternalId(commit.Author.Username, IntegrationType.GH);
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
                        {"timestamp", now.ToString()},
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
            
            DbContext.SaveChanges();
            return Ok();
        }
        
        #endregion
    }
}