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

        protected BaseConnector(ILogger logger, IHttpContextAccessor httpContext, ITenantProvider tenantProvider, OrganizationDbContext dataContext)
        {
            Logger = logger;
            HttpContext = httpContext?.HttpContext;
            Tenant = tenantProvider.GetTenant();
            OrganizationContext = dataContext;
        }

        #endregion

        #region Properties

        public abstract IntegrationType Type { get; }

        protected ILogger Logger { get; }

        protected HttpContext HttpContext { get; }
        protected TenantDTO Tenant { get; }
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

        protected Integration CreateProjectIntegration(int projectId, Dictionary<string, string> fields, Integration oldIntegration = null)
        {
            return CreateProfileIntegration(null, projectId, fields, oldIntegration);
        }

        protected Integration CreateProfileIntegration(int? profileId, int projectId, Dictionary<string, string> fields, Integration oldIntegration = null)
        {
            if (oldIntegration != null)
            {
                oldIntegration.Fields.ToList().ForEach(x => OrganizationContext.Remove(x));
                OrganizationContext.Remove(oldIntegration);
            }

            if(profileId != null)
            {
                var externalId = fields.GetValueOrDefault(Constants.USER_ACCOUNT_ID);
                if (!string.IsNullOrEmpty(externalId))
                {
                    var eId = OrganizationContext.ProfileExternalIds.FirstOrDefault(x => x.ProfileId == profileId && x.ProjectId == projectId && x.IntegrationType == Type);
                    if (eId != null)
                        OrganizationContext.Remove(eId);

                    OrganizationContext.Add(new ProfileExternalId
                    {
                        ProfileId = profileId.Value,
                        ProjectId = projectId,
                        IntegrationType = Type,
                        ExternalId = externalId
                    });
                }
            }

            return OrganizationContext.Integrations.Add(new Integration
            {
                ProfileId = profileId,
                ProjectId = projectId,
                Type = Type,
                Fields = fields.Select(x => new IntegrationField { Key = x.Key, Value = x.Value }).ToList()
            }).Entity;
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
