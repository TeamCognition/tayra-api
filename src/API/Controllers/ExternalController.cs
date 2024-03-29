﻿using System;
using System.Linq;
using Finbuckle.MultiTenant;
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

namespace Tayra.API.Controllers
{
    [AllowAnonymous]
    public class ExternalController : BaseController
    {
        #region Constructor

        public ExternalController(CatalogDbContext catalogDb, IServiceProvider serviceProvider, IConnectorResolver connectorResolver, IMailerService mailerService) : base(serviceProvider)
        {
            ConnectorResolver = connectorResolver;
            _catalogContext = catalogDb;
            MailerService = mailerService;
        }

        #endregion

        public IConnectorResolver ConnectorResolver { get; }

        private IMailerService MailerService { get; }
        private CatalogDbContext _catalogContext { get; }

        #region Public Methods

        [HttpGet, Route("callback/{type?}")]
        public IActionResult AuthenticateCallback([FromServices] CatalogDbContext catalogContext, IntegrationType type, [FromQuery] string state, [FromQuery] string setup_action, [FromQuery] string installation_id, [FromQuery] string error = null)
        {
            IOAuthConnector connector;
            if (setup_action == "update" && string.IsNullOrEmpty(state))
            {
                var ti = catalogContext.TenantIntegrations.Include(x => x.Tenant).FirstOrDefault(x => x.InstallationId == installation_id);
                Request.QueryString = Request.QueryString.Add("tenant", ti.Tenant.Identifier);
                connector = ConnectorResolver.Get<IOAuthConnector>(type);
                connector.UpdateAuthentication(installation_id);

                return Redirect($"https://{ti.Tenant.Identifier}/segments");
            }
            var oauthState = new OAuthState(state);
            
            Request.QueryString = Request.QueryString.Add("tenant", oauthState.TenantIdentifier);
            var tenant = catalogContext.TenantInfo.FirstOrDefault(x => x.Identifier == oauthState.TenantIdentifier);
            HttpContext.TrySetTenantInfo(tenant, false);
            
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
                MailerService.SendEmail("haris@tayra.io",
                    "New Try Submitted (Landing Page Try for free)",
                    JsonConvert.SerializeObject(dto));

                _catalogContext.LandingPageTry.Add(new LandingPageTry
                {
                    EmailAddress = dto.Email,
                });

                _catalogContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return Ok();
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
                MailerService.SendEmail("haris@tayra.io",
                    "New Company Signup",
                    JsonConvert.SerializeObject(jObject));

                MailerService.SendEmail("haris@tayra.io",
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