using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Common
{
    public abstract class BaseOAuthConnector : BaseConnector, IOAuthConnector
    {
        #region Constructor

        protected BaseOAuthConnector(ILogger logger, IHttpContextAccessor httpContext, ITenantProvider tenantProvider, OrganizationDbContext dataContext) : base(logger, httpContext, tenantProvider, dataContext)
        {
        }

        protected BaseOAuthConnector(ILogger logger, OrganizationDbContext dataContext) : base(logger, dataContext)
        {
        }

        #endregion

        #region Public Methods

        public abstract string GetAuthUrl(string userState);

        public abstract Integration Authenticate(int profileId, ProfileRoles profileRole, int segmentId, string userState);

        public virtual  Integration RefreshToken(int integrationId)
        {
            throw new NotImplementedException("Not supported by this network");
        }

        public virtual string GetAuthDoneUrl(bool isSuccessful)
        {
            string protocol = Tenant.Host.StartsWith("localhost:", StringComparison.InvariantCulture) ? "http" : "https"; 
            return $"{protocol}://{Tenant.Host}/integrations/{Type.ToString().ToLower()}/done?successful={isSuccessful}";
        }

        #endregion

        #region Protected Methods

        protected virtual string GetCallbackUrl(string userState)//userState is not used, but everything works
        {
            return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/external/callback/{Type.ToString().ToLower()}";
        }

        protected string ReadAccessToken(int integrationId)
        {
            return ReadField(integrationId, Constants.ACCESS_TOKEN, "Unable to access the integration account, access token is missing or has expired");
        }

        protected string ReadAccessTokenType(int integrationId)
        {
            return ReadField(integrationId, Constants.ACCESS_TOKEN_TYPE) ?? "Bearer";
        }

        #endregion
    }
}