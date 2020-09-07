using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices.Common;
using Tayra.SyncServices.Metrics;

namespace Tayra.SyncServices.Tayra
{
    public class NewSegmentMetricsLoader : BaseLoader
    {
        #region Private Variables

        private readonly IShardMapProvider _shardMapProvider;

        #endregion

        #region Constructor

        public NewSegmentMetricsLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {
            _shardMapProvider = shardMapProvider;
        }

        #endregion

        #region Public Methods
        public override void Execute(DateTime date, JObject requestBody, params Tenant[] tenants)
        {
            foreach (var tenant in tenants)
            {
                LogService.SetOrganizationId(tenant.Key);
                using (var organizationDb = new OrganizationDbContext(null, new ShardTenantProvider(tenant.Key), _shardMapProvider))
                {
                    GenerateSegmentMetrics(organizationDb, date, LogService);
                }
            }
        }

        #endregion

        #region Private Methods

        public static List<SegmentMetric> GenerateSegmentMetrics(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            var metricsToInsert = new List<SegmentMetric>();

            var companyTokenId = organizationDb.Tokens.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Id).FirstOrDefault();
            if (companyTokenId == 0)
                throw new ApplicationException("COMPANY TOKEN NOT FOUND");

            var dateId = DateHelper2.ToDateId(fromDay);

            var segmentIds = organizationDb.Segments.Select(x => x.Id).ToArray();

            foreach (var segmentId in segmentIds)
            {
                var profileIds = organizationDb.ProfileAssignments
                    .Where(x => x.SegmentId == segmentId && x.Profile.Created.Date <= DateHelper2.ParseDate(dateId))
                    .Select(x => x.ProfileId)
                    .ToArray()
                    .Distinct();

                metricsToInsert.AddRange(organizationDb.ProfileMetrics
                    .Where(x => x.DateId == dateId && profileIds.Contains(x.ProfileId))
                    .Select(x => new SegmentMetric(segmentId, x)));
            }

            var existing = organizationDb.ProfileMetrics.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<ProfileReportDaily>($"deleting {existing} records from database");
                //organizationDb.Database.ExecuteSqlInterpolated($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId} AND {nameof(ProfileReportDaily.SegmentId)} = {segmentId}");
                organizationDb.Database.ExecuteSqlCommand($"delete from SegmentMetrics where {nameof(ProfileReportDaily.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.SegmentMetrics.AddRange(metricsToInsert);

            organizationDb.SaveChanges();

            logService.Log<NewSegmentMetricsLoader>($"{metricsToInsert.Count} new profile metrics saved to database.");
            return metricsToInsert;
        }

        #endregion
    }
}
