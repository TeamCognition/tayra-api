using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.Common
{
    public abstract class BaseOAuthConnector : BaseConnector, IOAuthConnector
    {
        #region Constructor

        protected BaseOAuthConnector(ILogger<BaseConnector> logger, IHttpContextAccessor httpContext, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config) : base(logger, httpContext, dataContext, catalogDbContext, config)
        {
        }

        protected BaseOAuthConnector(ILogger<BaseConnector> logger, OrganizationDbContext dataContext, CatalogDbContext catalogDbContext, IConfiguration config) : base(logger, dataContext, catalogDbContext, config)
        {
        }

        #endregion

        #region Public Methods

        public abstract string GetAuthUrl(OAuthState state);

        public abstract Integration Authenticate(OAuthState state);
        public abstract void UpdateAuthentication(string installationId);

        public virtual Integration RefreshToken(Guid integrationId)
        {
            throw new NotImplementedException("Not supported by this network");
        }

        public virtual string GetAuthDoneUrl(string returnPath, bool isSuccessful)
        {
            string protocol = TenantInfo.Identifier.StartsWith("localhost:", StringComparison.InvariantCulture) || TenantInfo.Identifier.EndsWith("local") ? "http" : "https";
            return $"{protocol}://{TenantInfo.Identifier}/{returnPath}/?i={Type.ToString().ToLower()}&success={isSuccessful}";
        }

        #endregion

        #region Protected Methods

        protected virtual string GetCallbackUrl(string userState)//userState is not used, but everything works
        {
            return $"https://{HttpContext.Request.Host}/external/callback/{Type.ToString().ToLower()}";
        }

        protected string ReadAccessToken(Guid integrationId)
        {
            return ReadField(integrationId, Constants.ACCESS_TOKEN, "Unable to access the integration account, access token is missing or has expired");
        }

        protected string ReadAccessTokenType(Guid integrationId)
        {
            return ReadField(integrationId, Constants.ACCESS_TOKEN_TYPE) ?? "Bearer";
        }

        #endregion
    }
}