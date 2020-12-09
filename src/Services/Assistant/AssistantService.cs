using System;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class AssistantService : BaseService<OrganizationDbContext>, IAssistantService
    {
        #region Constructor

        public AssistantService(OrganizationDbContext dbContext) : base(dbContext)
        {

        }

        #endregion

        #region Public Methods

        public AssistantOverviewDTO GetActionPointOverview(Guid? segmentId)
        {
            IQueryable<ActionPoint> scope = DbContext.ActionPoints.Where(x => x.ConcludedOn == null && x.SegmentId.HasValue);

            if (segmentId.HasValue)
            {
                scope = scope.Where(x => x.SegmentId == segmentId);
            }

            return new AssistantOverviewDTO
            {
                ActionPoints = (from s in scope
                                where s.ConcludedOn == null
                                group s by s.SegmentId into g
                                select new AssistantOverviewDTO.ActionPointDTO
                                {
                                    SegmentId = g.Key,
                                    Types = g.Select(x => x.Type).Distinct().ToArray()
                                }).ToArray()
            };
        }

        public GridData<AssistantMemberGridDTO> GetMemberActionPointGrid(GridParams gridParams, Guid profileId)
        {
            var q = from ap in DbContext.ActionPoints
                    where ap.ProfileId == profileId
                    where ap.ConcludedOn == null
                    select new AssistantMemberGridDTO
                    {
                        ActionPointId = ap.Id,
                        Type = ap.Type,
                        Created = ap.Created
                    };

            GridData<AssistantMemberGridDTO> gridData = q.GetGridData(gridParams);

            return gridData;
        }

        public GridData<AssistantSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams, Guid segmentId)
        {
            var q = from ap in DbContext.ActionPoints
                    where ap.SegmentId == segmentId
                    where ap.ConcludedOn == null
                    group ap by ap.Type into g
                    select new AssistantSegmentGridDTO
                    {
                        Type = g.Key,
                        ImpactedMembers = g.Select(x => new AssistantSegmentGridDTO.ProfileDTO
                        {
                            ActionPointId = x.Id,
                            FullName = $"{x.Profile.FirstName} {x.Profile.LastName}",
                            Username = x.Profile.Username,
                            Avatar = x.Profile.Avatar,
                            Created = x.Created
                        }).ToArray()
                    };

            GridData<AssistantSegmentGridDTO> gridData = q.GetGridData(gridParams);

            return gridData;
        }

        public void ConcludeActionPoints(Guid segmentId, Guid[] apIds, ActionPointTypes? apType)
        {
            IQueryable<ActionPoint> scope = DbContext.ActionPoints.Where(x => x.SegmentId == segmentId);

            if (apIds.Length > 0)
            {
                scope = scope.Where(x => apIds.Contains(x.Id));
            }
            else if (apType.HasValue)
            {
                scope = scope.Where(x => x.Type == apType);
            }
            else
            {
                throw new ApplicationException($"provide either {nameof(apIds)} or {nameof(apType)}");
            }

            var now = DateTime.UtcNow;
            foreach (var ap in scope)
            {
                ap.ConcludedOn = now;
            }
        }

        #endregion
    }
}
