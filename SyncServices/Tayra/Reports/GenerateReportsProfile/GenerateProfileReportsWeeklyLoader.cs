using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateProfileReportsWeeklyLoader : BaseLoader
    {
        #region Private Variables

        #endregion

        #region Constructor

        public GenerateProfileReportsWeeklyLoader(LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
        {

        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, params Tenant[] organizations)
        {
            foreach (var org in organizations)
            {
                LogService.SetOrganizationId(org.Id);
                using (var organizationDb = new OrganizationDbContext(org.Database, false))
                {
                    GenerateProfileReports(organizationDb, date, LogService);
                }
            }
        }



        #endregion

        #region Private Methods

        public static List<ProfileReportWeekly> GenerateProfileReports(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            if (!CommonHelper.IsMonday(fromDay))
                return null;


            var reportsToInsert = new List<ProfileReportWeekly>();

            var iterationDays = 7;
            var dateId = DateHelper2.ToDateId(fromDay);
            var dateId1ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays));
            var dateId2ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays * 2));
            var dateId4ago = DateHelper2.ToDateId(fromDay.AddDays(-iterationDays * 4));

            reportsToInsert = (from d in organizationDb.ProfileReportsDaily
                               where d.DateId <= dateId
                               group d by d.ProfileId into dg
                               let dgCount = dg.Count()
                               //let dgCountWorked = dg.Count()
                               let last1 = dg.Where(x => x.DateId > dateId1ago) //last 1 iteration
                               let last4 = dg.Where(x => x.DateId > dateId4ago)
                               let ic = (dgCount + 7 - 1) / 7
                               let icMax4 = Math.Min(4, ic)
                               select new ProfileReportWeekly
                               {
                                   ProfileId = dg.Key,
                                   DateId = dateId,
                                   IterationCount = ic,
                                   TaskCategoryId = 1,

                                   ComplexityChange = last1.Sum(x => x.ComplexityChange),
                                   ComplexityTotalAverage = (float)dg.Sum(c => c.ComplexityChange) / ic,

                                   CompanyTokensChange = last1.Sum(x => x.CompanyTokensChange),
                                   CompanyTokensTotalAverage = dg.Sum(c => c.CompanyTokensChange) / ic,

                                   EffortScoreChange = last1.Sum(x => x.EffortScoreChange),
                                   EffortScoreTotalAverage = dg.Sum(c => c.EffortScoreChange) / ic,

                                   OneUpsGivenChange = last1.Sum(x => x.OneUpsGivenChange),
                                   OneUpsGivenTotalAverage = (float)dg.Sum(c => c.OneUpsGivenChange) / ic,

                                   OneUpsReceivedChange = last1.Sum(x => x.OneUpsReceivedChange),
                                   OneUpsReceivedTotalAverage = (float)dg.Sum(c => c.OneUpsReceivedChange) / ic,

                                   AssistsChange = last1.Sum(x => x.AssistsChange),
                                   AssistsTotalAverage = (float)dg.Sum(c => c.AssistsChange) / ic,

                                   TasksCompletedChange = last1.Sum(x => x.TasksCompletedChange),
                                   TasksCompletedTotalAverage = (float)dg.Sum(c => c.TasksCompletedChange) / ic,

                                   TurnoverChange = last1.Sum(x => x.TurnoverChange),
                                   TurnoverTotalAverage = (float)dg.Sum(c => c.TurnoverChange) / ic,

                                   ErrorChange = last1.Sum(x => x.ErrorChange),
                                   ErrorTotalAverage = dg.Sum(c => c.ErrorChange) / ic,

                                   ContributionChange = last1.Sum(x => x.ContributionChange),
                                   ContributionTotalAverage = dg.Sum(c => c.ContributionChange) / ic,

                                   SavesChange = last1.Sum(x => x.SavesChange),
                                   SavesTotalAverage = (float)dg.Sum(c => c.SavesChange) / ic,

                                   TacklesChange = last1.Sum(x => x.TacklesChange),
                                   TacklesTotalAverage = (float)dg.Sum(c => c.TacklesChange) / ic,

                                   OImpactAverage = (float)last4.Sum(x => x.ComplexityChange + x.TasksCompletedChange + x.AssistsChange) / icMax4,
                                   OImpactTotalAverage = (float)dg.Sum(x => x.ComplexityChange + x.TasksCompletedChange + x.AssistsChange) / ic,

                                   DImpactAverage = (float)last4.Sum(x => x.SavesChange + x.TacklesChange) / icMax4,
                                   DImpactTotalAverage = (float)dg.Sum(x => x.SavesChange + x.TacklesChange) / ic,

                                   PowerAverage = last4.Sum(x => x.ComplexityChange / (float)x.TasksCompletedChange) / icMax4,
                                   PowerTotalAverage = dg.Sum(x => x.ComplexityChange / (float)x.TasksCompletedChange) / ic,

                                   SpeedAverage = last4.Sum(x => x.ComplexityChange) / icMax4,
                                   SpeedTotalAverage = (float)dg.Sum(x => x.ComplexityChange) / ic,

                                   HeatIndex = last1.Sum(x => x.ComplexityChange) / (float)dg.Where(x => x.DateId > dateId2ago && x.DateId <= dateId1ago).Sum(x => x.ComplexityChange)
                               }).ToList();

            var lastReportsHeat = (from r in organizationDb.ProfileReportsWeekly
                                   where r.DateId == dateId1ago
                                   select new
                                   {
                                       r.ProfileId,
                                       r.Heat
                                   })
                                    .DistinctBy(x => x.ProfileId).ToList(); //ProjectArea is ignored cuz we are only taking 'Totals'


            foreach (var r in reportsToInsert)
            {
                r.PowerAverage = double.IsNaN(r.PowerAverage) || double.IsInfinity(r.PowerAverage) ? 0 : r.PowerAverage;
                r.PowerTotalAverage = double.IsNaN(r.PowerTotalAverage) || double.IsInfinity(r.PowerTotalAverage) ? 0 : r.PowerTotalAverage;
                r.HeatIndex = double.IsNaN(r.HeatIndex) || double.IsInfinity(r.HeatIndex) ? 0 : r.HeatIndex;

                var lastHeat = lastReportsHeat.FirstOrDefault(x => x.ProfileId == r.ProfileId)?.Heat;

                if (!lastHeat.HasValue)
                {
                    r.Heat = 22f;
                }
                else
                {
                    r.Heat = lastHeat.Value * r.HeatIndex;
                }
            }

            var existing = organizationDb.ProfileReportsWeekly.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<ProfileReportWeekly>($"deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlCommand($"delete from ProfileReportsWeekly where {nameof(ProfileReportWeekly.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.ProfileReportsWeekly.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateReportsLoader>($"{reportsToInsert.Count} new profile reports saved to database.");
            return reportsToInsert;
        }

        #endregion
    }
}
