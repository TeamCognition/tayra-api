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

        public AdvisorOverviewDTO GetActionPointOverview(int segmentId)
        {

            IQueryable<ActionPoint> scope = DbContext.ActionPoints.Where(x => x.ConcludedOn == null && x.SegmentId.HasValue);

            if (segmentId == 0)
            {
                return new AdvisorOverviewDTO
                {
                    ActionPoints = (from s in scope
                                    group s by s.SegmentId.Value into g
                                    select new AdvisorOverviewDTO.ActionPointDTO
                                    {
                                        SegmentId = g.Key,
                                        Types = g.Select(x => x.Type).ToArray()
                                    }).ToArray()
                };

            }
            else
            {
                return new AdvisorOverviewDTO
                {
                    ActionPoints = new AdvisorOverviewDTO.ActionPointDTO[]
                    {
                        new AdvisorOverviewDTO.ActionPointDTO
                        {
                            Types = scope.Where(x => x.SegmentId == segmentId).Select(x => x.Type).ToArray()
                        }
                    }
                };
            }
        }

        public GridData<AdvisorSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams, int segmentId)
        {
            var query = from asd in DbContext.ActionPoints
                        where asd.SegmentId == segmentId
                        where asd.ConcludedOn == null
                        select new AdvisorSegmentGridDTO
                        {
                            ActionPointId = asd.Id,
                            Type = asd.Type,
                            Created = asd.Created
                        };


            GridData<AdvisorSegmentGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        public void Conclude(AdvisorConcludeDTO dto)
        {
            var actionPoint = DbContext.ActionPoints.FirstOrDefault(x => x.Id == dto.ActionPointId);

            actionPoint.EnsureNotNull(dto.ActionPointId);

            actionPoint.ConcludedOn = DateTime.UtcNow;
        }

        #endregion
    }
}
