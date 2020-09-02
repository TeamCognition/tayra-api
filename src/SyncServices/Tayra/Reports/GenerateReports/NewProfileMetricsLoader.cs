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
using Tayra.SyncServices.Common;

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
                    GenerateProfileReportsDaily(organizationDb, date, LogService);
                }
            }
        }


        #endregion

        #region Private Methods

        public static List<ProfileMetric> GenerateProfileReportsDaily(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, params int[] segmentIds)
        {
            var metricsToInsert = new List<ProfileMetric>();

            if(segmentIds.Length > 0)
            {
                if(!organizationDb.Segments.Any(x => segmentIds.Contains(x.Id)))
                {
                    throw new ApplicationException("there is an ID in segmentIds that doesn't exists");
                }
            }
            else
            {
                segmentIds = organizationDb.Segments.Where(x => x.IsReportingUnlocked).Select(x => x.Id).ToArray();
            }

            var companyTokenId = organizationDb.Tokens.Where(x => x.Type == TokenType.CompanyToken).Select(x => x.Id).FirstOrDefault();
            if (companyTokenId == 0)
                throw new ApplicationException("COMPANY TOKEN NOT FOUND");

            var dateId = DateHelper2.ToDateId(fromDay);
            var fourWeekPeriod = DateHelper.FindPeriod(DateRanges.Last28Days);
            foreach (var segmentId in segmentIds)
            {
                var profiles = organizationDb.ProfileAssignments.Where(x => x.SegmentId == segmentId).Select(x => new { Id = x.ProfileId, x.Profile.Role }).DistinctBy(x => x.Id).ToArray();
                var profileIds = profiles.Select(x => x.Id);

                var tasks = (from t in organizationDb.Tasks
                             where t.SegmentId == segmentId
                             where t.Status == TaskStatuses.Done
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
                                    group i by i.CreatedBy into total
                                    let change = total.Where(x => x.CreatedDateId == dateId)
                                    select new
                                    {
                                        ProfileId = total.Key,
                                        Count = change.Count(),
                                        CountTotal = total.Count()
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

                var quests = (from qc in organizationDb.QuestCompletions
                    where profileIds.Contains(qc.ProfileId)
                    group qc by qc.ProfileId
                    into total
                    let change = total.Where(x => x.Created.Date == fromDay.Date)
                    select new
                    {
                        ProfileId = total.Key,

                        Count = change.Count(),
                        CountTotal = total.Count()
                    }).ToList();
                
                var gitCommitsToday = (from gc in organizationDb.GitCommits
                    where profileIds.Contains(gc.AuthorProfileId.Value)
                    where gc.Created.Date == fromDay.Date
                    select gc).ToArray();
                
                foreach (var p in profiles)
                {
                    var ts = tasks.Where(x => x.AssigneeProfileId == p.Id);
                    var t = tokens.Where(x => x.ProfileId == p.Id);
                    var sp = shopPurchases.Where(x => x.ProfileId == p.Id);
                    var iCreated = itemsCreated.FirstOrDefault(x => x.ProfileId == p.Id);
                    var iDissed = itemsDissed.Where(x => x.ProfileId == p.Id);
                    var iGiftS = giftsSent.Where(x => x.SenderId == p.Id);
                    var iGiftR = giftsReceived.Where(x => x.ReceiverId == p.Id);
                    var inv = inventory.Where(x => x.ProfileId == p.Id);
                    var q = quests.FirstOrDefault(x => x.ProfileId == p.Id);
                    var commits = gitCommitsToday.Where(x => x.AuthorProfileId == p.Id);

                    var prm = new PraisesReceivedMetric(praises, p.Id, dateId);
                    
                    metricsToInsert.Add(new ProfileMetric(p.Id, prm));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new AssistMetric(prm)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new PraisesGivenMetric(praises, p.Id, dateId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new EffortMetric(ts, dateId, segmentId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new ComplexityMetric(ts, dateId, segmentId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new ErrorsMetric(ts, dateId, segmentId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new SavesMetric(ts, dateId, segmentId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new TimeWorkedMetric(ts, dateId, segmentId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new TimeWorkedLoggedMetric(ts, dateId, segmentId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new WorkUnitsCompletedMetric(ts, dateId, segmentId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new TokensEarnedMetric(t, dateId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new TokensSpentMetric(t, dateId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new ItemsInInventoryMetric(inv, dateId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new InventoryValueMetric(inv, dateId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new ItemsBoughtMetric(sp, dateId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new GiftsSentMetric(iGiftS, dateId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new GiftsReceivedMetric(iGiftR, dateId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new ItemsDisenchantedMetric(iDissed, dateId)));
                    metricsToInsert.Add(new ProfileMetric(p.Id, new CommitsMetric(commits, dateId)));
                }

                var existing = organizationDb.ProfileMetrics.Count(x => x.DateId == dateId && x.SegmentId == segmentId);
                if (existing > 0)
                {
                    logService.Log<ProfileReportDaily>($"deleting {existing} records from database");
                    //organizationDb.Database.ExecuteSqlInterpolated($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId} AND {nameof(ProfileReportDaily.SegmentId)} = {segmentId}");
                    organizationDb.Database.ExecuteSqlCommand($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId} AND {nameof(ProfileReportDaily.SegmentId)} = {segmentId}", dateId); //this extra parameter is a workaround in ef 2.2
                    organizationDb.SaveChanges();
                }
            }
            
            organizationDb.ProfileMetrics.AddRange(metricsToInsert);
            
            organizationDb.SaveChanges();

            logService.Log<GenerateProfileReportsLoader>($"{metricsToInsert.Count} new profile metrics saved to database.");
            return metricsToInsert;
        }
        
        #endregion
    }
}
