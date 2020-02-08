using System;
using System.Linq;
using Firdaws.Core;
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
                      orderby trw.DateId ascending
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
                          Name = r.First().Team.Name,
                          Data = new ReportOverviewDTO.NodeDTO.DataDTO[]
                          {
                              new ReportOverviewDTO.NodeDTO.DataDTO
                              {
                                  MetricId = MetricTypes.OImpact,
                                  Value = r.Average(a => a.OImpactAverage)
                              },
                              new ReportOverviewDTO.NodeDTO.DataDTO
                              {
                                  MetricId = MetricTypes.Speed,
                                  Value = r.Average(a => a.SpeedAverage)
                              },
                              new ReportOverviewDTO.NodeDTO.DataDTO
                              {
                                  MetricId = MetricTypes.Heat,
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
                          MinutesSpentAverage = trw.TasksCompletionTimeChange / trw.TasksCompletedChange
                      }).ToList();

            if(wr == null)
            {
                return null;
            }

            var startDateId = wr.First().DateId;
            var endDateId = wr.Last().DateId;
            var weeks = ((endDateId - startDateId) / 7) + 1;
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
                      where trd.SegmentId == reportParams.SegmentId
                      group trd by 1 into g
                      select new
                      {
                          SpeedAverage = g.Average(x => x.SpeedAverage),
                          ComplexityAverage = g.Average(x => x.ComplexityAverage)
                      }).FirstOrDefault();


            var ms = (from trd in DbContext.TeamReportsDaily
                      where trd.DateId >= reportParams.From && trd.DateId <= reportParams.To
                      where trd.SegmentId == reportParams.SegmentId
                      orderby trd.DateId ascending
                      select new
                      {
                          DateId = trd.DateId,
                          MinutesSpentAverage = trd.TasksCompletionTimeChange / trd.TasksCompletedChange
                      }).ToArray();

            var tasks = (from trd in DbContext.Tasks
                         where trd.LastModifiedDateId >= reportParams.From && trd.LastModifiedDateId <= reportParams.To
                         where trd.SegmentId == reportParams.SegmentId
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
            var profileIds = DbContext.TeamMembers.Where(x => x.TeamId == teamId).Select(x => x.ProfileId).ToArray();

            var ms = (from prw in DbContext.ProfileReportsWeekly
                      where prw.DateId >= reportParams.From && prw.DateId <= reportParams.To
                      where prw.ProfileRole == ProfileRoles.Member && profileIds.Contains(prw.ProfileId)
                      orderby prw.DateId ascending
                      group prw by prw.ProfileId into pr
                      select new ReportStatisticsTeamMetricsDTO.MemberDTO
                      {
                          ProfileId = pr.Key,
                          Name = pr.First().Profile.FirstName + " " + pr.First().Profile.LastName,
                          Avatar = pr.First().Profile.Avatar,
                          Metrics = new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO[]
                          {

                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.OImpact, Average = pr.Average(x => x.OImpactAverage) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.OImpact, Average = pr.Average(x => x.SpeedTotalAverage) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.OImpact, Average = pr.Average(x => x.Heat) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.OImpact, Average = pr.Average(x => x.ComplexityTotalAverage) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.OImpact, Average = pr.Average(x => x.AssistsTotalAverage) },
                              new ReportStatisticsTeamMetricsDTO.MemberDTO.MetricDTO { Id = MetricTypes.OImpact, Average = pr.Average(x => x.TasksCompletedTotalAverage) },
                          }
                      }).ToArray();

            return new ReportStatisticsTeamMetricsDTO
            {
                Members = ms
            };
        }

        #endregion
    }
}