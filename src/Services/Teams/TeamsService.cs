using System;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;
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
                    TimeWorked =  r.TasksCompletionTimeTotal,
                    TokensEarned =  r.CompanyTokensEarnedTotal,
                    TokensSpent =  r.CompanyTokensSpentTotal,
                    ItemsBought = r.ItemsBoughtTotal,
                    QuestsCompleted = r.QuestsCompletedTotal,
                    DaysOnTayra = EF.Functions.DateDiffDay(team.Created, DateTime.UtcNow)
                }).LastOrDefault();
        }
        
        public TeamSwarmPlotDTO GetTeamSwarmPloteDTO(string teamKey)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey);
            team.EnsureNotNull(teamKey);
            
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
                Metrics = new[]
                {
                    new TeamSwarmPlotDTO.DataDTO
                    {
                        MetricType = MetricTypes.OImpact,
                        Averages = teamStats.Select(x => x.Impact).ToArray(),
                        ProfileStats = profileStats.ToLookup(x => x.ProfileId)
                            .ToDictionary(x => x.Key, x => x.Select(s => s.Impact).ToArray())
                    },
                    new TeamSwarmPlotDTO.DataDTO
                    {
                        MetricType = MetricTypes.Speed,
                        Averages = teamStats.Select(x => x.Speed).ToArray(),
                        ProfileStats = profileStats.ToLookup(x => x.ProfileId)
                            .ToDictionary(x => x.Key, x => x.Select(s => s.Speed).ToArray())
                    },
                    new TeamSwarmPlotDTO.DataDTO
                    {
                        MetricType = MetricTypes.Heat,
                        Averages = new float[]{},
                        ProfileStats = profileStats.ToLookup(x => x.ProfileId)
                            .ToDictionary(x => x.Key, x => x.Select(s => s.Heat).ToArray())
                    },
                    new TeamSwarmPlotDTO.DataDTO
                    {
                        MetricType = MetricTypes.Power,
                        Averages = teamStats.Select(x => x.Power).ToArray(),
                        ProfileStats = profileStats.ToLookup(x => x.ProfileId)
                            .ToDictionary(x => x.Key, x => x.Select(s => s.Power).ToArray())
                    },
                    new TeamSwarmPlotDTO.DataDTO
                    {
                        MetricType = MetricTypes.Assist,
                        Averages = teamStats.Select(x => (float)x.Assists).ToArray(),
                        ProfileStats = profileStats.ToLookup(x => x.ProfileId)
                            .ToDictionary(x => x.Key, x => x.Select(s => s.Assists).ToArray())
                    },
                    new TeamSwarmPlotDTO.DataDTO
                    {
                        MetricType = MetricTypes.WorkUnitsCompleted,
                        Averages = teamStats.Select(x => x.Completion).ToArray(),
                        ProfileStats = profileStats.ToLookup(x => x.ProfileId)
                            .ToDictionary(x => x.Key, x => x.Select(s => s.Completion).ToArray())
                    },
                    new TeamSwarmPlotDTO.DataDTO
                    {
                        MetricType = MetricTypes.Complexity,
                        Averages = teamStats.Select(x => x.Complexity).ToArray(),
                        ProfileStats = profileStats.ToLookup(x => x.ProfileId)
                            .ToDictionary(x => x.Key, x => x.Select(s => s.Complexity).ToArray())
                    },
                }
            };
        }

        public GridData<TeamViewGridDTO> GetViewGridData(int[] segmentIds, TeamViewGridParams gridParams)
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

        public void Create(int segmentId, TeamCreateDTO dto)
        {
            DbContext.Add(new Team
            {
                SegmentId = segmentId,
                Key = dto.Key.Trim(),
                Name = dto.Name.Trim(),
                AvatarColor = dto.AvatarColor
            });
        }

        public void Update(int teamId, TeamUpdateDTO dto)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Id == teamId);

            team.EnsureNotNull(teamId);

            team.Key = dto.Key.Trim();
            team.Name = dto.Name.Trim();
            team.AvatarColor = dto.AvatarColor;
        }

        public void Archive(int profileId, string teamKey)
        {
            var team = DbContext.Teams.Include(x => x.Members).FirstOrDefault(x => x.Key == teamKey);

            team.EnsureNotNull(team.Key);

            DbContext.Remove(team);

            foreach (var m in team.Members) //is this needed?
            {
                DbContext.Remove(m);
            }
        }
        
        public TeamStatsDTO GetTeamStatsData(string teamKey)
        {
            var latestUpdateDateId = DateHelper.FindPeriod(DateRanges.Last4Week).FromId;

            var team = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey);
            
            team.EnsureNotNull(teamKey);
            
            var otherTeams = DbContext.Teams.Where(x => x.Key != teamKey).Select(x => x.Id).ToArray();

            var otherTeamsStats =
                DbContext.TeamReportsWeekly
                    .Where(x => otherTeams.Contains(x.TeamId) && x.DateId >= latestUpdateDateId)
                    .ToLookup(x => x.TeamId).ToDictionary(x => x.Key, x => new 
                    {
                        Impact = x.Select(r => r.OImpactAverage).ToArray(),
                        Speed = x.Select(r => r.SpeedAverage).ToArray(),
                        Power = x.Select(r => r.PowerAverage).ToArray(),
                        Heat = x.Select(r => r.HeatAverageTotal).ToArray(),
                        Assists = x.Select(r => (float) r.AssistsChange).ToArray(),
                        TaskCompletion = x.Select(r => (float) r.TasksCompletedChange).ToArray(),
                        Complexity = x.Select(r => (float) r.ComplexityChange).ToArray(),
                    });
            
            return (from trw in DbContext.TeamReportsWeekly
                where trw.TeamId == team.Id
                where trw.DateId >= latestUpdateDateId
                group trw by 1
                into r
                select new TeamStatsDTO
                {
                    LatestUpdateDateId = latestUpdateDateId,
                    Metrics = (new TeamStatsDTO.TeamMetricDTO[]
                    {
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.OImpact,
                            TeamsAverages = otherTeamsStats.Select(x => new TeamStatsDTO.TeamMetricDTO.OtherTeamsAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Impact,
                                TotalAverage = x.Value.Impact.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => x.OImpactAverage).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Speed,  
                            TeamsAverages = otherTeamsStats.Select(x => new TeamStatsDTO.TeamMetricDTO.OtherTeamsAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Speed,
                                TotalAverage = x.Value.Speed.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => x.SpeedAverage).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Power,  
                            TeamsAverages = otherTeamsStats.Select(x => new TeamStatsDTO.TeamMetricDTO.OtherTeamsAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Power,
                                TotalAverage = x.Value.Power.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => x.PowerAverage).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Heat,
                            TeamsAverages = otherTeamsStats.Select(x => new TeamStatsDTO.TeamMetricDTO.OtherTeamsAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Heat,
                                TotalAverage = x.Value.Heat.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => x.HeatAverageTotal).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Complexity,
                            TeamsAverages = otherTeamsStats.Select(x => new TeamStatsDTO.TeamMetricDTO.OtherTeamsAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Complexity,
                                TotalAverage = x.Value.Complexity.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => (float) x.ComplexityChange).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Assist, 
                            TeamsAverages = otherTeamsStats.Select(x => new TeamStatsDTO.TeamMetricDTO.OtherTeamsAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.Assists,
                                TotalAverage = x.Value.Assists.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => (float) x.AssistsChange).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.WorkUnitsCompleted,
                            TeamsAverages = otherTeamsStats.Select(x => new TeamStatsDTO.TeamMetricDTO.OtherTeamsAveragesDTO
                            {
                                Id = x.Key ,
                                Averages = x.Value.TaskCompletion,
                                TotalAverage = x.Value.TaskCompletion.Sum() / 4f
                            }).ToArray(),
                            WeeklyAverages = r.Select(x => (float) x.TasksCompletedChange).ToArray()
                        }
                    }).ToArray()
                }).FirstOrDefault();
        }

        public TeamPulseDTO GetTeamPulse(string teamKey)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey);
            
            team.EnsureNotNull(teamKey);

            var teamMembers = DbContext.ProfileAssignments.Where(x => x.TeamId == team.Id).Select(x => x.ProfileId)
                .ToArray();

            var yesterdayDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(-1));
            
            return (from t in DbContext.Tasks
                where teamMembers.Contains(t.AssigneeProfileId.Value)
                where t.Status == TaskStatuses.InProgress || (t.Status == TaskStatuses.Done && t.LastModifiedDateId >= yesterdayDateId)
                group t by 1 into g
                select new TeamPulseDTO
                {
                    InProgress = g.Where(x => x.Status == TaskStatuses.InProgress).Count(),
                    RecentlyDone = g.Where(x => x.Status == TaskStatuses.Done).Count()
                }).FirstOrDefault();
        }
        
        #endregion

        #region Private Methods


        #endregion
    }
}
