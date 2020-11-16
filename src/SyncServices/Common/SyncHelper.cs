using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.DAL;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices.Tayra;

namespace Tayra.SyncServices.Common
{
    public static class SyncHelper
    {
        #region Public Methods

        public static void RunFromHttp(JobTypes jobTypes, HttpRequest request, ExecutionContext context, ILogger logger)
        {
            var loader = GetLoader(jobTypes, context, logger);
            var requestBody = new StreamReader(request.Body).ReadToEndAsync().Result;
            var dto = string.IsNullOrEmpty(requestBody) ? new SyncRequest() : JsonConvert.DeserializeObject<SyncRequest>(requestBody);
            if (request.Query.TryGetValue("tenant", out StringValues tenantKey))
            {
                dto.TenantKey = tenantKey;
            }

            var jObject = string.IsNullOrEmpty(requestBody) ? new JObject() : JObject.Parse(requestBody);
            if (!string.IsNullOrEmpty(dto.TenantKey))
            {
                var date = dto.Date.HasValue ? dto.Date.Value.Date : DateTime.UtcNow.Date.Subtract(TimeSpan.FromDays(1));
                loader.Execute(date, dto.TenantKey, jObject);
            }
            else
            {
                var timezoneInfo = dto.Date.HasValue && string.IsNullOrEmpty(dto.TimezoneId)
                    ? new List<TimeZoneDTO> { new TimeZoneDTO { Date = dto.Date.Value, Id = dto.TimezoneId } }
                    : GetCurrentTimezones();

                loader.Execute(timezoneInfo.First().Date.Date, jObject, timezoneInfo.ToArray());
            }
        }

        public static void RunFromSchedule(JobTypes jobTypes, TimerInfo timerInfo, ExecutionContext context, ILogger logger)
        {
            var loader = GetLoader(jobTypes, context, logger);
            
            var timezoneInfo = GetCurrentTimezones();
            
            loader.Execute(timezoneInfo.First().Date.Date, null, timezoneInfo.ToArray());
        }

        public static IConfigurationRoot LoadSettings(ExecutionContext context)
        {
            return new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("sharedSettings.json", optional: true)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public static BaseLoader GetLoader(JobTypes jobTypes, ExecutionContext context, ILogger logger)
        {
            var config = LoadSettings(context);
            //var logger = CreateApplicationInsightsLogger($"{serviceType}Loader", config["ApplicationInsights:InstrumentationKey"]);
            var shardMapProvider = new ShardMapProvider(config);
            var logService = new LogService(logger);
            var coreDatabase = new CatalogDbContext(ConnectionStringUtilities.GetCatalogDbConnStr(config));

            switch (jobTypes)
            {
                case JobTypes.GenerateReports:
                    return new GenerateReportsLoader(shardMapProvider, logService, coreDatabase);

                case JobTypes.GenerateReportProfile:
                    return new GenerateProfileReportsLoader(shardMapProvider, logService, coreDatabase);

                case JobTypes.GenerateReportSegment:
                    return new GenerateSegmentReportsLoader(shardMapProvider, logService, coreDatabase);

                case JobTypes.GenerateReportTeam:
                    return new GenerateTeamReportsLoader(shardMapProvider, logService, coreDatabase);

                case JobTypes.SyncCompetitions:
                    return new SyncCompetitionsLoader(shardMapProvider, logService, coreDatabase);

                case JobTypes.MakeActionPoints:
                    return new MakeActionPointsLoader(shardMapProvider, logService, coreDatabase);

                case JobTypes.SyncIssues:
                    return new SyncIssuesLoader(shardMapProvider, logService, coreDatabase);

                case JobTypes.WebHookATJIssueUpdate:
                    return new ATJIssueUpdateLoader(shardMapProvider, logService, coreDatabase);
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
