using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateProjectReportsDailyLoader : BaseLoader
    {
        #region Constructor

        public GenerateProjectReportsDailyLoader(LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
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
                    GenerateProjectReports(organizationDb, date, LogService);
                }
            }
        }

        public static void GenerateProjectReports(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService, List<ProfileReportDaily> profileReportsDaily = null)
        {
            var dateId = DateHelper2.ToDateId(fromDay);

            if (profileReportsDaily == null)
            {
                profileReportsDaily = organizationDb.ProfileReportsDaily.Where(x => x.DateId == dateId).ToList();
            }


            var reportsToInsert = new List<ProjectReportDaily>();

            var projects = (from t in organizationDb.Projects
                            where t.ArchivedAt == null
                            select new
                            {
                                TeamId = t.Id,
                                MemberIds = t.Members.Select(x => x.ProfileId).ToList()
                            }).ToList();

            foreach (var t in projects)
            {
                var mr = profileReportsDaily.Where(x => t.MemberIds.Contains(x.ProfileId)).ToList();

                reportsToInsert.Add(new ProjectReportDaily
                {
                    ProjectId = t.TeamId,
                    DateId = dateId,
                    IterationCount = 1,
                    TaskCategoryId = 1,
                    MembersCountTotal = mr.Count(),

                    ComplexityChange = mr.Sum(x => x.ComplexityChange),
                    ComplexityTotal = mr.Sum(x => x.ComplexityTotal),

                    CompanyTokensChange = mr.Sum(x => x.CompanyTokensChange),
                    CompanyTokensTotal = mr.Sum(x => x.CompanyTokensTotal),

                    EffortScoreChange = mr.Sum(x => x.EffortScoreChange),
                    EffortScoreTotal = mr.Sum(x => x.EffortScoreTotal),

                    OneUpsGivenChange = mr.Sum(x => x.OneUpsGivenChange),
                    OneUpsGivenTotal = mr.Sum(x => x.OneUpsGivenTotal),

                    OneUpsReceivedChange = mr.Sum(x => x.OneUpsReceivedChange),
                    OneUpsReceivedTotal = mr.Sum(x => x.OneUpsReceivedTotal),

                    AssistsChange = mr.Sum(x => x.AssistsChange),
                    AssistsTotal = mr.Sum(x => x.AssistsTotal),

                    TasksCompletedChange = mr.Sum(x => x.TasksCompletedChange),
                    TasksCompletedTotal = mr.Sum(x => x.TasksCompletedTotal),

                    TurnoverChange = mr.Sum(x => x.TurnoverChange),
                    TurnoverTotal = mr.Sum(x => x.TurnoverTotal),

                    ErrorChange = mr.Sum(x => x.ErrorChange),
                    ErrorTotal = mr.Sum(x => x.ErrorTotal),

                    ContributionChange = mr.Sum(x => x.ContributionChange),
                    ContributionTotal = mr.Sum(x => x.ContributionTotal),

                    SavesChange = mr.Sum(x => x.SavesChange),
                    SavesTotal = mr.Sum(x => x.SavesTotal),

                    TacklesChange = mr.Sum(x => x.TacklesChange),
                    TacklesTotal = mr.Sum(x => x.TacklesTotal),
                });
            }


            var existing = organizationDb.ProjectReportsDaily.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<ProjectReportDaily>($"deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlCommand($"delete from ProjectReportsDaily where {nameof(ProjectReportDaily.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }

            organizationDb.ProjectReportsDaily.AddRange(reportsToInsert);
            organizationDb.SaveChanges();

            logService.Log<GenerateProjectReportsDailyLoader>($"{reportsToInsert.Count} new reports saved to database.");
        }

        #endregion
    }
}