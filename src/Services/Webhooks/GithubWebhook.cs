using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services.webhooks
{
    public class GithubWebhook : IWebhook
    {
        //     public GithubWebhook(ProfilesService profilesService, OrganizationDbContext dbContext)
        //     {
        //         DbContext = dbContext;
        //     }
        //     private OrganizationDbContext DbContext { get; }
        //     
        //     private void SaveWebhookEventLog(JObject jObject, IntegrationType integrationType)
        //     {
        //         DbContext.WebhookEventLogs.Add(new WebhookEventLog { IntegrationType = integrationType, Data = jObject.ToString(Formatting.None) });
        //         DbContext.SaveChanges();
        //     }
        //
        //     public string HandleWebhook(JObject jObject, string ghEvent)
        //     {  
        //         PushWebhookPayload payload = jObject.ToObject<PushWebhookPayload>();
        //
        //         if (!DbContext.Repositories.Any(x => x.ExternalId == payload.Repository.Id))
        //         {
        //             return "skipped - repo not active";
        //         }
        //
        //         SaveWebhookEventLog(jObject, IntegrationType.GH);
        //         
        //         var now = DateTime.UtcNow;
        //         foreach (var commit in payload.Commits)
        //         {
        //             if(!commit.Distinct)
        //                 continue;
        //
        //             var authorProfile = ProfilesService.GetProfileByExternalId(commit.Author.Username, IntegrationType.GH);
        //             DbContext.Add(new GitCommit
        //             {
        //                 SHA = commit.Id,
        //                 AuthorProfile = authorProfile,
        //                 AuthorExternalId = commit.Author.Username,
        //                 Message = commit.Message,
        //                 ExternalUrl = commit.Url
        //             });
        //             
        //             var logData = new LogCreateDTO
        //             {
        //                 Event = LogEvents.CodeCommitted,
        //                 Data = new Dictionary<string, string>
        //                 {
        //                     {"timestamp", now.ToString()},
        //                     {"committedAt", commit.Timestamp.ToString()},
        //                     {"externalUrl", commit.Url},
        //                     {"externalAuthorUsername", commit.Author.Username},
        //                     {"sha", commit.Id},
        //                     {"message", commit.Message},
        //                 }
        //             };
        //             
        //             if (authorProfile != null)
        //             {
        //                 logData.Data.Add("profileUsername", authorProfile.Username);
        //                 logData.ProfileId = authorProfile.Id;
        //             }
        //             logsService.LogEvent(logData);
        //         }
        //         
        //         DbContext.SaveChanges();
        //         return "ok";
        //     }
        //     }
        // }
        public string HandleWebhook(JObject jObject, string ghEvent)
        {
            throw new NotImplementedException();
        }
    }
}