using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class SegmentsService : BaseService<OrganizationDbContext>, ISegmentsService
    {
        #region Constructor

        public SegmentsService(ITeamsService teamsService, OrganizationDbContext dbContext) : base(dbContext)
        {
            TeamsService = teamsService;
        }

        #endregion

        public ITeamsService TeamsService { get; set; }

        #region Public Methods

        public Segment Get(string segmentKey)
        {
            return DbContext.Segments
                .AsNoTracking()
                .FirstOrDefault(i => i.Key == segmentKey);
        }

        public GridData<SegmentGridDTO> GetGridData(int profileId, SegmentGridParams gridParams)
        {
            var query = (from tm in DbContext.TeamMembers
                         where tm.ProfileId == profileId
                         select new SegmentGridDTO
                         {
                            SegmentId = tm.Team.Segment.Id,
                            Name = tm.Team.Segment.Name,
                            Key = tm.Team.Segment.Key,
                            Avatar = tm.Team.Segment.Avatar,
                            Created = tm.Team.Segment.Created
                         }).DistinctBy(x => x.Key);

            GridData<SegmentGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<SegmentMemberGridDTO> GetSegmentMembersGridData(string segmentKey, SegmentMemberGridParams gridParams)
        {
            var teamIds = DbContext.Segments
                .Where(x => x.Key == segmentKey)
                .Select(x => x.Teams.Select(y => y.Id))
                .FirstOrDefault();

            teamIds.EnsureNotNull(segmentKey);

            //var teamIds = segment.Teams.Select(x => x.TeamId).ToList(); //lazy load works?

            IQueryable<SegmentMemberGridDTO> query = from s in DbContext.TeamMembers.Where(x => teamIds.Contains(x.TeamId))
                                                   select new SegmentMemberGridDTO
                                                   {
                                                       Name = s.Profile.FirstName + " " + s.Profile.LastName,
                                                       Username = s.Profile.Username,
                                                       Avatar = s.Profile.Avatar,
                                                       MemberFrom = s.Created
                                                   };

            GridData<SegmentMemberGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<SegmentTeamGridDTO> GetSegmentTeamsGridData(string segmentKey, SegmentTeamGridParams gridParams)
        {
            var segment = DbContext.Segments.FirstOrDefault(x => x.Key == segmentKey);

            segment.EnsureNotNull(segmentKey);

            IQueryable<SegmentTeamGridDTO> query = from t in DbContext.TeamsScopeOfSegment(segment.Id)
                                                    select new SegmentTeamGridDTO
                                                    {
                                                        Name = t.Name,
                                                        Key = t.Key,
                                                        Avatar = t.AvatarColor,
                                                        MemberFrom = t.Created
                                                    };

            GridData<SegmentTeamGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public SegmentViewDTO GetSegmnetViewDTO(string segmentKey)
        {
            var segmentDTO =  (from segment in DbContext.Segments
                                where segment.Key == segmentKey
                                select new SegmentViewDTO
                                {
                                    SegmentId = segment.Id,
                                    Name = segment.Name,
                                    Key = segment.Key,
                                    Avatar = segment.Avatar
                                }).FirstOrDefault();

            segmentDTO.EnsureNotNull(segmentKey);

            return segmentDTO;
        }

        public void AddMember(string segmentKey, SegmentMemberAddRemoveDTO dto)
        {
            var segment = DbContext.Segments.FirstOrDefault(x => x.Key == segmentKey);

            segment.EnsureNotNull(segmentKey);

            var teamScope = DbContext.Teams.Where(x => x.SegmentId == segment.Id);

            var team = dto.TeamId.HasValue
                ? teamScope.FirstOrDefault(x => x.Id == dto.TeamId.Value)
                : teamScope.FirstOrDefault(x => x.Key == null);

            team.EnsureNotNull(dto.TeamId);

            DbContext.TeamMembers.Add(new TeamMember
            {
                TeamId = team.Id,
                ProfileId = dto.ProfileId
            });
        }

        public void RemoveMember(string segmentKey, SegmentMemberAddRemoveDTO dto)
        {
            var segment = DbContext.Segments.FirstOrDefault(x => x.Key == segmentKey);

            segment.EnsureNotNull(segmentKey);

            var teamScope = DbContext.Teams.Where(x => x.SegmentId == segment.Id);

            var team = dto.TeamId.HasValue
                ? teamScope.FirstOrDefault(x => x.Id == dto.TeamId.Value)
                : teamScope.FirstOrDefault(x => x.Key == null);

            DbContext.Remove(DbContext.TeamMembers.FirstOrDefault(x => x.ProfileId == dto.ProfileId && x.TeamId == team.Id));
        }

        public void Create(int profileId, SegmentCreateDTO dto)
        {
            if (!IsSegmentKeyUnique(dto.Key))
            {
                throw new ApplicationException($"A segment exists with the same key");
            }

            var segment = DbContext.Add(new Segment
            {
                Name = dto.Name,
                Key = dto.Key,
                Avatar = dto.Avatar
            }).Entity;

            var team = DbContext.Add(new Team
            {
                Segment = segment,
                Name = "Unassigned",
                Key = null                
            }).Entity;

            DbContext.Add(new TeamMember
            {
                Team = team,
                ProfileId = profileId
            });
        }

        public void Update(int segmentId, SegmentCreateDTO dto)
        {
            var segment = DbContext.Segments.FirstOrDefault(x => x.Id == segmentId);

            segment.EnsureNotNull(segmentId);

            if (segment.Key != dto.Key)
            {
                if (!IsSegmentKeyUnique(dto.Key))
                {
                    throw new ApplicationException($"A segment exists with the same key");
                }
            }

            segment.Key = dto.Key;
            segment.Name = dto.Name;
            segment.Avatar = dto.Avatar;
        }

        public bool IsSegmentKeyUnique(string segmentKey)
        {
            return IsSegmentKeyUnique(DbContext, segmentKey);
        }

        public static bool IsSegmentKeyUnique(OrganizationDbContext dbContext, string segmentKey)
        {
            return !dbContext.Segments.Any(x => x.Key == segmentKey);
        }

        public void Archive(int segmentId)
        {
            var segment = DbContext.Segments.FirstOrDefault(x => x.Id == segmentId);

            segment.EnsureNotNull(segment.Key);

            DbContext.Remove(segment);
        }

        #endregion
    }
}