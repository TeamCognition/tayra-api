using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Connectors.Atlassian.Jira;
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

        private void SaveWebhookEventLog(JObject jObject)
        {
            DbContext.WebhookEventLogs.Add(new WebhookEventLog { Data = jObject.ToString(Formatting.None) });
            DbContext.SaveChanges();
        }

        [HttpPost("atjissueupdate")]
        [AllowAnonymous]
        public ActionResult JiraIssueUpdate([FromBody] JObject jObject)
        {
            SaveWebhookEventLog(jObject);
            WebhookEvent we = jObject.ToObject<WebhookEvent>();

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

        public interface IABC
        {
            string T { get; set; }
            void Process();
        }

        public class A : IABC
        {
            public string T { get; set; }
            public int botic { get; set; }

            public void Process()
            {
                T = "from A";
            }
        }
        
        public class B : IABC
        {public string T { get; set; }
            public int haris { get; set; }
            public void Process(){T = "from B";}
        }
        
        [HttpPost("gh")]
        [AllowAnonymous]
        public ActionResult GithubWebhook([FromBody] JObject jObject)
        {
            var x = jObject.ToObject<IABC>();
            x.Process();
            //SaveWebhookEventLog(jObject);
            // WebhookEvent we = jObject.ToObject<WebhookEvent>();
            //
            // TaskConverterJira taskConverter = new TaskConverterJira(
            //     DbContext,
            //     ProfilesService,
            //     we);
            // if (TaskHelpers.DoStandardStuff(taskConverter, TasksService, TokensService, LogsService, AssistantService))
            // {
            //     DbContext.SaveChanges();
            // }

            return Ok(x);
        }
        
        #endregion
    }
}