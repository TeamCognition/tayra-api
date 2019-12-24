using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public ExternalController(CatalogDbContext catalogDb, IServiceProvider serviceProvider, IConnectorResolver connectorResolver, OrganizationDbContext db) : base(serviceProvider)
        {
            ConnectorResolver = connectorResolver;
            _db = db;
            _catalogContext = catalogDb;
        }

        #endregion

        public IConnectorResolver ConnectorResolver { get; }

        private OrganizationDbContext _db;

        private CatalogDbContext _catalogContext { get; }

        #region Public Methods

        [HttpGet, Route("callback/{type?}")]
        public IActionResult AuthenticateCallback(IntegrationType type, [FromQuery]string state)
        {
            var connector = ConnectorResolver.Get<IOAuthConnector>(type);
            try
            {
                var stateData = Cipher.Decrypt(state).Split('|');

                var profileId = int.Parse(stateData[0]);
                var profileRole = Enum.Parse<ProfileRoles>(stateData[1]);
                var segmentId = int.Parse(stateData[2]);
                connector.Authenticate(profileId, profileRole, segmentId, state);
            }
            catch
            {
                return Redirect(connector.GetAuthDoneUrl(false));
            }

            return Redirect(connector.GetAuthDoneUrl(true));
        }

        public class ContactFormDTO
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Message { get; set; }
        }

        [HttpPost, Route("contactUs")]
        public IActionResult ContactUs([FromBody] ContactFormDTO dto)
        {
            try
            {
                MailerService.SendEmail("haris.botic96@gmail.com",
                            "haris@tayra.io",
                            "New Contact (Landing Page Contact Form)",
                            JsonConvert.SerializeObject(dto));

                _catalogContext.LandingPageContacts.Add(new LandingPageContact
                {
                    Name = dto.Name,
                    EmailAddresss = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Message = dto.Message
                });

                _catalogContext.SaveChanges();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }

        #endregion
    }
}