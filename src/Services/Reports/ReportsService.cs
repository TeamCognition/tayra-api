using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class ReportsService : BaseService<OrganizationDbContext>, IReportsService
    {
        #region Constructor

        public ReportsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public ReportStatusDTO[] GetReportStatus(int[] segmentIds)
        {
            return (from s in DbContext.Segments
                    where segmentIds.Contains(s.Id)
                    select new ReportStatusDTO
                    {
                        SegmentId = s.Id,
                        IsReportingUnlocked = s.IsReportingUnlocked,
                        TotalMembers = s.Members.Where(x => x.Profile.Role == ProfileRoles.Member).Select(x => x.ProfileId).Distinct().Count(),
                        LinkedMembers = s.MembersLinked.Count(x => x.IntegrationType == IntegrationType.ATJ)
                    }).ToArray();
        }

        public void UnlockReporting(int segmentId)
        {
            var segment = DbContext.Segments.FirstOrDefault(x => x.Id == segmentId);

            segment.EnsureNotNull(segmentId);

            segment.IsReportingUnlocked = true;

            //okini reporte
        }

        public ReportOverviewDTO GetOverviewReport(ReportParams reportParams)
        {
            var avg = (from srw in DbContext.SegmentReportsWeekly
                      where srw.DateId >= reportParams.From && srw.DateId <= reportParams.To
                      where srw.SegmentId == reportParams.SegmentId
                      //orderby trw.DateId ascending
                      group srw by 1 into r
                      select new 
                      {
                          OImpactAverage = r.Average(x => x.OImpactAverage),
                          SpeedAverage = r.Average(x => x.SpeedAverage),
                          PowerAverage = r.Average(x => x.PowerAverage),
                          HeatAverage = r.Average(x => x.HeatAverageTotal)
                      }).FirstOrDefault();

            var dateFrom = DateHelper2.ParseDate(reportParams.From);
            var dateTo = DateHelper2.ParseDate(reportParams.To);

            return new ReportOverviewDTO
            {
                Statistics = (from srw in DbContext.SegmentReportsDaily
                              where srw.DateId >= reportParams.From && srw.DateId <= reportParams.To
                              where srw.SegmentId == reportParams.SegmentId
                              orderby srw.DateId descending
                              group srw by 1 into r
                              select new ReportOverviewDTO.StatisticsDTO
                              {
                                  ActiveTeams = DbContext.Teams.Where(x => x.SegmentId == reportParams.SegmentId && x.Created >= dateFrom && x.Created <= dateTo).Count(),
                                  ActiveMembers = r.Select(x => x.MembersCountTotal).FirstOrDefault(),
                                  ActiveChallenges = DbContext.ChallengeSegments.Where(x => x.SegmentId == reportParams.SegmentId && x.Challenge.Status == ChallengeStatuses.Active && x.Created >= dateFrom && x.Created <= dateTo).Count(),
                                  ActiveIntegrations = DbContext.Integrations.Where(x => x.SegmentId == reportParams.SegmentId && x.Created >= dateFrom && x.Created <= dateTo).GroupBy(x => x.Type).Count(),
                                  ShopItemsBought = r.Sum(x => x.ItemsBoughtChange)
                              }).FirstOrDefault(),

                Metrics = new ReportOverviewDTO.MetricDTO[]
                {
                    new ReportOverviewDTO.MetricDTO
                    {
                        Id = MetricTypes.OImpact,
                        MaxValue = 30,
                        AverageValue = avg?.OImpactAverage ?? 0
                    },
                    new ReportOverviewDTO.MetricDTO
                    {
                        Id = MetricTypes.Speed,
                        MaxValue = 30,
                        AverageValue = avg?.SpeedAverage ?? 0
                    },
                    new ReportOverviewDTO.MetricDTO
                    {
                        Id = MetricTypes.Power,
                        MaxValue = 40,
                        AverageValue = avg?.PowerAverage ?? 0
                    }
                },
                Nodes = (from trw in DbContext.TeamReportsWeekly
                      where trw.DateId >= reportParams.From && trw.DateId <= reportParams.To
                      where trw.SegmentId == reportParams.SegmentId && trw.MembersCountTotal > 0
                      group trw by trw.TeamId into r
                      select new ReportOverviewDTO.NodeDTO
                      {
                          Name = r.FirstOrDefault().Team.Name,
                          Metrics = new ReportOverviewDTO.NodeDTO.MetricDTO[]
                          {
                              new ReportOverviewDTO.NodeDTO.MetricDTO
                              {
                                  Id = MetricTypes.OImpact,
                                  Value = r.Average(a => a.OImpactAverage)
                              },
                              new ReportOverviewDTO.NodeDTO.MetricDTO
                              {
                                  Id = MetricTypes.Speed,
                                  Value = r.Average(a => a.SpeedAverage)
                              },
                              new ReportOverviewDTO.NodeDTO.MetricDTO
                              {
                                  Id = MetricTypes.Power,
                                  Value = r.Average(a => a.PowerAverage)
                              }
                          }
                      }).ToArray()
            };
        }

        public ReportMembersPerformanceDTO GetMembersPerformanceReport(ReportParams reportParams)
        {
            var profileIds = DbContext.ProfileAssignments.Where(x => x.SegmentId == reportParams.SegmentId).Select(x => x.ProfileId).ToArray();

            var ms = (from prw in DbContext.ProfileReportsDaily
                                  where profileIds.Contains(prw.ProfileId)
                                  where prw.DateId >= reportParams.From && prw.DateId <= reportParams.To
                                  where prw.Profile.Role == ProfileRoles.Member 
                                  orderby prw.DateId descending
                                  group prw by prw.ProfileId into m
                                  select new ReportMembersPerformanceDTO.MembersPerformanceDTO
                                  {
                                      Name = $"{m.First().Profile.FirstName} {m.First().Profile.LastName}",
                                      TasksCompleted = m.Sum(x => x.TasksCompletedChange),
                                      Complexity = m.Sum(x => x.ComplexityChange),
                                      Assists = m.Sum(x => x.ComplexityChange),
                                      AvarageCompletionTime = m.Sum(x => x.TasksCompletionTimeChange),
                                      Tokens = m.Sum(x => x.CompanyTokensEarnedChange),
                                      InventoryValue = m.FirstOrDefault().InventoryValueTotal,
                                      InventoryItems = m.FirstOrDefault().InventoryCountTotal,
                                      Impact = DbContext.ProfileReportsWeekly.Where(x => x.ProfileId == m.Key).Select(x => x.OImpactAverage).FirstOrDefault(),
                                      Speed = DbContext.ProfileReportsWeekly.Where(x => x.ProfileId == m.Key).Select(x => x.SpeedAverage).FirstOrDefault(),
                                      Power = DbContext.ProfileReportsWeekly.Where(x => x.ProfileId == m.Key).Select(x => x.PowerAverage).FirstOrDefault()
                                  }).ToArray();

            if (!ms.Any())
            {
                return null;
            }

            return new ReportMembersPerformanceDTO
            {
                MembersPerformance = ms
            };
        }
          
        public ReportDeliverySegmentMetricsDTO GetDeliverySegmentMetricsReport(ReportParams reportParams)
        {
            var teamIds = DbContext.TeamsScopeOfSegment(reportParams.SegmentId).Select(x => x.Id);

            var wr = (from trw in DbContext.TeamReportsWeekly
                      where trw.DateId >= reportParams.From && trw.DateId <= reportParams.To
                      where teamIds.Contains(trw.TeamId)
                      orderby trw.DateId ascending
                      select new
                      {
                          trw.TeamId,
                          trw.DateId,
                          TasksCompletionTimeChange = trw.TasksCompletionTimeChange,
                          TasksCompletedChange = trw.TasksCompletedChange,
                          MinutesSpentAverage = trw.TasksCompletedChange != 0 ? trw.TasksCompletionTimeChange / trw.TasksCompletedChange : 0
                      }).ToArray();

            if(!wr.Any())
            {
                return null;
            }

            var startDateId = wr.First().DateId;
            var endDateId = wr.Last().DateId;

            var maxTime = (from t in DbContext.Tasks
                           where t.Status == TaskStatuses.Done
                           where t.LastModifiedDateId >= startDateId && t.LastModifiedDateId <= endDateId
                           orderby t.TimeSpentInMinutes, t.AutoTimeSpentInMinutes
                           select t.TimeSpentInMinutes ?? t.AutoTimeSpentInMinutes)
                        .FirstOrDefault();

            var minTime = (from t in DbContext.Tasks
                           where t.Status == TaskStatuses.Done
                           where t.LastModifiedDateId >= startDateId && t.LastModifiedDateId <= endDateId
                           orderby t.TimeSpentInMinutes descending, t.AutoTimeSpentInMinutes descending
                           select t.TimeSpentInMinutes ?? t.AutoTimeSpentInMinutes)
                           .FirstOrDefault();

            var weeks = wr.Count(); //((endDateId - startDateId) / 7) + 1;
            return new ReportDeliverySegmentMetricsDTO
            {
                StartDateId = startDateId,
                EndDateId = endDateId,
                MinTime = minTime ?? 0,
                MaxTime = maxTime ?? 0,
                TotalTasksCompleted = wr.Sum(x => x.TasksCompletedChange),
                AvgTime = Math.Round(wr.Sum(x => x.TasksCompletionTimeChange) / (float)wr.Sum(x => x.TasksCompletedChange), 2),
                Teams = teamIds.Select(tId => new ReportDeliverySegmentMetricsDTO.TeamDTO
                {
                    TeamId = tId,
                    AverageTaskCompletionTime = wr.Where(x => x.TeamId == tId).Select(x => x.MinutesSpentAverage).ToArray().EnsureSize(weeks)
                }).ToArray()
            };
        }

        public ReportDeliveryTeamMetricsDTO GetDeliveryTeamMetricsReport(int teamId, ReportParams reportParams)
        {
            var ms = (from trd in DbContext.TeamReportsDaily
                      where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                      where trd.TeamId == teamId
                      orderby trd.DateId ascending
                      select new
                      {
                          DateId = trd.DateId,
                          MinutesSpentAverage = trd.TasksCompletedChange != 0 ? trd.TasksCompletionTimeChange / trd.TasksCompletedChange : 0
                      }).ToArray();

            if (!ms.Any())
            {
                return null;
            }

            var tm = (from trd in DbContext.TeamReportsWeekly
                      where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                      where trd.TeamId == teamId
                      group trd by 1 into g
                      select new
                      {
                          SpeedAverage = g.Average(x => x.SpeedAverage),
                          ComplexityAverage = g.Average(x => x.ComplexityAverage)
                      }).FirstOrDefault();

            var tasks = (from trd in DbContext.Tasks
                         where trd.LastModifiedDateId >= reportParams.From && trd.LastModifiedDateId <= reportParams.To
                         where trd.TeamId == teamId
                         select new
                         {
                             DateId = trd.LastModifiedDateId,
                             Name = trd.Summary,
                             MinutesSpent = trd.TimeSpentInMinutes ?? trd.AutoTimeSpentInMinutes,
                             Complexity = trd.Complexity,
                             AssigneeName = trd.AssigneeProfile.FirstName + " " + trd.AssigneeProfile.LastName
                         }).ToArray();

            if (!tasks.Any())
            {
                return null;
            }

            return new ReportDeliveryTeamMetricsDTO
            {
                StartDateId = ms.First().DateId,
                EndDateId = ms.Last().DateId,
                ComplexityAverage = tm?.ComplexityAverage ?? 0,
                SpeedAverage = tm?.SpeedAverage ?? 0,
                Data = ms.Select(x => new ReportDeliveryTeamMetricsDTO.DataDTO
                {
                    DateId = x.DateId,
                    Tasks = tasks.Where(t => t.DateId == x.DateId).Select(t => new ReportDeliveryTeamMetricsDTO.DataDTO.TaskDTO
                    {
                        Name = t.Name,
                        MinutesSpent = t.MinutesSpent ?? 0,
                        Complexity = t.Complexity,
                        AssigneeName = t.AssigneeName
                    }).ToArray()
                }).ToArray()
            };
        }

        public ReportStatisticsSegmentMetricsDTO GetStatisticsSegmentMetricsReport(ReportParams reportParams)
        {
            var wr = (from trw in DbContext.SegmentReportsWeekly
                      where trw.DateId >= reportParams.From && trw.DateId <= reportParams.To
                      where trw.SegmentId == reportParams.SegmentId
                      orderby trw.DateId ascending
                      select new
                      {
                          DateId = trw.DateId,
                          OImpact = trw.OImpactAverage,
                          Speed = trw.SpeedAverage,
                          Power = trw.PowerAverage,
                          Complexity = trw.ComplexityAverage,
                          Assists = trw.AssistsAverage,
                          TasksCompleted = trw.TasksCompletedAverage
                      }).ToList();

            if (!wr.Any())
            {
                return null;
            }

            return new ReportStatisticsSegmentMetricsDTO
            {
                Metrics = new[]
                {
                    new ReportStatisticsSegmentMetricsDTO.MetricDTO { MetricId = MetricTypes.OImpact, Data = wr.Select(x => new ReportStatisticsSegmentMetricsDTO.MetricDTO.DataDTO { DateId = x.DateId, Average = x.OImpact }).ToArray() },
                    new ReportStatisticsSegmentMetricsDTO.MetricDTO { MetricId = MetricTypes.Speed, Data = wr.Select(x => new ReportStatisticsSegmentMetricsDTO.MetricDTO.DataDTO { DateId = x.DateId, Average = x.Speed }).ToArray() },
                    new ReportStatisticsSegmentMetricsDTO.MetricDTO { MetricId = MetricTypes.Power, Data = wr.Select(x => new ReportStatisticsSegmentMetricsDTO.MetricDTO.DataDTO { DateId = x.DateId, Average = x.Power }).ToArray() },
                    new ReportStatisticsSegmentMetricsDTO.MetricDTO { MetricId = MetricTypes.Complexity, Data = wr.Select(x => new ReportStatisticsSegmentMetricsDTO.MetricDTO.DataDTO { DateId = x.DateId, Average = x.Complexity }).ToArray() },
                    new ReportStatisticsSegmentMetricsDTO.MetricDTO { MetricId = MetricTypes.Assist, Data = wr.Select(x => new ReportStatisticsSegmentMetricsDTO.MetricDTO.DataDTO { DateId = x.DateId, Average = x.Assists }).ToArray() },
                    new ReportStatisticsSegmentMetricsDTO.MetricDTO { MetricId = MetricTypes.TaskCompletion, Data = wr.Select(x => new ReportStatisticsSegmentMetricsDTO.MetricDTO.DataDTO { DateId = x.DateId, Average = x.TasksCompleted }).ToArray() }
                }
            };
        }

        public ReportStatisticsTeamMetricsDTO GetStatisticsTeamMetricsReport(int teamId, ReportParams reportParams)
        {
            //TODO: make this secure
            var profileIds = DbContext.ProfileAssignments.Where(x => x.TeamId == teamId).Select(x => x.ProfileId).ToArray();

            var ms = (from prw in DbContext.ProfileReportsWeekly
                      where prw.DateId >= reportParams.From && prw.DateId <= reportParams.To
                      where prw.ProfileRole == ProfileRoles.Member && profileIds.Contains(prw.ProfileId)
                      orderby prw.DateId ascending
                      group prw by prw.ProfileId into pr
                      select new ReportStatisticsTeamMetricsDTO.MemberDTO
                      {
                          ProfileId = pr.Key,
                          Name = pr.FirstOrDefault().Profile.FirstName + " " + pr.FirstOrDefault().Profile.LastName,
                          Avatar = pr.FirstOrDefault().Profile.Avatar,
                          Metrics = new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO[]
                          {
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.OImpact, Average = pr.Average(x => x.OImpactAverage) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.Speed, Average = pr.Average(x => x.SpeedTotalAverage) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.Power, Average = pr.Average(x => x.PowerAverage) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.Complexity, Average = pr.Average(x => x.ComplexityChange) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.Assist, Average = pr.Average(x => x.AssistsChange) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.TaskCompletion, Average = pr.Average(x => x.TasksCompletedChange) },
                          },
                          HeatTrend = pr.Select(x => x.Heat).ToArray()
                      }).ToArray();

            if (!ms.Any())
            {
                return null;
            }

            return new ReportStatisticsTeamMetricsDTO
            {
                Members = ms
            };
        }

        public ReportTokensSegmentMetricsDTO GetTokensSegmentMetricsReport(ReportParams reportParams)
        {
            var tm = (from trd in DbContext.SegmentReportsDaily
                      where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                      where trd.SegmentId == reportParams.SegmentId
                      group trd by 1 into g
                      select new
                      {
                          TokensEarnedTotal = g.Sum(x => x.CompanyTokensEarnedChange),
                          TokensSpentTotal = g.Sum(x => x.CompanyTokensSpentChange)
                      }).FirstOrDefault();


            var ms = (from trd in DbContext.SegmentReportsDaily
                      where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                      where trd.SegmentId == reportParams.SegmentId
                      orderby trd.DateId ascending
                      select new
                      {
                          DateId = trd.DateId,
                          Earning = trd.CompanyTokensEarnedChange,
                          Spending = trd.CompanyTokensSpentChange
                      }).ToArray();


            if (!ms.Any())
            {
                return null;
            }

            return new ReportTokensSegmentMetricsDTO
            {
                StartDateId = ms.First().DateId,
                EndDateId = ms.Last().DateId,
                TokensEarnedTotal = tm.TokensEarnedTotal,
                TokensSpentTotal = tm.TokensSpentTotal,
                Earnings = ms.Select(x => x.Earning).ToArray(),
                Spendings = ms.Select(x => x.Spending).ToArray()
            };
        }

        public ReportTokensTeamMetricsDTO GetTokensTeamMetricsReport(ReportAggregationMethods aggrType, ReportTimeIntervals timeInterval, ReportParams reportParams)
        {
            int i = GetTimeIntervalModifier(timeInterval);

            var teamQ = from trw in DbContext.TeamReportsWeekly
                        where trw.DateId >= reportParams.From && trw.DateId <= reportParams.To
                        && trw.MembersCountTotal > 0
                        where trw.SegmentId == reportParams.SegmentId
                        group trw by trw.TeamId;

            var dataQ = from trd in DbContext.SegmentReportsWeekly
                        where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                        where trd.SegmentId == reportParams.SegmentId
                        group trd by trd.DateId / i;

            ReportTokensTeamMetricsDTO.TeamDTO[] teamsSummary = null;
            ReportTokensTeamMetricsDTO.DataDTO[] teamsData = null;
            if (aggrType == ReportAggregationMethods.Average)
            {
                teamsSummary = teamQ.Select(g => new ReportTokensTeamMetricsDTO.TeamDTO
                {
                    TeamId = g.Key,
                    TokensEarned = g.Average(x => x.CompanyTokensEarnedChange),
                    TokensSpent = g.Average(x => x.CompanyTokensSpentChange)
                }).ToArray();

                teamsData = dataQ.Select(g => new ReportTokensTeamMetricsDTO.DataDTO
                {
                    DateId = g.Key * i + (i == 1 ? 0 : 1),
                    Value = g.Average(x => x.CompanyTokensEarnedChange)
                }).ToArray();
            }
            else if (aggrType == ReportAggregationMethods.Sum)
            {
                teamsSummary = teamQ.Select(g => new ReportTokensTeamMetricsDTO.TeamDTO
                {
                    TeamId = g.Key,
                    TokensEarned = g.Sum(x => x.CompanyTokensEarnedChange),
                    TokensSpent = g.Sum(x => x.CompanyTokensSpentChange)
                }).ToArray();

                teamsData = dataQ.Select(g => new ReportTokensTeamMetricsDTO.DataDTO
                {
                    DateId = g.Key * i + (i == 1 ? 0 : 1),
                    Value = g.Sum(x => x.CompanyTokensEarnedChange)
                }).ToArray();
            }

            if (!teamsSummary.Any() && !teamsData.Any())
            {
                return null;
            }

            return new ReportTokensTeamMetricsDTO
            {
                Teams = teamsSummary,
                Data = teamsData
            };
        }

        public ReportItemsSegmentMetricsDTO GetItemsSegmentMetricsReport(ReportParams reportParams)
        {
            var stats = (from trw in DbContext.SegmentReportsWeekly
                         where trw.DateId >= reportParams.From && trw.DateId <= reportParams.To
                         where trw.SegmentId == reportParams.SegmentId
                         group trw by 1 into g
                         select new
                         {
                             Created = g.Sum(x => x.ItemsCreatedChange),
                             Gifted = g.Sum(x => x.ItemsGiftedChange),
                             Dissed = g.Sum(x => x.ItemsDisenchantedChange)
                         }).FirstOrDefault();

            var wr = (from trw in DbContext.SegmentReportsWeekly
                      where trw.DateId >= reportParams.From && trw.DateId <= reportParams.To
                      where trw.SegmentId == reportParams.SegmentId
                      orderby trw.DateId ascending
                      select new
                      {
                          DateId = trw.DateId,
                          Bought = trw.ItemsBoughtChange,
                          BoughtAvg = trw.ItemsBoughtChange / trw.MembersCountTotal
                      }).ToList();

            if(!wr.Any())
            {
                return null;
            }

            return new ReportItemsSegmentMetricsDTO
            {
                StartDateId = wr.First().DateId,
                EndDateId = wr.Last().DateId,

                ItemsCreated = stats.Created,
                ItemsGifted = stats.Gifted,
                ItemsDisenchanted = stats.Dissed,
                Purchases = wr.Select(x => new ReportItemsSegmentMetricsDTO.PurchasesDTO
                {
                    Total = x.Bought,
                    Average = x.BoughtAvg
                }).ToArray()
            };
        }

        public ReportItemsTeamMetricsDTO GetItemTeamMetricsReport(int teamId, ReportParams reportParams)
        {
            //TODO: make this secure
            var profileIds = DbContext.ProfileAssignments.Where(x => x.TeamId == teamId).Select(x => x.ProfileId).ToArray();

            var ms = (from prw in DbContext.ProfileReportsWeekly
                      where prw.DateId >= reportParams.From && prw.DateId <= reportParams.To
                      where prw.ProfileRole == ProfileRoles.Member && profileIds.Contains(prw.ProfileId)
                      orderby prw.DateId ascending
                      group prw by prw.ProfileId into pr
                      select new ReportItemsTeamMetricsDTO.MemberDTO
                      {
                          ProfileId = pr.Key,
                          Name = pr.FirstOrDefault().Profile.FirstName + " " + pr.FirstOrDefault().Profile.LastName,
                          InventoryValue = pr.OrderByDescending(x => x.DateId).Select(x => x.InventoryValueTotal).FirstOrDefault(),
                          InventoryCount = pr.OrderByDescending(x => x.DateId).Select(x => x.InventoryCountTotal).FirstOrDefault(),
                          DisenchantCount = pr.Sum(x => x.ItemsDisenchantedChange),
                          GiftedCount = pr.Sum(x => x.ItemsDisenchantedChange)
                      }).ToArray();

            if(!ms.Any())
            {
                return null;
            }

            return new ReportItemsTeamMetricsDTO
            {
                InventoryValueAverage = ms.Sum(x => x.InventoryValue) / ms.Count(),
                Members = ms
            };
        }

        #endregion

        #region Private Methods

        private static int GetTimeIntervalModifier(ReportTimeIntervals i)
        {
            switch(i)
            {
                case ReportTimeIntervals.Month:
                    return 100;

                default:return 1;
            }
        }

        #endregion
    }
}