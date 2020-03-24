using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Tayra.Common;
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

        public GridData<SegmentGridDTO> GetGridData(int[] segmentIds, SegmentGridParams gridParams)
        {
            var query = from s in DbContext.Segments
                        where segmentIds.Contains(s.Id)
                        select new SegmentGridDTO
                        {
                            SegmentId = s.Id,
                            Name = s.Name,
                            Key = s.Key,
                            Avatar = s.Avatar,
                            Created = s.Created,
                            ChallengesActive = s.Challenges.Count(x => x.Status == ChallengeStatuses.Active),
                            ChallengesCompleted = s.Challenges.Count(x => x.Status == ChallengeStatuses.Ended),
                            ShopItemsBought = s.ShopPurchases.Count(x => x.Status == ShopPurchaseStatuses.Fulfilled),
                            Integrations = s.Integrations.Where(x => x.ProfileId == null).Select(x => x.Type).ToArray(),
                            ActionPointsCount = s.ActionPoints.Count(x => x.ConcludedOn == null)
                        };

            GridData<SegmentGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<SegmentMemberGridDTO> GetSegmentMembersGridData(string segmentKey, SegmentMemberGridParams gridParams)
        {
            var segment = DbContext.Segments.Where(s => s.Key == segmentKey).FirstOrDefault();

            segment.EnsureNotNull(segmentKey);

            IQueryable<SegmentMemberGridDTO> query = from s in DbContext.ProfileAssignments.Where(x => x.SegmentId == segment.Id)
                                                   select new SegmentMemberGridDTO
                                                   {
                                                       Name = s.Profile.FirstName + " " + s.Profile.LastName,
                                                       Username = s.Profile.Username,
                                                       Avatar = s.Profile.Avatar,
                                                       MemberFrom = s.Created
                                                   };

            GridData<SegmentMemberGridDTO> gridData = query.GetGridData(gridParams);

            gridData.Records = gridData.Records.DistinctBy(x => x.Username).ToList();

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
                                                        AvatarColor = t.AvatarColor,
                                                        MembersCount = t.Members.Count(),
                                                        Created = t.Created
                                                    };

            GridData<SegmentTeamGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public SegmentViewDTO GetSegmnetViewDTO(string segmentKey)
        {
            var segmentDTO =  (from s in DbContext.Segments
                                where s.Key == segmentKey
                                select new SegmentViewDTO
                                {
                                    SegmentId = s.Id,
                                    Name = s.Name,
                                    Key = s.Key,
                                    Avatar = s.Avatar,
                                    TokensEarned = Math.Round(s.ReportsDaily.OrderByDescending(x => x.DateId).Select(x => x.CompanyTokensEarnedTotal).FirstOrDefault(), 2),
                                    TokensSpent = Math.Round(s.ReportsDaily.OrderByDescending(x => x.DateId).Select(x => x.CompanyTokensSpentTotal).FirstOrDefault(), 2),
                                    ChallengesActive = s.Challenges.Count(x => x.Status == ChallengeStatuses.Active),
                                    ChallengesCompleted = s.Challenges.Count(x => x.Status == ChallengeStatuses.Ended),
                                    ShopItemsBought = s.ShopPurchases.Count(x => x.Status == ShopPurchaseStatuses.Fulfilled),
                                }).FirstOrDefault();

            segmentDTO.EnsureNotNull(segmentKey);

            return segmentDTO;
        }

        public SegmentImpactPieChartDTO GetImpactPieChart(int segmentId)
        {
            var lastDateId = DbContext.TeamReportsWeekly.OrderByDescending(x => x.DateId).Select(x => x.DateId).FirstOrDefault();

            if (lastDateId == 0)
                return null;

            var wr = (from trw in DbContext.TeamReportsWeekly
                      where trw.SegmentId == segmentId
                      && trw.DateId == lastDateId
                      select new
                      {
                          TeamName = trw.Team.Name,
                          OImpact = trw.OImpactAverage
                      }).ToList();

            if (wr == null || !wr.Any())
            {
                return null;
            }

            var impactSum = wr.Sum(x => x.OImpact);

            return new SegmentImpactPieChartDTO
            {
                Teams = wr.Select(x => new SegmentImpactPieChartDTO.TeamDTO
                {
                    Name = x.TeamName,
                    ImpactPercentage = x.OImpact / impactSum * 100
                }).ToArray()
            };
        }

        public SegmentImpactLineChartDTO GetImpactLineChart(int segmentId)
        {
            var wr = (from trw in DbContext.SegmentReportsWeekly
                      where trw.SegmentId == segmentId
                      orderby trw.DateId descending
                      select new
                      {
                          DateId = trw.DateId,
                          OImpact = trw.OImpactAverage
                      }).Take(30).ToList();

            if(wr == null || !wr.Any())
            {
                return null;
            }

            return new SegmentImpactLineChartDTO
            {
                StartDateId = wr.Last().DateId,
                EndDateId = wr.First().DateId,
                Averages = wr.Select(x => x.OImpact).Reverse().ToArray()
            };
        }

        public void AddMember(SegmentMemberAddRemoveDTO dto)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Id == dto.ProfileId);
            profile.EnsureNotNull(dto.ProfileId);

            if(!SegmentRules.CanAddProfileToSegment(profile.Role, dto.TeamId))
            {
                throw new ApplicationException("If you are adding a member you must provide a teamId");
            }

            int? segmentId, teamId;
            if (dto.TeamId.HasValue)
            {
                var team = DbContext.Teams.FirstOrDefault(x => x.Id == dto.TeamId);
                team.EnsureNotNull(dto.TeamId);
                teamId = team.Id;
                segmentId = team.SegmentId;
            }
            else if (dto.SegmentId.HasValue)
            {
                if (profile.Role != ProfileRoles.Manager)
                    throw new ApplicationException("only managers can be added to segment without teams");

                var segment = DbContext.Segments.FirstOrDefault(x => x.Id == dto.SegmentId);
                segment.EnsureNotNull(dto.SegmentId);
                segmentId = segment.Id;
            }
            else
            {
                throw new ApplicationException("you have to provide either segmentId or teamId");
            }

            DbContext.Add(new ProfileAssignment
            {
                ProfileId = dto.ProfileId,
                SegmentId = segmentId.Value,
                TeamId = dto.TeamId
            });
        }

        public void RemoveMember(SegmentMemberAddRemoveDTO dto)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Id == dto.ProfileId);
            profile.EnsureNotNull(dto.ProfileId);

            if (!SegmentRules.CanRemoveProfileToSegment(profile.Role, dto.TeamId))
            {
                throw new ApplicationException("If you are removing a member you must provide a teamId");
            }

            if (dto.TeamId.HasValue)
            {
                var team = DbContext.Teams.FirstOrDefault(x => x.Id == dto.TeamId);
                team.EnsureNotNull(dto.TeamId);
            }
            else if (dto.SegmentId.HasValue)
            {
                if(profile.Role != ProfileRoles.Manager)
                    throw new ApplicationException("only managers can be in segment without a team");

                var segment = DbContext.Segments.FirstOrDefault(x => x.Id == dto.SegmentId);
                segment.EnsureNotNull(dto.SegmentId);
            }
            else
            {
                throw new ApplicationException("you have to provide either segmentId or teamId");
            }

            DbContext.Remove(DbContext.ProfileAssignments.FirstOrDefault(x => x.ProfileId == dto.ProfileId && x.TeamId == dto.TeamId));
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

            DbContext.Add(new ProfileAssignment
            {
                ProfileId = profileId,
                SegmentId = segment.Id,
                Team = team,
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
            var segment = DbContext.Segments.Include(x => x.Teams).FirstOrDefault(x => x.Id == segmentId);

            segment.EnsureNotNull(segmentId);

            DbContext.Remove(segment);

            foreach(var t in segment.Teams) //is this needed?
            {
                DbContext.Remove(t);
            }
        }

        #endregion
    }
}