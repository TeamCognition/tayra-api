using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Mailer.Templates.PraiseReceived;

namespace Tayra.API.Features.TestMailer
{
    [Route("[controller]")]
    [ApiController]
    public class TestMailerController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public ActionResult TestMailer([FromServices] IMailerService mailerService)
        {
            var res = mailerService.SendEmail("ejub@tayra.io",
                new TemplateModelPraiseReceived("You received a praise from someone",
                    receiverName: "Bota",
                    senderName: "Ejub",
                    url: "https://github.com/toddams/RazorLight",
                    PraiseTypes.HardWorker));
            return Ok(res);
        }
    }
}