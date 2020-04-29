using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tayra.Common;
using Tayra.Connectors.App.Helpers;
using Tayra.Connectors.Common;
using Tayra.Models.Organizations;

namespace Tayra.Connectors.App.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IConnectorResolver connectorResolver, OrganizationDbContext dataContext)
        {
            ConnectorResolver = connectorResolver;
            DataContext = dataContext;
        }

        public IConnectorResolver ConnectorResolver { get; }

        public OrganizationDbContext DataContext { get; set; }

        [HttpGet]
        public IActionResult Index(string type)
        {
            return View();
        }

        [HttpGet, Route("connect/{type?}")]
        public IActionResult Connect(IntegrationType type)
        {
            var connector = ConnectorResolver.Get<IOAuthConnector>(type);
            return Redirect(connector.GetAuthUrl("test"));
        }

        [HttpGet, Route("external/callback/{type?}")]
        public IActionResult Callback(IntegrationType type, [FromQuery]string state)
        {
            try
            {
                var connector = ConnectorResolver.Get<IOAuthConnector>(type);
                var account = connector.Authenticate(1, ProfileRoles.Admin, 1, "test");

                TempData["Account"] = JsonConvert.SerializeObject(account, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return RedirectToAction(type.ToString(), "Results");
            }
            catch (ApplicationException e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("Error");
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Something went wrong: {e.Message}";
                return RedirectToAction("Error");
            }
        }

        [HttpGet, Route("refresh/{type}/{id}")]
        public IActionResult Refresh([FromRoute]IntegrationType type, [FromRoute]int id)
        {
            try
            {
                var connector = ConnectorResolver.Get<IOAuthConnector>(type);
                if (connector == null)
                {
                    throw new ApplicationException("Not Supported");
                }

                var account = connector.RefreshToken(id);
                var newToken = account.Fields.FirstOrDefault(x => x.Key == Constants.ACCESS_TOKEN)?.Value;
                return Content($"New Access Token is {newToken}");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [HttpGet]
        public IActionResult Done()
        {
            return View(JsonConvert.DeserializeObject<Integration>(TempData["Account"] as string));
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(TempData["Error"]);
        }
    }
}
