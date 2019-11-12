using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tayra.Models.Core;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateTeamReportsWeeklyLoader : BaseLoader
    {
        #region Private Variables

        #endregion

        #region Constructor

        public GenerateTeamReportsWeeklyLoader(LogService logService, CoreDbContext coreDb) : base(logService, coreDb)
        {

        }

        #endregion

        #region Public Methods

        public override void Execute(DateTime date, params Organization[] organizations)
        {
            foreach (var org in organizations)
            {
                LogService.SetOrganizationId(org.Id);
                using (var organizationDb = new OrganizationDbContext(org.Database, false))
                {
                    GenerateTeamReports(organizationDb, date, LogService);
                }
            }
        }

        #endregion

        #region Private Methods

        public static void GenerateTeamReports(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, List<ProfileReportDaily> profileReportsDaily = null, List<ProfileReportWeekly> profileReportsWeekly = null)
        {
            if (!CommonHelper.IsMonday(fromDay) || profileReportsWeekly == null)
                return;

            var dateId = DateHelper2.ToDateId(fromDay);
            
            if (profileReportsDaily == null)
            {
                profileReportsDaily = organizationDb.ProfileReportsDaily.Where(x => x.DateId == dateId).ToList();
            }

            if (profileReportsWeekly == null)
            {
                profileReportsWeekly = organizationDb.ProfileReportsWeekly.Where(x => x.DateId == dateId).ToList();
            }


            
            var reportsToInsert = new List<TeamReportWeekly>();
            
            var teams = (from t in organizationDb.Teams
                                where t.ArchivedAt == null
                                select new
                                {
                                    TeamId = t.Id,
                                    MemberIds = t.Members.Select(x => x.ProfileId).ToList()
                                }).ToList();

            foreach(var t in teams)
            {   
                var drs = profileReportsDaily.Where(x => t.MemberIds.Contains(x.ProfileId)).ToList();
                var drsCount = drs.Count();

                var wrs = profileReportsWeekly.Where(x => t.MemberIds.Contains(x.ProfileId)).ToList();
                var wrsCount = wrs.Count();

                if (drsCount == 0)
                    continue;

                
                reportsToInsert.Add(new TeamReportWeekly
                {
                    TeamId = t.TeamId,
                    DateId = dateId,
                    IterationCount = 1,
                    TaskCategoryId = 1,
                    MembersCountTotal = drsCount,

                    ComplexityChange = drs.Sum(x => x.ComplexityChange),
                    ComplexityAverage = (float)drs.Average(x => x.ComplexityChange),

                    CompanyTokensChange = drs.Sum(x => x.CompanyTokensChange),
                    CompanyTokensAverage = drs.Average(x => x.CompanyTokensChange),

                    EffortScoreChange = drs.Sum(x => x.EffortScoreChange),
                    EffortScoreAverage = drs.Average(x => x.EffortScoreChange),

                    OneUpsGivenChange = drs.Sum(x => x.OneUpsGivenChange),
                    OneUpsGivenAverage = (float)drs.Average(x => x.OneUpsGivenChange),

                    OneUpsReceivedChange = drs.Sum(x => x.OneUpsReceivedChange),
                    OneUpsReceivedAverage = (float)drs.Average(x => x.OneUpsReceivedChange),

                    AssistsChange = drs.Sum(x => x.AssistsChange),
                    AssistsAverage = (float)drs.Average(x => x.AssistsChange),

                    TasksCompletedChange = drs.Sum(x => x.TasksCompletedChange),
                    TasksCompletedAverage = (float)drs.Average(x => x.TasksCompletedChange),

                    TurnoverChange = drs.Sum(x => x.TurnoverChange),
                    TurnoverAverage = (float)drs.Average(x => x.TurnoverChange),

                    ErrorChange = drs.Sum(x => x.ErrorChange),
                    ErrorAverage = drs.Average(x => x.ErrorChange),

                    ContributionChange = drs.Sum(x => x.ContributionChange),
                    ContributionAverage = drs.Average(x => x.ContributionChange) / drsCount,

                    SavesChange = drs.Sum(x => x.SavesChange),
                    SavesAverage = (float)drs.Average(x => x.SavesChange) / drsCount,

                    TacklesChange = drs.Sum(x => x.TacklesChange),
                    TacklesAverage = (float)drs.Average(x => x.TacklesChange),

                    OImpactAverage = wrs.Average(x => x.OImpactAverage),
                    OImpactAverageTotal = wrs.Sum(x => x.OImpactAverage),

                    DImpactAverage = wrs.Average(x => x.DImpactAverage),
                    DImpactAverageTotal = wrs.Sum(x => x.DImpactAverage),

                    PowerAverage = wrs.Average(x => x.PowerAverage),
                    PowerAverageTotal = wrs.Sum(x => x.PowerAverage),

                    SpeedAverage = wrs.Average(x => x.SpeedAverage),
                    SpeedAverageTotal = wrs.Sum(x => x.SpeedAverage),

                    HeatAverageTotal = wrs.Average(x => x.Heat)
                });
            }

            var existing = organizationDb.TeamReportsWeekly.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<TeamReportWeekly>($"deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlCommand($"delete from TeamReportsWeekly where {nameof(TeamReportWeekly.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.TeamReportsWeekly.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateTeamReportsWeeklyLoader>($"{reportsToInsert.Count} new reports saved to database.");
        }

        #endregion
    }
}