using System;
using System.Collections.Generic;
using System.Linq;
using Cog.DAL;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Common
{
    public abstract class BaseConnector : IConnector
    {
        #region Constructor

        protected BaseConnector(ILogger<BaseConnector> logger, IHttpContextAccessor httpContext, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config) : this(logger, dataContext, catalogDbContext, config)
        {
            HttpContext = httpContext?.HttpContext;
            TenantInfo = HttpContext.GetMultiTenantContext<Tenant>()?.TenantInfo;
        }

        protected BaseConnector(ILogger<BaseConnector> logger, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config)
        {
            Logger = logger;
            OrganizationContext = dataContext;
            CatalogContext = catalogDbContext;
            TenantInfo = dataContext.TenantInfo;
            Config = config;
        }

        #endregion

        #region Properties

        public abstract IntegrationType Type { get; }

        protected ILogger<BaseConnector> Logger { get; }

        protected HttpContext HttpContext { get; }
        protected ITenantInfo TenantInfo { get; }
        protected OrganizationDbContext OrganizationContext { get; }
        protected CatalogDbContext CatalogContext { get; }
        protected IConfiguration Config { get; }

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

        protected Integration CreateSegmentIntegration(Guid segmentId, string installationId, Dictionary<string, string> fields, Integration oldIntegration = null)
        {
            return CreateProfileIntegration(null, segmentId, installationId, fields, oldIntegration);
        }

        protected Integration CreateProfileIntegration(Guid? profileId, Guid segmentId, string installationId, Dictionary<string, string> fields, Integration oldIntegration = null)
        {
            if (oldIntegration != null)
            {
                oldIntegration.Fields.ToList().ForEach(x => OrganizationContext.Remove(x));
                OrganizationContext.Remove(oldIntegration);
                if (oldIntegration.ProfileId == null)
                {
                    var x = CatalogContext.TenantIntegrations.FirstOrDefault(x =>
                        x.Type == oldIntegration.Type && x.SegmentId == oldIntegration.SegmentId && x.TenantId == TenantInfo.Id);

                    if (x != null) CatalogContext.TenantIntegrations.Remove(x);
                }
            }

            if (profileId != null)
            {
                var externalId = fields.GetValueOrDefault(Constants.PROFILE_EXTERNAL_ID);
                if (!string.IsNullOrEmpty(externalId))
                {
                    var eId = OrganizationContext.ProfileExternalIds.FirstOrDefault(x => x.ExternalId == externalId && x.IntegrationType == Type);
                    if (eId != null)
                    {
                        var someoneElsesI = OrganizationContext.Integrations.Include(x => x.Fields).FirstOrDefault(x => x.ProfileId == eId.ProfileId && x.Type == Type);
                        if (someoneElsesI != null)
                        {
                            someoneElsesI.Fields.ToList().ForEach(x => OrganizationContext.Remove(x));
                            OrganizationContext.Remove(someoneElsesI);
                        }

                        OrganizationContext.Remove(eId);
                    }
                    OrganizationContext.Add(new ProfileExternalId
                    {
                        ProfileId = profileId.Value,
                        SegmentId = segmentId,
                        IntegrationType = Type,
                        ExternalId = externalId
                    });
                }
            }

            if (profileId == null)
            {
                CatalogContext.TenantIntegrations.Add(new TenantIntegration
                {
                    TenantId = TenantInfo.Id,
                    Type = Type,
                    SegmentId = segmentId,
                    InstallationId = installationId,
                    Created = DateTime.UtcNow
                });
            }

            return OrganizationContext.Integrations.Add(new Integration
            {
                ProfileId = profileId,
                SegmentId = segmentId,
                Type = Type,
                Fields = fields.Select(x => new IntegrationField { Key = x.Key, Value = x.Value }).ToList()
            }).Entity;
        }

        protected string ReadField(Guid integrationId, string key, string errorMessage = null)
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