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

        public AdvisorOverviewDTO GetActionPointOverview()
        {
            return new AdvisorOverviewDTO
            {
                ActionPoints = (from asd in DbContext.ActionPoints
                                where asd.ConcludedOn == null && asd.SegmentId.HasValue
                                group asd by asd.SegmentId.Value into g
                                select new AdvisorOverviewDTO.ActionPointDTO
                                {
                                    SegmentId = g.Key,
                                    Types = g.Select(x => x.Type).ToArray()
                                }).ToArray()
            };
        }

        public AdvisorSingleSegmentDTO GetSegmentView(int segmentId)
        {
            return new AdvisorSingleSegmentDTO
            {
                ActionPoints = DbContext.ActionPoints.Where(x => x.SegmentId == segmentId && x.ConcludedOn == null)
                    .Select(x => new AdvisorSingleSegmentDTO.ActionPointDTO
                    {
                        Id = x.Id,
                        Type = x.Type
                    }).ToArray()
            };
        }

        public GridData<AdvisorSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams, int segmentId)
        {
            var query = from asd in DbContext.ActionPoints
                        where asd.SegmentId == segmentId
                        where asd.ConcludedOn == null
                        select new AdvisorSegmentGridDTO
                        {
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
