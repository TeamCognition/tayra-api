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
                }).FirstOrDefault();
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
                        MetricType = MetricTypes.TaskCompletion,
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
                                                    Teams = s.Teams.Where(x => x.Key != null /*&& EF.Property<int?>(x, "ArchievedAt") == null*/).Select(x => new TeamViewGridDTO.TeamDTO
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

        public GridData<TeamMembersGridDTO> GetTeamMembersGridData(TeamMembersGridParams gridParams)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == gridParams.TeamKey);

            team.EnsureNotNull(gridParams.TeamKey);

            var scope = DbContext.ProfileAssignments
                .Where(x => x.TeamId == team.Id && x.Profile.Role == ProfileRoles.Member); //role check is maybe unnecessary

            IQueryable<TeamMembersGridDTO> query = from t in scope
                                                   let ws = t.Profile.StatsWeekly.OrderByDescending(x => x.DateId).Where(x => x.SegmentId == team.SegmentId)
                                                   select new TeamMembersGridDTO
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

            GridData<TeamMembersGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public TeamImpactPieChartDTO GetImpactPieChart(int teamId)
        {
            var lastDateId = DbContext.ProfileReportsWeekly.OrderByDescending(x => x.DateId).Select(x => x.DateId).FirstOrDefault();

            if (lastDateId == 0)
                return null;

            var profileIds = DbContext.ProfileAssignments.Where(x => x.TeamId == teamId).Select(x => x.ProfileId).ToArray();

            var pr = (from prw in DbContext.ProfileReportsWeekly
                      where profileIds.Contains(prw.ProfileId)
                      && prw.DateId == lastDateId
                      group prw by prw.ProfileId into g
                      select new
                      {
                          Username = g.First().Profile.Username,
                          FirstName = g.First().Profile.FirstName,
                          LastName = g.First().Profile.LastName,
                          OImpact = g.Sum(x => x.OImpactAverage)
                      }).ToList();

            if (pr == null || !pr.Any())
            {
                return null;
            }

            var impactSum = pr.Sum(x => x.OImpact);

            return new TeamImpactPieChartDTO
            {
                Profiles = pr.Select(x => new TeamImpactPieChartDTO.ProfilesDTO
                {
                    Username = x.Username,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ImpactPercentage = x.OImpact / impactSum * 100
                }).ToArray()
            };
        }

        public TeamImpactLineChartDTO GetImpactLineChart(int teamId)
        {
            var wr = (from trw in DbContext.TeamReportsWeekly
                      where trw.TeamId == teamId
                      orderby trw.DateId descending
                      select new
                      {
                          DateId = trw.DateId,
                          OImpact = trw.OImpactAverage
                      }).Take(30).ToList();

            if (wr == null || !wr.Any())
            {
                return null;
            }

            return new TeamImpactLineChartDTO
            {
                StartDateId = wr.Last().DateId,
                EndDateId = wr.First().DateId,
                Averages = wr.Select(x => x.OImpact).Reverse().ToArray()
            };
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
            var latestUpdateDateId = DateHelper2.ToDateId(DateTime.UtcNow.AddDays(-32));
            
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey);
            team.EnsureNotNull(teamKey);
            
            return (from trw in DbContext.TeamReportsWeekly
                where trw.TeamId == team.Id
                where trw.DateId >= latestUpdateDateId
                group trw by 1 into r
                select new TeamStatsDTO
                {
                    LatestUpdateDateId = latestUpdateDateId,
                    Metrics = (new TeamStatsDTO.TeamMetricDTO[]
                    {
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.OImpact,
                            WeeklyAverages = r.Select(x => x.OImpactAverageTotal).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Speed,
                            WeeklyAverages = r.Select(x => x.SpeedAverageTotal).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Power,
                            WeeklyAverages = r.Select(x => x.PowerAverageTotal).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Heat,
                            WeeklyAverages = r.Select(x => x.HeatAverageTotal).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Complexity,
                            WeeklyAverages = r.Select(x => x.ComplexityAverage).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.Assist,
                            WeeklyAverages = r.Select(x => x.AssistsAverage).ToArray()
                        },
                        new TeamStatsDTO.TeamMetricDTO
                        {
                            Id = MetricTypes.TaskCompletion,
                            WeeklyAverages = r.Select(x => x.TasksCompletedAverage).ToArray()
                        }
                    }).ToArray()
                }).FirstOrDefault();
        }
         
        #endregion

        #region Private Methods


        #endregion
    }
}
