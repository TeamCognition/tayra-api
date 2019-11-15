using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq;
using Tayra.Common;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.SyncServices.Common;

namespace Tayra.SyncServices.Tayra
{
    public class GenerateProfileReportsDailyLoader : BaseLoader
    {
        #region Private Variables

        #endregion

        #region Constructor

        public GenerateProfileReportsDailyLoader(LogService logService, CatalogDbContext catalogDb) : base(logService, catalogDb)
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
                    GenerateProfileReports(organizationDb, date, LogService);
                }
            }
        }



        #endregion

        #region Private Methods

        public static List<ProfileReportDaily> GenerateProfileReports(OrganizationDbContext organizationDb, DateTime fromDay, LogService logService)
        {
            var reportsToInsert = new List<ProfileReportDaily>();

            var dateId = DateHelper2.ToDateId(fromDay);
            var taskStats = (from t in organizationDb.Tasks
                            where t.Status == TaskStatuses.Done
                            group t by t.AssigneeProfileId into total
                            let change = total.Where(x => x.LastModifiedDateId == dateId)
                            select new
                            {
                                ProfileId = total.Key,

                                EffortScore = change.Sum(x => x.EffortScore),
                                EffortScoreTotal = total.Sum(x => x.EffortScore),

                                Complexity = change.Sum(x => x.Complexity),
                                ComplexityTotal = total.Sum(x => x.Complexity),
                                
                                Completed = change.Count(),
                                CompletedTotal = total.Count(),
                                
                                ProductionBugsFixed = change.Count(x => x.IsProductionBugFixing),
                                ProductionBugsFixedTotal = total.Count(x => x.IsProductionBugFixing),
                                
                                ProductionBugsCaused = change.Count(x => x.IsProductionBugCausing),
                                ProductionBugsCausedTotal = total.Count(x => x.IsProductionBugCausing),
                                
                                Errors = change.Where(x => x.IsProductionBugCausing).Sum(x => x.BugSeverity * x.BugPopulationAffect),
                                ErrorsTotal = total.Where(x => x.IsProductionBugCausing).Sum(x => x.BugSeverity * x.BugPopulationAffect),
                                
                                Saves = change.Count(x => x.IsProductionBugFixing && x.BugSeverity > 3),
                                SavesTotal = total.Count(x => x.IsProductionBugFixing && x.BugSeverity > 3),
                                
                                Turnovers = 0,
                                TurnoversTotal = 0,
                                
                                Tackles = 0,
                                TacklesTotal = 0
                            }).ToList();

            var companyTokenId = organizationDb.Tokens.FirstOrDefault(x => x.Type == TokenType.CompanyToken).Id;
            var tokens = (from tt in organizationDb.TokenTransactions
                        group tt by tt.ProfileId into total
                        let change = total.Where(x => x.Created.Date == fromDay.Date)
                        select new
                        {
                            ProfileId = total.Key,
                            CompanyTokens = change.Where(x => x.TokenId == companyTokenId).Sum(x => x.Value),
                            CompanyTokensTotal = total.Where(x => x.TokenId == companyTokenId).Sum(x => x.Value)
                        }).ToList();

            var oneUpsGiven = (from u in organizationDb.ProfileOneUps
                                group u by u.CreatedBy into total
                                let change = total.Where(x => x.DateId == dateId)
                                select new
                                {
                                    ProfileId = total.Key,

                                    Count = change.Count(),
                                    CountTotal = total.Count()
                                })
                                .ToList();

            var oneUpsReceived = (from u in organizationDb.ProfileOneUps
                                    group u by u.UppedProfileId into total
                                    let change = total.Where(x => x.DateId == dateId)
                                    select new
                                    {
                                        ProfileId = total.Key,

                                        Count = change.Count(),
                                        CountTotal = total.Count()
                                    })
                                    .ToList();

            var profiles = organizationDb.Profiles.Select(x => x.Id).ToList();
            foreach(var p in profiles)
            {
                var ts = taskStats.FirstOrDefault(x => x.ProfileId == p);
                var t = tokens.FirstOrDefault(x => x.ProfileId == p);
                var upsG = oneUpsGiven.FirstOrDefault(x => x.ProfileId == p);
                var upsR = oneUpsReceived.FirstOrDefault(x => x.ProfileId == p);
                var iterationCount = 1;

                reportsToInsert.Add(new ProfileReportDaily
                {
                    ProfileId = p,
                    DateId = dateId,
                    IterationCount = iterationCount, 
                    TaskCategoryId = 1,

                    ComplexityChange = ts?.Complexity ?? 0,
                    ComplexityTotal = ts?.ComplexityTotal ?? 0,

                    CompanyTokensChange = (float)(t?.CompanyTokens ?? 0),
                    CompanyTokensTotal = (float)(t?.CompanyTokensTotal ?? 0),
                    
                    EffortScoreChange = ts?.EffortScore ?? 0f,
                    EffortScoreTotal = ts?.EffortScoreTotal ?? 0f,
                    
                    OneUpsGivenChange = upsG?.Count ?? 0,
                    OneUpsGivenTotal = upsG?.CountTotal ?? 0,
                    
                    OneUpsReceivedChange = upsR?.Count ?? 0,
                    OneUpsReceivedTotal = upsR?.CountTotal ?? 0,

                    AssistsChange = (upsR?.Count) ?? 0,
                    AssistsTotal = (upsR?.CountTotal) ?? 0,

                    TasksCompletedChange = ts?.Completed ?? 0,
                    TasksCompletedTotal = ts?.CompletedTotal ?? 0,
                    
                    TurnoverChange = ts?.Turnovers ?? 0,
                    TurnoverTotal = ts?.TurnoversTotal ?? 0,
                    
                    ErrorChange = ts?.Errors ?? 0,
                    ErrorTotal = ts?.ErrorsTotal ?? 0,

                    ContributionChange = (ts?.Complexity - ts?.Turnovers - ts?.Errors) ?? 0,
                    ContributionTotal = (ts?.ComplexityTotal - ts?.TurnoversTotal - ts?.ErrorsTotal) ?? 0,
                    
                    SavesChange = ts?.Saves ?? 0,
                    SavesTotal = ts?.SavesTotal ?? 0,
                    
                    TacklesChange = ts?.Tackles ?? 0,
                    TacklesTotal = ts?.TacklesTotal ?? 0,
                });
            }

            var existing = organizationDb.ProfileReportsDaily.Count(x => x.DateId == dateId);
            if (existing > 0)
            {
                logService.Log<ProfileReportDaily>($"deleting {existing} records from database");
                organizationDb.Database.ExecuteSqlCommand($"delete from ProfileReportsDaily where {nameof(ProfileReportDaily.DateId)} = {dateId}", dateId); //this extra parameter is a workaround in ef 2.2
                organizationDb.SaveChanges();
            }                    

            organizationDb.ProfileReportsDaily.AddRange(reportsToInsert);
            organizationDb.SaveChanges();
            
            logService.Log<GenerateReportsLoader>($"{reportsToInsert.Count} new profile reports saved to database.");
            return reportsToInsert;
        }

        #endregion
    }
}
