using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;

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
                               Created = t.Created
                           }).FirstOrDefault();

            teamDto.EnsureNotNull(teamKey);

            return teamDto;
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
                .Where(x => x.TeamId == team.Id);

            IQueryable<TeamMembersGridDTO> query = from t in scope
                                                   from pdr in DbContext.ProfileReportsWeekly.Where(x => t.ProfileId == x.ProfileId)
                                                   .OrderByDescending(x => x.DateId).Take(1).DefaultIfEmpty()
                                                   select new TeamMembersGridDTO
                                                   {
                                                       ProfileId = t.ProfileId,
                                                       Name = t.Profile.FirstName + " " + t.Profile.LastName,
                                                       Username = t.Profile.Username,
                                                       Avatar = t.Profile.Avatar,
                                                       Speed = Math.Round(t.Profile.StatsWeekly.Select(x => x.SpeedAverage).FirstOrDefault(), 2),
                                                       Power = Math.Round(t.Profile.StatsWeekly.Select(x => x.PowerAverage).FirstOrDefault(), 2),
                                                       Impact = Math.Round(t.Profile.StatsWeekly.Select(x => x.OImpactAverage).FirstOrDefault(), 2),
                                                       MemberFrom = t.Created
                                                   };

            GridData<TeamMembersGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public TeamImpactPieChartDTO GetImpactPieChart(int teamId)
        {
            var lastDateId = DbContext.ProfileReportsWeekly.OrderByDescending(x => x.DateId).Select(x => x.DateId).FirstOrDefault();

            var profileIds = DbContext.ProfileAssignments.Where(x => x.TeamId == teamId).Select(x => x.ProfileId).ToArray();

            if (lastDateId == 0)
                return null;

            var pr = (from prw in DbContext.ProfileReportsWeekly
                      where profileIds.Contains(prw.ProfileId)
                      && prw.DateId == lastDateId
                      select new
                      {
                          Username = prw.Profile.Username,
                          FirstName = prw.Profile.FirstName,
                          LastName = prw.Profile.LastName,
                          OImpact = prw.OImpactAverage
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


        #endregion

        #region Private Methods


        #endregion
    }
}
