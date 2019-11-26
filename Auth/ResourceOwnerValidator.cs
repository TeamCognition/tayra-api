using System.Threading.Tasks;
using Firdaws.Core;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Tayra.Models.Organizations;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.Auth
{
    public class ResourceOwnerValidator : IResourceOwnerPasswordValidator
    {
        private readonly IIdentitiesService _identitiesService;

        public ResourceOwnerValidator(IIdentitiesService identitiesService)
        {
            _identitiesService = identitiesService;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var identity = _identitiesService.GetByEmail(context.UserName);
            if (identity == null)
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    "custom error code login");

                return Task.CompletedTask;
            }

            // Ensure the password is valid.
            if (!PasswordHelper.Verify(identity.Password, identity.Salt, context.Password))
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    "custom error code login");

                return Task.CompletedTask;
            }

            context.Result = new GrantValidationResult(
                subject: identity.Username,
                authenticationMethod: OidcConstants.AuthenticationMethods.Password);

            return Task.CompletedTask;
        }
    }
}