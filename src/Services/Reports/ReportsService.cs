using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
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
                      group trw by 1 into r
                      select new 
                      {
                          OImpactAverage = r.Average(x => x.OImpactAverage),
                          SpeedAverage = r.Average(x => x.SpeedAverage),
                          HeatAverage = r.Average(x => x.HeatAverageTotal)
                      }).FirstOrDefault();

            return new ReportOverviewDTO
            {
                Categories = new ReportOverviewDTO.CategoryDTO[]
                {
                    new ReportOverviewDTO.CategoryDTO
                    {
                        Id = 1,
                        Name = "OImpact",
                        MaxValue = 30,
                        AverageValue = avg?.OImpactAverage ?? 0
                    },
                    new ReportOverviewDTO.CategoryDTO
                    {
                        Id = 1,
                        Name = "Speed",
                        MaxValue = 30,
                        AverageValue = avg?.SpeedAverage ?? 0
                    },
                    new ReportOverviewDTO.CategoryDTO
                    {
                        Id = 3,
                        Name = "Heat",
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
                                  Id = 1,
                                  Value = r.Average(a => a.OImpactAverage)
                              },
                              new ReportOverviewDTO.NodeDTO.DataDTO
                              {
                                  Id = 2,
                                  Value = r.Average(a => a.SpeedAverage)
                              },
                              new ReportOverviewDTO.NodeDTO.DataDTO
                              {
                                  Id = 3,
                                  Value = r.Average(a => a.HeatAverageTotal)
                              }
                          }
                      }).ToArray()
            };
        }

        public ReportTeamPerformanceChartDTO GetReportTeamPerformanceChartDTO(int teamId, int periodInDays)
        {
            var teamProfiles = DbContext.TeamMembers.Where(x => x.TeamId == teamId).Select(x => x.ProfileId).ToList();
            var lastReportCreatedAt = DbContext.TeamReportsWeekly.OrderByDescending(x => x.DateId).Select(x => x.DateId).FirstOrDefault();

            var dataset = (from pr in DbContext.ProfileReportsDaily
                           where teamProfiles.Contains(pr.ProfileId)
                           where pr.DateId > DateHelper2.AddDays(lastReportCreatedAt, -periodInDays)
                           orderby pr.DateId
                           group pr by pr.ProfileId into p
                           select new ReportTeamPerformanceChartDTO.Dataset
                           {
                               ProfileId = p.Key,
                               FullName = $"{p.First().Profile.FirstName} {p.First().Profile.LastName}",
                               Avatar = p.First().Profile.Avatar,
                               //AverageScores = p.Select(x => x.AverageScore).ToArray()
                           })
                                .ToList();

            dataset.ForEach(x => x.AverageScores = x.AverageScores.EnsureSize(periodInDays).Select(s => Math.Round(s, 2)).ToArray());

            return new ReportTeamPerformanceChartDTO
            {
                Labels = DateHelper2.CreateDayOfWeekList(lastReportCreatedAt, periodInDays),
                ProfilesDataset = dataset
            };
        }

        public ReportTeamsPerformanceChartDTO GetReportTeamsPerformanceChartDTO(int segmentId, int periodInDays)
        {
            var segmentTeamsIds = DbContext.TeamsScopeOfSegment(segmentId).Select(x => x.Id).ToList();
            var lastReportCreatedAt = DbContext.TeamReportsWeekly.OrderByDescending(x => x.DateId).Select(x => x.DateId).FirstOrDefault();

            var dataset = (from tr in DbContext.TeamReportsWeekly
                           where segmentTeamsIds.Contains(tr.TeamId)
                           where tr.DateId > DateHelper2.AddDays(lastReportCreatedAt, -periodInDays)
                           orderby tr.DateId
                           group tr by tr.TeamId into t
                           select new ReportTeamsPerformanceChartDTO.Dataset
                           {
                               TeamId = t.Key,
                               TeamName = t.First().Team.Name,
                               //AverageScores = t.Select(x => x.ScoreAverage).ToArray()
                           })
                            .ToList();

            dataset.ForEach(x => x.AverageScores = x.AverageScores.EnsureSize(periodInDays).Select(s => Math.Round(s, 2)).ToArray());

            return new ReportTeamsPerformanceChartDTO
            {
                Labels = DateHelper2.CreateDayOfWeekList(lastReportCreatedAt, periodInDays),
                Datasets = dataset
            };
        }

        public IList<ReportTeamsCompletedTasksChartDTO> GetReportTeamsCompletedTasksChartDTO(int segmentId)
        {
            var segmentTeamsIds = DbContext.TeamsScopeOfSegment(segmentId).Select(x => x.Id).ToList();
            var lastReportCreatedAt = DbContext.TeamReportsDaily.OrderByDescending(x => x.DateId).Select(x => x.DateId).FirstOrDefault();

            return (from tr in DbContext.TeamReportsDaily
                    where segmentTeamsIds.Contains(tr.TeamId)
                    where tr.DateId == lastReportCreatedAt
                    select new ReportTeamsCompletedTasksChartDTO
                    {
                        TeamId = tr.TeamId,
                        TeamName = tr.Team.Name,
                        TeamAvatar = tr.Team.AvatarColor,
                        TasksCompletedTotal = tr.TasksCompletedTotal,
                    }).ToList();
        }

        #endregion
    }
}