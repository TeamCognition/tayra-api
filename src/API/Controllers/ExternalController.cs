using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.API.Helpers;
using Tayra.Common;
using Tayra.Connectors.Common;
using Tayra.Mailer;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;

namespace Tayra.API.Controllers
{
    [AllowAnonymous]
    public class ExternalController : BaseController
    {
        #region Constructor

        public ExternalController(CatalogDbContext catalogDb, IServiceProvider serviceProvider, IConnectorResolver connectorResolver) : base(serviceProvider)
        {
            ConnectorResolver = connectorResolver;
            _catalogContext = catalogDb;
        }

        #endregion

        public IConnectorResolver ConnectorResolver { get; }

        private CatalogDbContext _catalogContext { get; }

        #region Public Methods

        [HttpGet, Route("callback/{type?}")]
        public IActionResult AuthenticateCallback([FromServices] CatalogDbContext catalogContext, IntegrationType type, [FromQuery] string state, [FromQuery] string setup_action, [FromQuery] string installation_id, [FromQuery] string error = null)
        {
            IOAuthConnector connector;
            if (setup_action == "update" && string.IsNullOrEmpty(state))
            {
                var ti = catalogContext.TenantIntegrations.Include(x => x.Tenant).FirstOrDefault(x => x.InstallationId == installation_id);
                Request.QueryString = Request.QueryString.Add("tenant", ti.Tenant.Key);
                connector = ConnectorResolver.Get<IOAuthConnector>(type);
                connector.UpdateAuthentication(installation_id);

                return Redirect($"https://{ti.Tenant.Key}/segments");
            }
            var oauthState = new OAuthState(state);
            Request.QueryString = Request.QueryString.Add("tenant", oauthState.TenantKey);
            connector = ConnectorResolver.Get<IOAuthConnector>(type);

            if (!string.IsNullOrEmpty(error))
            {
                return Redirect(connector.GetAuthDoneUrl(oauthState.ReturnPath, false));
            }

            try
            {
                connector.Authenticate(oauthState);
            }
            catch
            {
                return Redirect(connector.GetAuthDoneUrl(oauthState.ReturnPath, false));
            }

            return Redirect(connector.GetAuthDoneUrl(oauthState.ReturnPath, true));
        }

        public class TryForFreeFormDTO
        {
            public string Email { get; set; }
        }

        [HttpPost, Route("tryForFree")]
        public IActionResult TryForFree([FromBody] TryForFreeFormDTO dto)
        {
            try
            {
                MailerService.SendEmail(dto.Email, new EmailTryForFreeDTO());

                _catalogContext.LandingPageTry.Add(new LandingPageTry
                {
                    EmailAddress = dto.Email,
                });

                _catalogContext.SaveChanges();

                MailerService.SendEmail("haris.botic96@gmail.com",
                    "haris@tayra.io",
                    "New Try Submitted (Landing Page Try for free)",
                    JsonConvert.SerializeObject(dto));

            }
            catch (Exception)
            {
                throw new Exception();
            }

            return Ok();
        }

        public class ContactFormDTO
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Message { get; set; }
        }

        [HttpPost("landingForm")]

        public IActionResult LandingForm([FromBody] JObject jObject)
        {
            string name = "unknown";
            string email = "unknown";
            string contact = "unknown";
            if (jObject.TryGetValue("name", out var nameToken)) name = nameToken.ToString();
            if (jObject.TryGetValue("email", out var emailToken)) name = emailToken.ToString();
            if (jObject.TryGetValue("contact", out var contactToken)) name = contactToken.ToString();

            try
            {
                MailerService.SendEmail("haris.botic96@gmail.com",
                    "haris@tayra.io",
                    "New Company Signup",
                    JsonConvert.SerializeObject(jObject));

                MailerService.SendEmail("haris.botic96@gmail.com",
                    "ejub@tayra.io",
                    "New Company Signup",
                    JsonConvert.SerializeObject(jObject));

                _catalogContext.LandingPageContacts.Add(new LandingPageContact
                {
                    Name = name,
                    EmailAddress = email,
                    PhoneNumber = contact,
                    Message = JsonConvert.SerializeObject(jObject)
                });

                _catalogContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return Ok();
        }

        #endregion
    }
}