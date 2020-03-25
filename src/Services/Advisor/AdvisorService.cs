using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class AdvisorService : BaseService<OrganizationDbContext>, IAdvisorService
    {
        #region Constructor

        public AdvisorService(OrganizationDbContext dbContext) : base(dbContext)
        {
           
        }

        #endregion

        #region Public Methods

        public AdvisorOverviewDTO GetActionPointOverview(int? segmentId)
        {
            IQueryable<ActionPoint> scope = DbContext.ActionPoints.Where(x => x.ConcludedOn == null && x.SegmentId.HasValue);

            if(segmentId.HasValue)
            {
                scope = scope.Where(x => x.SegmentId == segmentId);
            }

            return new AdvisorOverviewDTO
            {
                ActionPoints = (from s in scope
                                group s by s.SegmentId.Value into g
                                select new AdvisorOverviewDTO.ActionPointDTO
                                {
                                    SegmentId = g.Key,
                                    Count = g.Select(x => x.Type).Distinct().Count(),
                                    Types = g.Select(x => x.Type).ToArray()
                                }).ToArray()
            };
        }

        public GridData<AdvisorMemberGridDTO> GetMemberActionPointGrid(GridParams gridParams, int profileId)
        {
            var q = from ap in DbContext.ActionPoints
                    where ap.ProfileId == profileId
                    where ap.ConcludedOn == null
                    select new AdvisorMemberGridDTO
                    {
                        ActionPointId = ap.Id,
                        Type = ap.Type,
                        Created = ap.Created
                    };

            GridData<AdvisorMemberGridDTO> gridData = q.GetGridData(gridParams);

            return gridData;
        }

        public GridData<AdvisorSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams, int segmentId)
        {
            var q =  from ap in DbContext.ActionPoints
                     where ap.SegmentId == segmentId
                     where ap.ConcludedOn == null
                     group ap by ap.Type into g
                     select new AdvisorSegmentGridDTO
                     {
                        Type = g.Key,
                        ImpactedMembers = g.Select(x => new AdvisorSegmentGridDTO.ProfileDTO
                        {  
                            ActionPointId = x.Id,
                            Username = x.Profile.Username,
                            FullName = $"{x.Profile.FirstName} {x.Profile.LastName}",
                            Created = x.Created
                        }).ToArray()                     
                     };
                    
            GridData<AdvisorSegmentGridDTO> gridData = q.GetGridData(gridParams);

            return gridData;
        }

        public void ConcludeActionPoints(int segmentId, int? apId, ActionPointTypes? apType)
        {
            IQueryable<ActionPoint> scope = DbContext.ActionPoints.Where(x => x.SegmentId == segmentId);

            if(apId.HasValue)
            {
                scope = scope.Where(x => x.Id == apId);
            }
            else if(apType.HasValue)
            {
                scope = scope.Where(x => x.Type == apType);
            }
            else
            {
                throw new ApplicationException($"provide either {nameof(apId)} or {nameof(apType)}");
            }

            var now = DateTime.UtcNow;
            foreach(var ap in scope)
            {
                ap.ConcludedOn = now;
            }  
        }

        #endregion
    }
}
