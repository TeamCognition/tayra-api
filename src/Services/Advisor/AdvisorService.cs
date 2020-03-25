using Firdaws.Core;
using Firdaws.DAL;
using MoreLinq;
using System;
using System.Linq;
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
                                where s.ConcludedOn == null
                                group s by s.SegmentId into g
                                select new AdvisorOverviewDTO.ActionPointDTO
                                {
                                    SegmentId = g.Key,
                                    Count = g.Select(x => x.Type).Distinct().Count(),
                                    Types = g.Select(x => x.Type).Distinct().ToArray()
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
                        Created = g.Select(x => x.Created).FirstOrDefault(),
                        ImpactedMembers = g.Select(x => new AdvisorSegmentGridDTO.ProfileDTO
                        {  
                            ProfileId = x.ProfileId,
                            Name = x.Profile.FirstName + ' ' + x.Profile.LastName,
                            Username = x.Profile.Username
                        }).ToArray()                     
                     };
                    
            GridData<AdvisorSegmentGridDTO> gridData = q.GetGridData(gridParams);

            return gridData;
        }

        public void ConcludeActionPoints(AdvisorConcludeDTO dto)
        {
            IQueryable<ActionPoint> scope = DbContext.ActionPoints.Where(x=> x.SegmentId == dto.SegmentId && x.Type == dto.Type);

            foreach(var memberAp in scope)
            {
                if (dto.Members.Contains(memberAp.ProfileId))
                {
                    memberAp.ConcludedOn = DateTime.UtcNow;
                }
            }  
        }

        #endregion
    }
}
