using System;
using System.Collections.Generic;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
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

        public GridData<TeamViewGridDTO> GetViewGridData(int segmentId, TeamViewGridParams gridParams)
        {
            IQueryable<TeamViewGridDTO> query = from t in DbContext.TeamsScopeOfSegment(segmentId)
                                                select new TeamViewGridDTO
                                                {
                                                    TeamKey = t.Key,
                                                    Name = t.Name,
                                                    Created = t.Created,
                                                    AvatarColor = t.AvatarColor,
                                                    Subtitle = t.Members.Count() + " Members",
                                                };

            GridData<TeamViewGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<TeamMembersGridDTO> GetTeamMembersGridData(TeamMembersGridParams gridParams)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == gridParams.TeamKey);

            team.EnsureNotNull(gridParams.TeamKey);

            var scope = DbContext.TeamMembers
                .Where(x => x.TeamId == team.Id);

            IQueryable<TeamMembersGridDTO> query = from t in scope
                                                   from pdr in DbContext.ProfileReportsWeekly.Where(x => t.ProfileId == x.ProfileId)
                                                   .OrderByDescending(x => x.DateId).Take(1).DefaultIfEmpty()
                                                   select new TeamMembersGridDTO
                                                   {
                                                       Name = t.Profile.FirstName + " " + t.Profile.LastName,
                                                       Username = t.Profile.Username,
                                                       Speed = Math.Round(t.Profile.StatsWeekly.Select(x => x.SpeedAverage).FirstOrDefault(), 2),
                                                       Heat = Math.Round(t.Profile.StatsWeekly.Select(x => x.Heat).FirstOrDefault(), 2),
                                                       Impact = Math.Round(t.Profile.StatsWeekly.Select(x => x.OImpactAverage).FirstOrDefault(), 2),
                                                       MemberFrom = t.Created
                                                   };

            GridData<TeamMembersGridDTO> gridData = query.GetGridData(gridParams);

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

        public void Update(int segmentId, TeamUpdateDTO dto)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == dto.TeamKey);

            team.EnsureNotNull(dto.TeamKey);

            team.SegmentId = dto.NewSegmentId ?? segmentId;
            team.Key = dto.Key.Trim();
            team.Name = dto.Name.Trim();
            team.AvatarColor = dto.AvatarColor;
        }

        public void AddMembers(string teamKey, IList<TeamAddMemberDTO> dto)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey);

            team.EnsureNotNull(teamKey);

            foreach (var m in dto)
            {
                DbContext.TeamMembers.Add(new TeamMember
                {
                    TeamId = team.Id,
                    ProfileId = m.ProfileId,
                });
            }
        }

        public void Archive(int profileId, string teamKey)
        {
            var team = DbContext.Teams.FirstOrDefault(x => x.Key == teamKey);

            team.EnsureNotNull(team.Key);

            DbContext.Remove(team);
        }


        #endregion

        #region Private Methods


        #endregion
    }
}
