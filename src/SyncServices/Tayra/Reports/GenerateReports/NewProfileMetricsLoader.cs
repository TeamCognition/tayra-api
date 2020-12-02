using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json.Linq;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;
using Tayra.Models.Organizations.Metrics.PureMetrics;
using Tayra.SyncServices.Common;
using Tayra.SyncServices.Metrics;

namespace Tayra.SyncServices.Tayra
{
    public class NewProfileMetricsLoader : BaseLoader
    {
        #region Private Variables

        private readonly IShardMapProvider _shardMapProvider;

        #endregion

        #region Constructor

        public NewProfileMetricsLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
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
                    GenerateProfileMetrics(organizationDb, date, LogService);
                }
            }
        }


        #endregion

        #region Private Methods

        public static List<ProfileMetric> GenerateProfileMetrics(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            var metricsToInsert = new List<ProfileMetric>();

            var companyTokenId = organizationDb.Tokens.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Id).FirstOrDefault();
            if (companyTokenId == 0)
                throw new ApplicationException("COMPANY TOKEN NOT FOUND");

            var dateId = DateHelper2.ToDateId(fromDay);

            var profiles = organizationDb.Profiles.Select(x => new { Id = x.Id }).ToArray();
            var profileIds = profiles.Select(x => x.Id);

            var tasks = (from t in organizationDb.Tasks
                         where t.Status == TaskStatuses.Done
                         where t.SegmentId.HasValue
                         where t.LastModifiedDateId == dateId
                         select t).ToArray();

            var tokens = (from tt in organizationDb.TokenTransactions
                          where profileIds.Contains(tt.ProfileId)
                          where tt.TokenId == companyTokenId
                          where tt.DateId == dateId
                          select tt).ToArray();

            var praises = (from u in organizationDb.ProfilePraises
                           where profileIds.Contains(u.PraiserProfileId)
                           where u.DateId == dateId
                           select u).ToArray();

            var itemsCreated = (from i in organizationDb.Items
                                where profileIds.Contains(i.CreatedBy)
                                where i.CreatedDateId == dateId
                                group i by i.CreatedBy into g
                                select new
                                {
                                    ProfileId = g.Key,
                                    Count = g.Count(),
                                }).ToList();


            var itemsDissed = (from i in organizationDb.ItemDisenchants
                               where profileIds.Contains(i.ProfileId)
                               where i.DateId == dateId
                               select i).ToArray();

            var giftsSent = (from u in organizationDb.ItemGifts
                             where profileIds.Contains(u.SenderId)
                             where u.DateId == dateId
                             select u).ToArray();

            var giftsReceived = (from u in organizationDb.ItemGifts
                                 where profileIds.Contains(u.ReceiverId)
                                 where u.DateId == dateId
                                 select u).ToArray();

            var shopPurchases = (from sp in organizationDb.ShopPurchases
                                 where profileIds.Contains(sp.ProfileId)
                                 where sp.Status == ShopPurchaseStatuses.Fulfilled
                                 where sp.LastModifiedDateId == dateId
                                 select sp).ToArray();

            var inventory = (from pinv in organizationDb.ProfileInventoryItems.Include(x => x.Item)
                             where profileIds.Contains(pinv.ProfileId)
                             where pinv.Created.Date == fromDay.Date
                             select pinv).ToArray();

            var gitCommitsToday = (from gc in organizationDb.GitCommits
                                   where profileIds.Contains(gc.AuthorProfileId.Value)
                                   where gc.Created.Date == fromDay.Date
                                   select gc).ToArray();

            var gitCommentsPerPr = (from gc in organizationDb.PullRequestReviewComments.Include(p => p.PullRequest)
                                    where profileIds.Contains(gc.PullRequest.AuthorProfileId.Value)
                                    where gc.Created.Date == fromDay.Date
                                    select gc).ToArray();
            
            var gitPullRequestsCreated = (from gp in organizationDb.PullRequests
                                    where profileIds.Contains(gp.AuthorProfileId.Value)
                                    where gp.Created.Date == fromDay.Date
                                    select gp).ToArray();
            
            var gitPullRequestsReviewed = (from gp in organizationDb.PullRequests
                                    join r in organizationDb.PullRequestReviews on gp.Id equals r.PullRequestId 
                                    where profileIds.Contains(gp.AuthorProfileId.Value)
                                    where gp.Created.Date == fromDay.Date
                                    select gp).ToArray();
            var gitPullRequestsCycle = (from pc in organizationDb.PullRequests
                                    where profileIds.Contains(pc.AuthorProfileId.Value)
                                    where pc.MergedAt.Value.Date == fromDay.Date
                                    select pc).ToArray();

            var existing = organizationDb.ProfileMetrics.Count(x => x.DateId == dateId);
            foreach (var p in profiles)
            {
                var ts = tasks.Where(x => x.AssigneeProfileId == p.Id);
                var tt = tokens.Where(x => x.ProfileId == p.Id);
                var sp = shopPurchases.Where(x => x.ProfileId == p.Id);
                var iCreated = itemsCreated.FirstOrDefault(x => x.ProfileId == p.Id);
                var iDissed = itemsDissed.Where(x => x.ProfileId == p.Id);
                var iGiftS = giftsSent.Where(x => x.SenderId == p.Id);
                var iGiftR = giftsReceived.Where(x => x.ReceiverId == p.Id);
                var inv = inventory.Where(x => x.ProfileId == p.Id);
                var commits = gitCommitsToday.Where(x => x.AuthorProfileId == p.Id);
                var prCreated = gitPullRequestsCreated.Where(x => x.AuthorProfileId == p.Id);
                var prReviewed = gitPullRequestsReviewed.Where(x => x.AuthorProfileId == p.Id);
                var prReviewComments = gitCommentsPerPr.Where(x => x.PullRequest.AuthorProfileId == p.Id);
                var pulRequestCycle = gitPullRequestsCycle.Where(x => x.AuthorProfileId == p.Id);
                
                metricsToInsert.Add(new ProfileMetric(p.Id, ((PraisesReceivedMetric) MetricType.PraisesReceived).Create(praises, p.Id, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((PraisesGivenMetric)MetricType.PraisesGiven).Create(praises, p.Id, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((TokensEarnedMetric)MetricType.TokensEarned).Create(tt, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((TokensSpentMetric)MetricType.TokensSpent).Create(tt, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((InventoryValueChangeMetric)MetricType.InventoryValueChange).Create(inv, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((ItemsBoughtMetric)MetricType.ItemsBought).Create(sp, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((GiftsSentMetric)MetricType.GiftsSent).Create(iGiftS, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((GiftsReceivedMetric)MetricType.GiftsReceived).Create(iGiftR, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((ItemsDisenchantedMetric)MetricType.ItemsDisenchanted).Create(iDissed, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((CommitsMetric)MetricType.Commits).Create(commits, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((PullRequestsCreatedMetric)MetricType.PullRequestsCreated).Create(prCreated, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((PullRequestsReviewedMetric)MetricType.PullRequestsReviewed).Create(prReviewed, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((CommentsPerPrMetric)MetricType.CommentsPerPr).Create(prReviewComments, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((PullRequestCycleMetric)MetricType.PullRequestCycle).Create(pulRequestCycle, dateId)));
                metricsToInsert.Add(new ProfileMetric(p.Id, ((PullRequestSizeMetric)MetricType.PullRequestSize).Create(new MetricService(organizationDb), prCreated, dateId)));
                
                metricsToInsert.AddRange(ProfileMetric.CreateRange(p.Id, ((WorkUnitsCompletedMetric)MetricType.TasksCompleted).CreateForEverySegment(ts, dateId)));
                metricsToInsert.AddRange(ProfileMetric.CreateRange(p.Id, ((EffortMetric)MetricType.Effort).CreateForEverySegment(ts, dateId)));
                metricsToInsert.AddRange(ProfileMetric.CreateRange(p.Id, ((ComplexityMetric)MetricType.Complexity).CreateForEverySegment(ts, dateId)));
                //metricsToInsert.AddRange(ProfileMetric.CreateRange(p.Id, ErrorsMetric.CreateForEverySegment(ts, dateId)));
                //metricsToInsert.AddRange(ProfileMetric.CreateRange(p.Id, SavesMetric.CreateForEverySegment(ts, dateId)));
                metricsToInsert.AddRange(ProfileMetric.CreateRange(p.Id, ((TimeWorkedMetric)MetricType.TimeWorked).CreateForEverySegment(ts, dateId)));
                metricsToInsert.AddRange(ProfileMetric.CreateRange(p.Id, ((TimeWorkedLoggedMetric)MetricType.TimeWorkedLogged).CreateForEverySegment(ts, dateId)));
            }

            if (existing > 0)
            {
                logService.Log<ProfileReportDaily>($"deleting {existing} records from database");
                //organizationDb.Database.ExecuteSqlInterpolated($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId} AND {nameof(ProfileReportDaily.SegmentId)} = {segmentId}");
                organizationDb.Database.ExecuteSqlCommand($"delete from ProfileMetrics where {nameof(ProfileReportDaily.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.ProfileMetrics.AddRange(metricsToInsert);

            organizationDb.SaveChanges();

            logService.Log<NewProfileMetricsLoader>($"{metricsToInsert.Count} new profile metrics saved to database.");
            return metricsToInsert;
        }

        #endregion
    }
}
