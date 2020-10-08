using System;
using System.Collections.Generic;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Tayra.Analytics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services.Analytics;


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
                            QuestsActive = s.Quests.Count(x => x.Status == QuestStatuses.Active),
                            QuestsCompleted = s.Quests.Count(x => x.Status == QuestStatuses.Ended),
                            ShopItemsBought = s.ShopPurchases.Count(x => x.Status == ShopPurchaseStatuses.Fulfilled),
                            Integrations = s.Integrations.Where(x => x.ProfileId == null).Select(x => x.Type).ToArray(),
                            ActionPointsCount = s.ActionPoints.Where(x => x.ConcludedOn == null).Select(x => x.Type).Distinct().Count()
                        };

            GridData<SegmentGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public GridData<SegmentMemberGridDTO> GetSegmentMembersGridData(string segmentKey, SegmentMemberGridParams gridParams)
        {
            var segment = DbContext.Segments.FirstOrDefault(s => s.Key == segmentKey);

            segment.EnsureNotNull(segmentKey);

            var scope = DbContext.ProfileAssignments.Where(x => x.SegmentId == segment.Id);

            if (gridParams.AnalyticsEnabledOnly.HasValue)
            {
                scope = scope.Where(x => x.Profile.IsAnalyticsEnabled);
            }
            
            IQueryable<SegmentMemberGridDTO> query = from s in scope
                                                     select new SegmentMemberGridDTO
                                                     {
                                                         ProfileId = s.Profile.Id,
                                                         Name = s.Profile.FirstName + " " + s.Profile.LastName,
                                                         Username = s.Profile.Username,
                                                         Role = s.Profile.Role,
                                                         Avatar = s.Profile.Avatar,
                                                         MemberFrom = s.Created
                                                     };

            GridData<SegmentMemberGridDTO> gridData = query.GetGridData(gridParams);

            gridData.Records = MoreEnumerable.DistinctBy(gridData.Records, x => x.Username).ToList();

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
            var segmentDTO = (from s in DbContext.Segments
                where s.Key == segmentKey
                select new SegmentViewDTO
                {
                    SegmentId = s.Id,
                    Name = s.Name,
                    Key = s.Key,
                    Avatar = s.Avatar,
                    AssistantSummary = s.AssistantSummary,
                    TokensEarned = Math.Round(s.ReportsDaily.OrderByDescending(x => x.DateId).Select(x => x.CompanyTokensEarnedTotal).FirstOrDefault(), 2),
                    TokensSpent = Math.Round(s.ReportsDaily.OrderByDescending(x => x.DateId).Select(x => x.CompanyTokensSpentTotal).FirstOrDefault(), 2),
                    QuestsActive = s.Quests.Count(x => x.Status == QuestStatuses.Active),
                    QuestsCompleted = s.Quests.Count(x => x.Status == QuestStatuses.Ended),
                    ShopItemsBought = s.ShopPurchases.Count(x => x.Status == ShopPurchaseStatuses.Fulfilled),
                }).FirstOrDefault();

            segmentDTO.EnsureNotNull(segmentKey);

            return segmentDTO;
        }
        
        public SegmentRawScoreDTO GetSegmentRawScore(string segmentKey)
        {
            var segment = DbContext.Segments.FirstOrDefault(x => x.Key == segmentKey);
            segment.EnsureNotNull(segmentKey);

            return (from r in DbContext.SegmentReportsDaily
                    where r.SegmentId == segment.Id
                    select new SegmentRawScoreDTO
                    {
                        TasksCompleted = r.TasksCompletedTotal,
                        AssistsGained = r.AssistsTotal,
                        TimeWorked = r.TasksCompletionTimeTotal,
                        TokensEarned = r.CompanyTokensEarnedTotal,
                        TokensSpent = r.CompanyTokensSpentTotal,
                        ItemsBought = r.ItemsBoughtTotal,
                        QuestsCompleted = 0,
                        DaysOnTayra = EF.Functions.DateDiffDay(segment.Created, DateTime.UtcNow)
                    }).LastOrDefault();
        }

        public Dictionary<int,AnalyticsMetricWithIterationSplitDto> GetSegmentAverageMetrics(string segmentKey)
        {
            var segment = DbContext.Segments.FirstOrDefault(x => x.Key == segmentKey);
            
            var analyticsService = new AnalyticsService(DbContext);

            var metricList = new MetricType[]
            {
                MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists,MetricType.Heat, 
                MetricType.TasksCompleted, MetricType.Complexity
            };
            
            return analyticsService.GetMetricsWithIterationSplit(
                metricList, segment.Id, EntityTypes.Segment,
                new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow));
        }

        public Dictionary<int, MetricsValueWEntity[]>  GetSegmentRankChart(string segmentKey)
        {
            var analyticsService = new AnalyticsService(DbContext);

            var metricList = new MetricType[]
            {
                MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists, MetricType.Heat,
                MetricType.TasksCompleted, MetricType.Complexity
            };

            var segmentProfiles = DbContext.ProfileAssignments.Where(x => x.Segment.Key == segmentKey && x.Profile.IsAnalyticsEnabled).Select(x => x.ProfileId)
                .Distinct().ToArray();
            
            return analyticsService.GetMetricsRanks(
                metricList, segmentProfiles, EntityTypes.Profile,
                new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow)
            );
        }

        public SegmentStatsDTO GetSegmentStats(int segmentId)
        {
            var analyticsService = new AnalyticsService(DbContext);

            var metricList = new MetricType[]
            {
                MetricType.Impact, MetricType.Speed, MetricType.Power, MetricType.Assists,
                MetricType.TasksCompleted, MetricType.Complexity, MetricType.CommitRate
            };

            var segmentMetrics = analyticsService.GetMetricsWithIterationSplit(
                metricList, segmentId, EntityTypes.Segment,
                new DatePeriod(DateTime.UtcNow.AddDays(-27), DateTime.UtcNow));

            return new SegmentStatsDTO()
            {
                LastRefreshAt = DbContext.ProfileMetrics.OrderByDescending(x => x.DateId).Select(x => x.Created).FirstOrDefault(),
                EntityMetrics = segmentMetrics,
                ComparatorMetrics = null
            };
        }

        public void AddMember(SegmentMemberAddRemoveDTO dto)
        {
            var profile = DbContext.Profiles.FirstOrDefault(x => x.Id == dto.ProfileId);
            profile.EnsureNotNull(dto.ProfileId);

            if (!SegmentRules.CanAddProfileToSegment(profile.Role, dto.TeamId))
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
                if (profile.Role != ProfileRoles.Manager)
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

        public void Create(int profileId, ProfileRoles role, SegmentCreateDTO dto)
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

            if (role != ProfileRoles.Admin)
            {
                DbContext.Add(new ProfileAssignment
                {
                    ProfileId = profileId,
                    SegmentId = segment.Id,
                    Team = team,
                });
            }
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

            foreach (var t in segment.Teams) //is this needed?
            {
                DbContext.Remove(t);
            }
        }

        #endregion
    }
}