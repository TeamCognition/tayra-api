using System;
using System.Linq;
using Firdaws.Core;
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

        public ReportOverviewDTO GetOverviewReport(ReportParams reportParams)
        {
            var avg = (from trw in DbContext.SegmentReportsWeekly
                      where trw.DateId >= reportParams.From && trw.DateId <= reportParams.To
                      where trw.SegmentId == reportParams.SegmentId
                      //orderby trw.DateId ascending
                      group trw by 1 into r
                      select new 
                      {
                          OImpactAverage = r.Average(x => x.OImpactAverage),
                          SpeedAverage = r.Average(x => x.SpeedAverage),
                          HeatAverage = r.Average(x => x.HeatAverageTotal)
                      }).FirstOrDefault();

            return new ReportOverviewDTO
            {
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
                        Id = MetricTypes.Heat,
                        MaxValue = 40,
                        AverageValue = avg?.HeatAverage ?? 0
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
                                  Id = MetricTypes.Heat,
                                  Value = r.Average(a => a.HeatAverageTotal)
                              }
                          }
                      }).ToArray()
            };
        }

        public ReportDeliverySegmentMetricsDTO GetDeliverySegmentMetricsReport(ReportParams reportParams)
        {
            var teamIds = DbContext.Teams.Where(x => x.SegmentId == reportParams.SegmentId).Select(x => x.Id);

            var wr = (from trw in DbContext.TeamReportsWeekly
                      where trw.DateId >= reportParams.From && trw.DateId <= reportParams.To
                      where teamIds.Contains(trw.TeamId)
                      orderby trw.DateId ascending
                      select new
                      {
                          trw.TeamId,
                          trw.DateId,
                          MinutesSpentAverage = trw.TasksCompletedChange != 0 ? trw.TasksCompletionTimeChange / trw.TasksCompletedChange : 0
                      }).ToArray();

            if(wr == null)
            {
                return null;
            }

            var startDateId = wr.First().DateId;
            var endDateId = wr.Last().DateId;
            var weeks = wr.Length; //((endDateId - startDateId) / 7) + 1;
            return new ReportDeliverySegmentMetricsDTO
            {
                StartDateId = startDateId,
                EndDateId = endDateId,
                Teams = teamIds.Select(tId => new ReportDeliverySegmentMetricsDTO.TeamDTO
                {
                    TeamId = tId,
                    AverageTaskCompletionTime = wr.Where(x => x.TeamId == tId).Select(x => x.MinutesSpentAverage).ToArray().EnsureSize(weeks)
                }).ToArray()
            };
        }

        public ReportDeliveryTeamMetricsDTO GetDeliveryTeamMetricsReport(int teamId, ReportParams reportParams)
        {
            var tm = (from trd in DbContext.TeamReportsWeekly
                      where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                      where trd.TeamId == teamId
                      group trd by 1 into g
                      select new
                      {
                          SpeedAverage = g.Average(x => x.SpeedAverage),
                          ComplexityAverage = g.Average(x => x.ComplexityAverage)
                      }).FirstOrDefault();


            var ms = (from trd in DbContext.TeamReportsDaily
                      where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                      where trd.TeamId == teamId
                      orderby trd.DateId ascending
                      select new
                      {
                          DateId = trd.DateId,
                          MinutesSpentAverage = trd.TasksCompletedChange != 0 ? trd.TasksCompletionTimeChange / trd.TasksCompletedChange : 0
                      }).ToArray();

            var tasks = (from trd in DbContext.Tasks
                         where trd.LastModifiedDateId >= reportParams.From && trd.LastModifiedDateId <= reportParams.To
                         where trd.TeamId == teamId
                         select new
                         {
                             DateId = trd.LastModifiedDateId,
                             Name = trd.Summary,
                             MinutesSpent = trd.TimeSpentInMinutes.Value, //check for autoTimeSpentinminutes
                             Complexity = trd.Complexity
                         }).ToArray();

            return new ReportDeliveryTeamMetricsDTO
            {
                StartDateId = ms.First().DateId,
                EndDateId = ms.Last().DateId,
                ComplexityAverage = tm.ComplexityAverage,
                SpeedAverage = tm.SpeedAverage,
                Data = ms.Select(x => new ReportDeliveryTeamMetricsDTO.DataDTO
                {
                    AverageTimeCompletionTime = x.DateId,
                    Tasks = tasks.Where(t => t.DateId == x.DateId).Select(t => new ReportDeliveryTeamMetricsDTO.DataDTO.TaskDTO
                    {
                        Name = t.Name,
                        MinutesSpent = t.MinutesSpent,
                        Complexity = t.Complexity
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
                          Heat = trw.HeatAverageTotal,
                          Complexity = trw.ComplexityAverage,
                          Assists = trw.AssistsAverage,
                          TasksCompleted = trw.TasksCompletedAverage
                      }).ToList();

            return new ReportStatisticsSegmentMetricsDTO
            {
                Metrics = new[]
                {
                    new ReportStatisticsSegmentMetricsDTO.MetricDTO { MetricId = MetricTypes.OImpact, Data = wr.Select(x => new ReportStatisticsSegmentMetricsDTO.MetricDTO.DataDTO { DateId = x.DateId, Average = x.OImpact }).ToArray() },
                    new ReportStatisticsSegmentMetricsDTO.MetricDTO { MetricId = MetricTypes.Speed, Data = wr.Select(x => new ReportStatisticsSegmentMetricsDTO.MetricDTO.DataDTO { DateId = x.DateId, Average = x.Speed }).ToArray() },
                    new ReportStatisticsSegmentMetricsDTO.MetricDTO { MetricId = MetricTypes.Heat, Data = wr.Select(x => new ReportStatisticsSegmentMetricsDTO.MetricDTO.DataDTO { DateId = x.DateId, Average = x.Heat }).ToArray() },
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
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.Heat, Average = pr.Average(x => x.Heat) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.Complexity, Average = pr.Average(x => x.ComplexityChange) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.Assist, Average = pr.Average(x => x.AssistsChange) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.TaskCompletion, Average = pr.Average(x => x.TasksCompletedChange) },
                          }
                      }).ToArray();

            return new ReportStatisticsTeamMetricsDTO
            {
                Members = ms
            };
        }

        public ReportTokensSegmentMetricsDTO GetTokensSegmentMetricsReport(ReportParams reportParams)
        {
            var tm = (from trd in DbContext.SegmentReportsWeekly
                      where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                      where trd.SegmentId == reportParams.SegmentId
                      group trd by 1 into g
                      select new
                      {
                          TokensEarnedAverage = g.Average(x => x.CompanyTokensEarnedChange),
                          TokensSpentAverage = g.Average(x => x.CompanyTokensSpentChange)
                      }).FirstOrDefault();


            var ms = (from trd in DbContext.SegmentReportsDaily
                      where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                      where trd.SegmentId == reportParams.SegmentId
                      orderby trd.DateId ascending
                      select new
                      {
                          DateId = trd.DateId,
                          Earning = trd.CompanyTokensEarnedChange
                      }).ToArray();


            return new ReportTokensSegmentMetricsDTO
            {
                StartDateId = ms.First().DateId,
                EndDateId = ms.Last().DateId,
                TokensEarnedAverage = tm.TokensEarnedAverage,
                TokensSpentAverage = tm.TokensSpentAverage,
                Earnings = ms.Select(x => x.Earning).ToArray()
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
                          InventoryValue = pr.LastOrDefault().InventoryValueTotal,
                          InventoryCount = pr.LastOrDefault().InventoryCountTotal,
                          DisenchantCount = pr.Sum(x => x.ItemsDisenchantedChange),
                          GiftedCount = pr.Sum(x => x.ItemsDisenchantedChange)
                      }).ToArray();

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

                default: return 1;
            }
        }

        #endregion
    }
}