using System;
using System.Threading.Tasks;
using Cog.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public IActionResult AuthenticateCallback(IntegrationType type, [FromQuery]string state)
        {
            var stateData = Cipher.Decrypt(state.Base64UrlDecode()).Split('|');
            Request.QueryString = Request.QueryString.Add("tenant", stateData[0]);
            var connector = ConnectorResolver.Get<IOAuthConnector>(type);
            try
            {
                connector.Authenticate(
                    profileId: int.Parse(stateData[1]),
                    profileRole: Enum.Parse<ProfileRoles>(stateData[2]),
                    segmentId: int.Parse(stateData[3]),
                    userState: state);
            }
            catch
            {
                return Redirect(connector.GetAuthDoneUrl(stateData[4], false));
            }

            return Redirect(connector.GetAuthDoneUrl(stateData[4], true));
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

        [HttpPost, Route("contactUs")]
        public ActionResult ContactUs([FromBody] ContactFormDTO dto)
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
                    EmailAddress = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Message = dto.Message
                });

                _catalogContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return Ok();
        }

        public class CompanySignupDTO
        {
            public string Name { get; set; }
            public string Location { get; set; }
            public string ContactPerson { get; set; }
            public string PhoneNumber { get; set; }
            public string EmailAddress { get; set; }
            public string Industry { get; set; }
            public int EmployeesCount { get; set; }
            public string Website { get; set; }
        }

        [HttpPost, Route("companySignup")]
        public IActionResult CompanySignup([FromBody] CompanySignupDTO dto)
        {
            try
            {
                MailerService.SendEmail("haris.botic96@gmail.com",
                            "haris@tayra.io",
                            "New Company Signup",
                            JsonConvert.SerializeObject(dto));

                _catalogContext.LandingPageContacts.Add(new LandingPageContact
                {
                    Name = dto.Name,
                    EmailAddress = dto.EmailAddress,
                    PhoneNumber = dto.PhoneNumber,
                    Message = JsonConvert.SerializeObject(dto)
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