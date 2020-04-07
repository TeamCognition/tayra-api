using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
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
            //maybe not needed anymore
            if (issueChangelogs.Last().Created.ToUniversalTime() != DateTimeExtensions.ConvertUnixEpochTime(we.Timestamp))
            {
                return Ok();
            }

            TaskConverterJira taskConverter = new TaskConverterJira(
                DbContext,
                ProfilesService,
                we);
            TaskHelpers.DoStandardStuff(taskConverter, TokensService, LogsService);
            TasksService.AddOrUpdate(taskConverter.Data);
            DbContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}