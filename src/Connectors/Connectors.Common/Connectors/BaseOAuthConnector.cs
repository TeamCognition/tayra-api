using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Common
{
    public abstract class BaseOAuthConnector : BaseConnector, IOAuthConnector
    {
        #region Constructor

        protected BaseOAuthConnector(ILogger logger, IHttpContextAccessor httpContext, OrganizationDbContext dataContext) : base(logger, httpContext, dataContext)
        {
        }

        #endregion

        #region Public Methods

        public abstract string GetAuthUrl(string userState);
        
        public abstract Integration Authenticate(int projectId, string userState);

        public virtual Integration RefreshToken(int integrationId)
        {
            throw new NotImplementedException("Not supported by this network");
        }

        public virtual string GetAuthDoneUrl(bool isSuccessful)
        {
            return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/integrations/{Type.ToString().ToLower()}/done?successful={isSuccessful}";
        }

        #endregion

        #region Protected Methods

        protected virtual string GetCallbackUrl(string userState)
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