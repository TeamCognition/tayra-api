﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tayra.Models.Catalog;
using Tayra.SyncServices.Tayra;

namespace Tayra.SyncServices.Common
{
    public static class SyncHelper
    {
        #region Public Methods

        public static void RunFromHttp(JobTypes jobTypes, HttpRequest request, ExecutionContext context, ILogger logger)
        {
            var loader = GetLoader(jobTypes, context, logger);

            var requestBody = new StreamReader(request.Body).ReadToEnd();
            var dto = string.IsNullOrEmpty(requestBody) ? new SyncRequest() : JsonConvert.DeserializeObject<SyncRequest>(requestBody);

            if (!string.IsNullOrEmpty(dto.OrganizationKey))
            {
                var date = dto.Date.HasValue ? dto.Date.Value.Date : DateTime.UtcNow.Date.Subtract(TimeSpan.FromDays(1));
                loader.Execute(date, dto.OrganizationKey);
            }
            else
            {
                var timezoneInfo = dto.Date.HasValue && string.IsNullOrEmpty(dto.TimezoneId)
                    ? new List<TimeZoneDTO> { new TimeZoneDTO { Date = dto.Date.Value, Id = dto.TimezoneId } }
                    : GetCurrentTimezones();

                loader.Execute(timezoneInfo.First().Date.Date, timezoneInfo.ToArray());
            }
        }

        public static void RunFromSchedule(JobTypes jobTypes, TimerInfo timerInfo, ExecutionContext context, ILogger logger)
        {
            var loader = GetLoader(jobTypes, context, logger);

            var timezoneInfo = GetCurrentTimezones();

            loader.Execute(timezoneInfo.First().Date.Date, timezoneInfo.ToArray());
        }

        public static IConfigurationRoot LoadSettings(ExecutionContext context)
        {
            return new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public static BaseLoader GetLoader(JobTypes jobTypes, ExecutionContext context, ILogger logger)
        {
            var config = LoadSettings(context);
            //var logger = CreateApplicationInsightsLogger($"{serviceType}Loader", config["ApplicationInsights:InstrumentationKey"]);
            var coreDatabase = new CatalogDbContext(config["ConnectionStrings:core"]);
            var logService = new LogService(logger);

            switch (jobTypes)
            {
                case JobTypes.GenerateReports:
                    return new GenerateReportsLoader(config, logService, coreDatabase);

                case JobTypes.GenerateReportProfile:
                    return new GenerateProfileReportsLoader(config, logService, coreDatabase);

                case JobTypes.GenerateReportProject:
                    return new GenerateProjectReportsLoader(config, logService, coreDatabase);

                case JobTypes.GenerateReportTeam:
                    return new GenerateTeamReportsLoader(config, logService, coreDatabase);

                case JobTypes.SyncCompetitions:
                    return new SyncCompetitionsLoader(config, logService, coreDatabase);
            }

            throw new NotSupportedException($"{jobTypes} integration are not supported");
        }

        #endregion

        #region Private Methods

        //private static ApplicationInsightsLogger CreateApplicationInsightsLogger(string categoryName, string instrumentationKey)
        //{
        //    var appTelemetryConfiguration = new TelemetryConfiguration(instrumentationKey);

        //    return new ApplicationInsightsLogger(categoryName, new TelemetryClient(appTelemetryConfiguration), new ApplicationInsightsLoggerOptions());
        //}

        private static List<TimeZoneDTO> GetCurrentTimezones()
        {
            var utcTime = DateTime.UtcNow;
            return TimeZoneInfo.GetSystemTimeZones()
                .Select(x => new { Zone = x, Date = TimeZoneInfo.ConvertTimeFromUtc(utcTime, x) })
                .Where(x => x.Date.TimeOfDay.TotalHours > 0 && x.Date.TimeOfDay.TotalHours <= 1)
                .Select(x => new TimeZoneDTO { Id = x.Zone.Id, Date = x.Date.Subtract(TimeSpan.FromDays(1)) })
                .ToList();
        }

        #endregion
    }
}
