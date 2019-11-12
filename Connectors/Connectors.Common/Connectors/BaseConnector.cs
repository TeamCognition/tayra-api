using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestSharp;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Common
{
    public abstract class BaseConnector : IConnector
    {
        #region Constructor

        protected BaseConnector(ILogger logger, IHttpContextAccessor httpContext, OrganizationDbContext dataContext)
        {
            Logger = logger;
            HttpContext = httpContext?.HttpContext;
            OrganizationContext = dataContext;
        }

        #endregion

        #region Properties

        public abstract IntegrationType Type { get; }

        protected ILogger Logger { get; }

        protected HttpContext HttpContext { get; }

        protected OrganizationDbContext OrganizationContext { get; }

        #endregion

        #region Protected Methods

        protected virtual void Log(IRestResponse response)
        {
            Console.WriteLine(response.Content);
            //if (response != null && TenantContext != null)
            //{
            //    try
            //    {
            //        var log = new RequestLog
            //        {
            //            Response = response.Content,
            //            Url = response.Request.Resource,
            //            Payload = JsonConvert.SerializeObject(response.Request.Parameters),
            //            ServiceType = Type.ToString(),
            //            CreatedOn = DateTime.UtcNow.Ticks,
            //            IsSuccessful = response.IsSuccessful
            //        };

            //        TenantContext.RequestLogs.Add(log);
            //        TenantContext.SaveChanges();
            //    }
            //    catch
            //    {
            //    }
            //}
        }

        protected Integration CreateIntegration(int projectId, Dictionary<string, string> fields)
        {
            var account = new Integration
            {
                ProjectId = projectId,
                Type = Type,
                Fields = fields.Select(x => new IntegrationField { Key = x.Key, Value = x.Value }).ToList()
            };
            OrganizationContext.Integrations.Add(account);
            OrganizationContext.SaveChanges();
            return account;
        }

        protected string ReadField(int integrationId, string key, string errorMessage = null)
        {
            var field = OrganizationContext.IntegrationFields.FirstOrDefault(a => a.IntegrationId == integrationId && a.Key == key);

            if (string.IsNullOrWhiteSpace(field?.Value) && !string.IsNullOrEmpty(errorMessage))
            {
                throw new ApplicationException(errorMessage);
            }

            return field?.Value;
        }

        #endregion
    }
}
