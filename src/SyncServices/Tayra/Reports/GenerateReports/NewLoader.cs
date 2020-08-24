using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Services;
using Tayra.SyncServices.Common;
using Tayra.SyncServices.Metrics;

namespace Tayra.SyncServices.Tayra
{
    public class NewLoader : BaseLoader
    {
        #region Private Variables

        private readonly IShardMapProvider _shardMapProvider;

        #endregion

        #region Constructor

        public NewLoader(IShardMapProvider shardMapProvider, LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
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

        public static List<ProfileReportDaily> GenerateProfileReportsDaily(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, params int[] segmentIds)
        {
            var reportsToInsert = new List<ProfileReportDaily>();

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
                              select tt).ToList();

                var praisesGiven = (from u in organizationDb.ProfilePraises
                                   where profileIds.Contains(u.PraiserProfileId)
                                   where u.DateId == dateId
                                   group u by u.PraiserProfileId into g
                                   select new
                                   {
                                       ProfileId = g.Key,

                                       Usernames = g.Select(x => x.Profile.Username).ToArray(),
                                       Count = g.Count()
                                   }).ToList();

                var praisesReceived = (from u in organizationDb.ProfilePraises
                                      where profileIds.Contains(u.ProfileId)
                                      where u.DateId == dateId
                                      group u by u.ProfileId into g
                                      select new
                                      {
                                          ProfileId = g.Key,

                                          Usernames = organizationDb.Profiles.Where(x => g.Select(c => c.CreatedBy).Contains(x.Id)).Select(x => x.Username).ToArray(),
                                          Count = g.Count(),
                                      }).ToList();

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
                                   group i by i.ProfileId into total
                                   let change = total.Where(x => x.DateId == dateId)
                                   select new
                                   {
                                       ProfileId = total.Key,

                                       Names = change.Select(x => x.Item.Name).ToArray(),
                                       Count = change.Count(),
                                       CountTotal = total.Count()
                                   }).ToList();

                var giftsSent = (from u in organizationDb.ItemGifts
                                 where profileIds.Contains(u.SenderId)
                                 group u by u.SenderId into total
                                 let change = total.Where(x => x.DateId == dateId)
                                 select new
                                 {
                                     ProfileId = total.Key,
                                     Gifts = change.Select(x => x.Receiver.Username + " - " + x.Item.Name).ToArray(),
                                     Count = change.Count(),
                                     CountTotal = total.Count()
                                 }).ToList();

                var giftsReceived = (from u in organizationDb.ItemGifts
                                     where profileIds.Contains(u.ReceiverId)
                                     group u by u.ReceiverId into total
                                     let change = total.Where(x => x.DateId == dateId)
                                     select new
                                     {
                                         ProfileId = total.Key,
                                         Gifts = change.Select(x => x.Sender.Username + " - " + x.Item.Name).ToArray()
                                     }).ToList();

                var shopPurchases = (from sp in organizationDb.ShopPurchases
                                     where profileIds.Contains(sp.ProfileId)
                                     where sp.Status == ShopPurchaseStatuses.Fulfilled
                                     group sp by sp.ProfileId into total
                                     let change = total.Where(x => x.LastModifiedDateId == dateId)
                                     select new
                                     {
                                         ProfileId = total.Key,
                                         Something = total.Select(x => x.Created).ToArray(),
                                         Names = change.Select(x => x.Item.Name).ToArray(),
                                         Count = change.Count(),
                                         CountTotal = total.Count()
                                     }).ToList();

                var inventory = (from pinv in organizationDb.ProfileInventoryItems
                                 where profileIds.Contains(pinv.ProfileId)
                                 group pinv by pinv.ProfileId into total
                                 select new
                                 {
                                     ProfileId = total.Key,

                                     TotalCount = total.Count(),
                                     TotalValue = total.Sum(x => x.Item.Price)
                                 }).ToList();

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
                    select new
                    {
                        ProfileId = gc.AuthorProfileId,
                        Message = gc.Message,
                        ExternalUrl = gc.ExternalUrl
                    }).ToList();

                var profileMetrics = new List<ProfileMetrics>();
                foreach (var p in profiles)
                {
                    var ts = tasks.Where(x => x.AssigneeProfileId == p.Id);
                    var t = tokens.Where(x => x.ProfileId == p.Id);
                    var praisesG = praisesGiven.FirstOrDefault(x => x.ProfileId == p.Id);
                    var praisesR = praisesReceived.FirstOrDefault(x => x.ProfileId == p.Id);
                    var sp = shopPurchases.FirstOrDefault(x => x.ProfileId == p.Id);
                    var iCreated = itemsCreated.FirstOrDefault(x => x.ProfileId == p.Id);
                    var iDissed = itemsDissed.FirstOrDefault(x => x.ProfileId == p.Id);
                    var iGiftS = giftsSent.FirstOrDefault(x => x.ProfileId == p.Id);
                    var iGiftR = giftsReceived.FirstOrDefault(x => x.ProfileId == p.Id);
                    var inv = inventory.FirstOrDefault(x => x.ProfileId == p.Id);
                    var q = quests.FirstOrDefault(x => x.ProfileId == p.Id);
                    var gcommits = gitCommitsToday.Where(x => x.ProfileId == p.Id).ToArray();
                    var iterationCount = 1;
                    
                    
                    
                    profileMetrics.Add(new ProfileMetrics(p.Id, segmentId, new EffortMetric(ts)));
                    profileMetrics.Add(new ProfileMetrics(p.Id, segmentId, new ComplexityMetric(ts)));
                    profileMetrics.Add(new ProfileMetrics(p.Id, segmentId, new TokensEarnedMetric(t)));
                    profileMetrics.Add(new ProfileMetrics(p.Id, segmentId, new TokensSpentMetric(t)));
                    profileMetrics.Add(new ProfileMetrics(p.Id, segmentId, new PraisesGivenMetric(praisesG?.Count ?? 0)));
                    profileMetrics.Add(new ProfileMetrics(p.Id, segmentId, new PraisesReceivedMetric(praisesR?.Count ?? 0)));
                    profileMetrics.Add(new ProfileMetrics(p.Id, segmentId, new AssistMetric(praisesR?.Count ?? 0)));
                    profileMetrics.Add(new ProfileMetrics(p.Id, segmentId, new WorkUnitsCompletedMetric(ts.Count())));
                    profileMetrics.Add(new ProfileMetrics(p.Id, segmentId, new SavesMetric(ts.Count(x => x.IsProductionBugFixing && x.BugSeverity > 3))));
                    

                }

                var existing = organizationDb.ProfileReportsDaily.Count(x => x.DateId == dateId && x.SegmentId == segmentId);
                if (existing > 0)
                {
                    logService.Log<ProfileReportDaily>($"deleting {existing} records from database");
                    //organizationDb.Database.ExecuteSqlInterpolated($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId} AND {nameof(ProfileReportDaily.SegmentId)} = {segmentId}");
                    organizationDb.Database.ExecuteSqlCommand($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId} AND {nameof(ProfileReportDaily.SegmentId)} = {segmentId}", dateId); //this extra parameter is a workaround in ef 2.2
                    organizationDb.SaveChanges();
                }
            }
            organizationDb.ProfileReportsDaily.AddRange(reportsToInsert);
            
            organizationDb.SaveChanges();

            logService.Log<GenerateProfileReportsLoader>($"{reportsToInsert.Count} new profile reports saved to database.");
            return reportsToInsert;
        }
        
        #endregion
    }
}
