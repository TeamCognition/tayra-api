﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tayra.Connectors.App.Helpers;
using Tayra.Connectors.Atlassian.Jira;
using Tayra.Connectors.Common;
using Tayra.Models.Organizations;
using Tayra.Common;

namespace Tayra.Connectors.App.Controllers
{
    public class ResultsController : Controller
    {
        public ResultsController(IWebHostEnvironment environment, IConnectorResolver connectorResolver)
        {
            Environment = environment;
            ConnectorResolver = connectorResolver;
        }

        public IWebHostEnvironment Environment { get; set; }

        public IConnectorResolver ConnectorResolver { get; }

        public IActionResult ATJ()
        {
            var connector = (AtlassianJiraConnector)ConnectorResolver.Get<IOAuthConnector>(IntegrationType.ATJ);
            var model = JsonConvert.DeserializeObject<Integration>(TempData["Account"] as string);
            try
            {
                ViewBag.RefreshToken = connector.RefreshToken(model.Id);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return View(model);
        }

//        public IActionResult GH()
//        {
//            var model = JsonConvert.DeserializeObject<Integration>(TempData["Account"] as string);
//            return View(model);
//        }
    }
}