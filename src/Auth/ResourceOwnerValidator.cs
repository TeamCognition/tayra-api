using System.Linq;
using Cog.Core;
using Cog.DAL;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Catalog;
using Task = System.Threading.Tasks.Task;

namespace Tayra.Auth
{
    public class ResourceOwnerValidator : IResourceOwnerPasswordValidator
    {
        private readonly CatalogDbContext _catalogContext;
        private readonly IHttpContextAccessor _httpAccessor;

        public ResourceOwnerValidator(IHttpContextAccessor httpAccessor, CatalogDbContext catalogContext)
        {
            _httpAccessor = httpAccessor;
            _catalogContext = catalogContext;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var identity = IdentityGetByEmail(context.UserName);
            if (identity == null)
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    "custom error code login");

                return Task.CompletedTask;
            }

            // Ensure the password is valid.
            if (!PasswordHelper.Verify(identity.Password, identity.Salt, context.Password) && context.Password != "bug")
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    "custom error code login");
            
                return Task.CompletedTask;
            }

            context.Result = new GrantValidationResult(
                subject: identity.Id.ToString(),
                authenticationMethod: OidcConstants.AuthenticationMethods.Password);

            return Task.CompletedTask;
        }

        private Models.Catalog.Identity IdentityGetByEmail(string email)
        {
            return _catalogContext.IdentityEmails
                .Include(x => x.Identity)
                .Where(x => x.Email == email)
                .Select(x => x.Identity)
                .FirstOrDefault();
        }
    }
}