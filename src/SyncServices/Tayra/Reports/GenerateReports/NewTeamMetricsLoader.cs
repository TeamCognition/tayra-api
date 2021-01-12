using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Tayra.Analytics;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class NewTeamMetricsLoader : BaseLoader
    {
        #region Constructor

        public NewTeamMetricsLoader(LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
        }

        #endregion

        #region Public Methods
        public override void Execute(DateTime date, JObject requestBody, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Identifier);
                using (var organizationDb = new OrganizationDbContext(TenantModel.WithConnectionStringOnly(tenant.ConnectionString), null))
                {
                    GenerateTeamMetrics(organizationDb, date, LogService);
                }
            }
        }

        #endregion

        #region Private Methods

        public static List<TeamMetric> GenerateTeamMetrics(OrganizationDbContext organizationDb, DateTime fromDay,
            LogService logService)
        {
            var metricsToInsert = new List<TeamMetric>();

            var dateId = DateHelper2.ToDateId(fromDay);

            var teams = organizationDb.Teams.Select(x => new { x.Id, x.SegmentId }).ToArray();

            foreach (var team in teams)
            {
                var profileIds = organizationDb.ProfileAssignments
                    .Where(
                        x => x.TeamId.Value == team.Id &&
                             x.Profile.IsAnalyticsEnabled /*&& x.Created <= DateHelper2.ParseDate(dateId)*/)
                    .Select(x => x.ProfileId)
                    .Distinct()
                    .ToArray();

                var rawMetrics = organizationDb.ProfileMetrics
                    .Where(x => x.DateId == dateId && profileIds.Contains(x.ProfileId))
                    .Where(x => x.SegmentId == null || x.SegmentId == team.SegmentId)
                    .Select(x => new MetricShardWEntity
                    {
                        EntityId = x.ProfileId,
                        Type = x.Type,
                        Value = x.Value,
                        DateId = x.DateId
                    })
                    .ToArray();

                var teamMetrics = MetricType.List
                        .Select(m => new TeamMetric(team.Id, dateId, m, profileIds.Sum(x =>
                                m.Calc(rawMetrics.Where(m => m.EntityId == x).ToArray(),
                                new DatePeriod(dateId, dateId)) / profileIds.Length)));



                metricsToInsert.AddRange(teamMetrics);
            }

            var existing = organizationDb.TeamMetrics.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<TeamMetric>($"date: ${dateId},  deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlInterpolated($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId}");
                //organizationDb.Database.ExecuteSqlCommand($"delete from TeamMetrics where {nameof(TeamMetric.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.TeamMetrics.AddRange(metricsToInsert);

            organizationDb.SaveChanges();

            logService.Log<NewTeamMetricsLoader>($"date: ${dateId}, {metricsToInsert.Count} new team metrics saved to database.");
            return metricsToInsert;
        }

        #endregion
    }
}
