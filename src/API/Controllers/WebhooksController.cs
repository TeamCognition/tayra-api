using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.Slack;
using Tayra.Connectors.Slack.DTOs;
using Tayra.Mailer;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.Services.Contracts;
using Tayra.Services.TaskConverters;
using Tayra.Mailer.Templates.PraiseReceived;

namespace Tayra.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WebhooksController : BaseController
    {
        #region Constructor
        public WebhooksController(IServiceProvider serviceProvider, IGithubWebhookService webhookService, OrganizationDbContext dbContext) : base(

            serviceProvider)
        {
            DbContext = dbContext;
            this.webhookService = webhookService;
        }

        #endregion

        #region Properties
        
        private IGithubWebhookService webhookService { get; }

        private OrganizationDbContext DbContext { get; }

        #endregion

        #region Action Methods

        private void SaveWebhookEventLog(JObject jObject, IntegrationType integrationType)
        {
            DbContext.WebhookEventLogs.Add(new WebhookEventLog
            { IntegrationType = integrationType, Data = jObject.ToString(Formatting.None) });
            DbContext.SaveChanges();
        }

        [HttpPost("atjissueupdate")]
        [AllowAnonymous]
        public ActionResult JiraIssueUpdate([FromBody] JObject jObject, [FromServices]IConfiguration config)
        {
            SaveWebhookEventLog(jObject, IntegrationType.ATJ);
            JiraWebhookEvent we = jObject.ToObject<JiraWebhookEvent>();

            TaskConverterJira taskConverter = new TaskConverterJira(DbContext, we, config);
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
            return Ok(webhookService.HandleWebhook(jObject, ghEvent));
        }

        [HttpPost("test")]
        [AllowAnonymous]
        public ActionResult TestMethods([FromQuery] string id, [FromBody] SlackMessageRequestDto requestDto)
        { 
            
            var res = SlackService.SendSlackMessage("xoxb-698826045604-1117671360278-zB1nNQLCkjI3iR8qXuvZGM7E",requestDto);
            Console.WriteLine(res);
            return Ok(res);
        }
        
        [HttpPost("testMailer")]
        [AllowAnonymous]
        public ActionResult TestMailer([FromServices] IMailerService mailerService)
        {
            var res = mailerService.SendEmail("haris.botic96@gmail.com",
                new TemplateModelPraiseReceived("You received a praise from someone",
                    receiverFirstName: "Bota",
                    senderFirstName: "Ejub",
                    url: "https://github.com/toddams/RazorLight",
                    PraiseTypes.HardWorker));
            Console.WriteLine(res);
            return Ok(res);
        }
        
        #endregion
    }
}