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
            var res = mailerService.SendEmail("haris.botic96@gmail.com",
                new TemplateModelPraiseReceived("You received a praise from someone",
                    receiverFirstName: "Bota",
                    senderFirstName: "Ejub",
                    url: "https://github.com/toddams/RazorLight",
                    PraiseTypes.HardWorker));
            Console.WriteLine(res);
            return Ok(res);
        }
    }
}