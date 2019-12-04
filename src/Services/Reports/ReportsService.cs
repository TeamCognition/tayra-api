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

        public ReportProjectPerformanceChartDTO GetReportProjectPerformanceChartDTO(int projectId)
        {
            return DbContext.ProjectReportsWeekly
                .OrderByDescending(x => x.DateId)
                .Where(x => x.ProjectId == projectId)
                .Select(x => new ReportProjectPerformanceChartDTO
                {
                    //MembersTotal = x.MembersTotal,
                    //ScoreAverage = Math.Round(x.ScoreAverage, 0),
                    //TasksCompletedTotal = x.TasksCompletedTotal,
                    //TokensEarnedTotal = Math.Round(x.TokensEarnedTotal, 0)
                }).FirstOrDefault();
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

        public ReportTeamsPerformanceChartDTO GetReportTeamsPerformanceChartDTO(int projectId, int periodInDays)
        {
            var projectTeams = DbContext.ProjectTeams.Where(x => x.ProjectId == projectId).Select(x => x.TeamId).ToList();
            var lastReportCreatedAt = DbContext.TeamReportsWeekly.OrderByDescending(x => x.DateId).Select(x => x.DateId).FirstOrDefault();

            var dataset = (from tr in DbContext.TeamReportsWeekly
                           where projectTeams.Contains(tr.TeamId)
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

        public IList<ReportTeamsCompletedTasksChartDTO> GetReportTeamsCompletedTasksChartDTO(int projectId)
        {
            var projectTeams = DbContext.ProjectTeams.Where(x => x.ProjectId == projectId).Select(x => x.TeamId).ToList();
            var lastReportCreatedAt = DbContext.TeamReportsDaily.OrderByDescending(x => x.DateId).Select(x => x.DateId).FirstOrDefault();

            return (from tr in DbContext.TeamReportsDaily
                    where projectTeams.Contains(tr.TeamId)
                    where tr.DateId == lastReportCreatedAt
                    select new ReportTeamsCompletedTasksChartDTO
                    {
                        TeamId = tr.TeamId,
                        TeamName = tr.Team.Name,
                        TeamAvatar = tr.Team.Avatar,
                        TasksCompletedTotal = tr.TasksCompletedTotal,
                    }).ToList();
        }

        #endregion
    }
}