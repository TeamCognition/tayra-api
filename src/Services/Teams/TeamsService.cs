using System;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Connectors.Atlassian;
using Tayra.Models.Organizations;
using Tayra.Models.Organizations.Metrics;
using DateRanges = Cog.Core.DateRanges;

namespace Tayra.Services
{
    public class TeamsService : BaseService<OrganizationDbContext>, ITeamsService
    {
        #region Constructor

        public TeamsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public TeamViewDTO GetTeamViewDTO(string teamKey)
        {
            var teamDto = (from t in DbContext.Teams
                           where t.Key == teamKey
                           select new TeamViewDTO
                           {
                               TeamId = t.Id,
                               TeamKey = t.Key,
                               Name = t.Name,
                               AvatarColor = t.AvatarColor,
                               AssistantSummary = t.AssistantSummary,
                               Created = t.Created,
                           }).FirstOrDefault();

            teamDto.EnsureNotNull(teamKey);

            return teamDto;
        }
        public TeamRawScoreDTO GetTeamRawScoreDTO(string teamKey)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey);
            team.EnsureNotNull(teamKey);

            return (from r in DbContext.TeamReportsDaily
                    where r.TeamId == team.Id
                    select new TeamRawScoreDTO
                    {
                        TasksCompleted = r.TasksCompletedTotal,
                        AssistsGained = r.AssistsTotal,
                        TimeWorked = r.TasksCompletionTimeTotal,
                        TokensEarned = r.CompanyTokensEarnedTotal,
                        TokensSpent = r.CompanyTokensSpentTotal,
                        ItemsBought = r.ItemsBoughtTotal,
                        QuestsCompleted = r.QuestsCompletedTotal,
                        DaysOnTayra = EF.Functions.DateDiffDay(team.Created, DateTime.UtcNow)
                    }).LastOrDefault();
        }

        public TeamSwarmPlotDTO GetTeamSwarmPloteDTO(string teamKey)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey);
            team.EnsureNotNull(teamKey);

            var metricService = new MetricService(DbContext);

            var metricList = new MetricType[]
            {
                MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists, MetricType.Heat,
                MetricType.TasksCompleted, MetricType.Complexity
            };

            var teamProfiless = DbContext.ProfileAssignments.Where(x => x.TeamId == team.Id && x.Profile.IsAnalyticsEnabled).Select(x => x.ProfileId)
                .ToArray();

            var teamStats = (from r in DbContext.TeamReportsWeekly
                             where r.TeamId == team.Id
                             orderby r.DateId descending
                             select new
                             {
                                 DateId = r.DateId,
                                 Impact = r.OImpactAverage,
                                 Speed = r.SpeedAverage,
                                 Power = r.PowerAverage,
                                 Assists = r.AssistsChange,
                                 Completion = r.ComplexityAverage,
                                 Complexity = r.ComplexityAverage
                             }).Take(4).ToArray();

            if (teamStats.Length == 0)
                return null;

            var p = DateHelper.FindPeriod(DateRanges.Last4Week);

            var teamProfiles = DbContext.ProfileAssignments.Where(x => x.TeamId == team.Id).Select(x => x.ProfileId).ToArray();

            var profileStats = (from r in DbContext.ProfileReportsWeekly
                                where teamProfiles.Contains(r.ProfileId)
                                where r.DateId >= p.FromId
                                orderby r.DateId descending
                                select new
                                {
                                    ProfileId = r.ProfileId,
                                    Impact = r.OImpactAverage,
                                    Speed = r.SpeedAverage,
                                    Heat = r.Heat,
                                    Power = r.PowerAverage,
                                    Assists = r.AssistsTotalAverage,
                                    Completion = r.ComplexityTotalAverage,
                                    Complexity = r.ComplexityTotalAverage
                                }).ToArray();

            return new TeamSwarmPlotDTO
            {
                LastUpdateDateId = teamStats.Select(x => x.DateId).FirstOrDefault(),

                ProfileMetrics = metricService.GetMetricsRanks(metricList, teamProfiless, EntityTypes.Profile,
                new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow)),
                Averages = metricService.GetMetrics(metricList, team.Id, EntityTypes.Team,
                new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow))
            };
        }
        public GridData<TeamViewGridDTO> GetViewGridData(Guid[] segmentIds, TeamViewGridParams gridParams)
        {
            //this query is garbo
            IQueryable<TeamViewGridDTO> query = from s in DbContext.Segments
                                                where segmentIds.Contains(s.Id)
                                                select new TeamViewGridDTO
                                                {
                                                    SegmentId = s.Id,
                                                    Teams = s.Teams.Where(x => x.Key != null).Select(x => new TeamViewGridDTO.TeamDTO
                                                    {
                                                        TeamId = x.Id,
                                                        Key = x.Key,
                                                        Name = x.Name,
                                                        AvatarColor = x.AvatarColor,
                                                        MembersCount = x.Members.Count()
                                                    }).ToArray()
                                                };

            GridData<TeamViewGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<TeamProfilesGridDTO> GetTeamProfilesGridData(TeamProfilesGridParams gridParams)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == gridParams.TeamKey);

            team.EnsureNotNull(gridParams.TeamKey);

            var scope = DbContext.ProfileAssignments
                .Where(x => x.TeamId == team.Id);

            IQueryable<TeamProfilesGridDTO> query = from t in scope
                                                    let ws = t.Profile.StatsWeekly.OrderByDescending(x => x.DateId).Where(x => x.SegmentId == team.SegmentId)
                                                    select new TeamProfilesGridDTO
                                                    {
                                                        ProfileId = t.ProfileId,
                                                        Name = t.Profile.FirstName + " " + t.Profile.LastName,
                                                        Username = t.Profile.Username,
                                                        Avatar = t.Profile.Avatar,
                                                        Speed = Math.Round(ws.Select(x => x.SpeedAverage).FirstOrDefault(), 2),
                                                        Power = Math.Round(ws.Select(x => x.PowerAverage).FirstOrDefault(), 2),
                                                        Impact = Math.Round(ws.Select(x => x.OImpactAverage).FirstOrDefault(), 2),
                                                        MemberFrom = t.Created
                                                    };

            GridData<TeamProfilesGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public void Create(Guid segmentId, TeamCreateDTO dto)
        {
            DbContext.Add(new Team
            {
                SegmentId = segmentId,
                Key = dto.Key.Trim(),
                Name = dto.Name.Trim(),
                AvatarColor = dto.AvatarColor
            });
        }

        public void Update(Guid teamId, TeamUpdateDTO dto)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Id == teamId);

            team.EnsureNotNull(teamId);

            team.Key = dto.Key.Trim();
            team.Name = dto.Name.Trim();
            team.AvatarColor = dto.AvatarColor;
        }

        public void Archive(Guid profileId, string teamKey)
        {
            var team = DbContext.Teams.Include(x => x.Members).FirstOrDefault(x => x.Key == teamKey);

            team.EnsureNotNull(team.Key);

            DbContext.Remove(team);

            foreach (var m in team.Members) //is this needed?
            {
                DbContext.Remove(m);
            }
        }

        public TeamStatsDTO GetTeamStatsData(Guid teamId)
        {
            var metricService = new MetricService(DbContext);

            var metricList = new MetricType[]
            {
                MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists,
                MetricType.TasksCompleted, MetricType.Complexity, MetricType.CommitRate
            };

            var teamMetrics = metricService.GetMetricsWithIterationSplit(
                metricList, teamId, EntityTypes.Team, new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow));

            var teamsSegmentId = DbContext.Teams.FirstOrDefault(x => x.Id == teamId).SegmentId;

            var segmentMetrics = metricService.GetMetricsWithIterationSplit(
                metricList, teamsSegmentId, EntityTypes.Segment, new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow));

            return new TeamStatsDTO
            {
                LastRefreshAt = DbContext.TeamMetrics.OrderByDescending(x => x.DateId).Select(x => x.Created).FirstOrDefault(),
                EntityMetrics = teamMetrics,
                ComparatorMetrics = segmentMetrics
            };
        }

        public TeamPulseDTO GetTeamPulse(string teamKey)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey);

            team.EnsureNotNull(teamKey);

            var teamMembers = DbContext.ProfileAssignments.Where(x => x.TeamId == team.Id).Select(x => x.ProfileId)
                .ToArray();

            var yesterdayDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(-1));

            string jiraBoardUrl = null;
            var segmentId = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey)?.SegmentId;
            if (segmentId != null)
            {
                var sFields = DbContext.Integrations
                    .Where(x => x.SegmentId == segmentId && x.ProfileId == null && x.Type == IntegrationType.ATJ)
                    .Select(x => x.Fields).FirstOrDefault();

                if (sFields != null)
                {
                    var jiraSiteName = sFields.FirstOrDefault(x => x.Key == ATConstants.AT_SITE_NAME)?.Value;
                    jiraBoardUrl = $"https://{jiraSiteName}.atlassian.net/secure/RapidBoard.jspa?rapidView=6";
                }
            }

            return (from t in DbContext.Tasks
                    where teamMembers.Contains(t.AssigneeProfileId.Value)
                    where t.Status == TaskStatuses.InProgress || (t.Status == TaskStatuses.Done && t.LastModifiedDateId >= yesterdayDateId)
                    group t by 1 into g
                    select new TeamPulseDTO
                    {
                        InProgress = g.Count(x => x.Status == TaskStatuses.InProgress),
                        RecentlyDone = g.Count(x => x.Status == TaskStatuses.Done),
                        JiraBoardUrl = jiraBoardUrl
                    }).FirstOrDefault();
        }

        #endregion

        #region Private Methods


        #endregion
    }
}