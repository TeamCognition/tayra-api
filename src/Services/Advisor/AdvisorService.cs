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

            var apd = (from asd in DbContext.ActionPointSegments
                         where asd.ConcludedOn == null
                         select new AdvisorOverviewDTO.ActionPointsDTO
                         {
                             SegmentId = asd.SegmentId,
                             ActionPoints = DbContext.ActionPointSegments.Where(x => x.ConcludedOn == null && x.SegmentId == asd.SegmentId).Select(x => x.ActionPointId).ToArray()
                         }).DistinctBy(x => x.SegmentId).ToArray();

            return new AdvisorOverviewDTO
            {
                ActionPoints = apd
            };

        }

        public AdvisorSegmentViewDTO GetSegmentView(int segmentId)
        {

            var apd = (from asd in DbContext.ActionPointSegments
                         where asd.SegmentId == segmentId
                         where asd.ConcludedOn == null
                         select new AdvisorSegmentViewDTO.ActionPointsDTO
                         {
                             SegmentId = asd.SegmentId,
                             ActionPoints = DbContext.ActionPointSegments.Where(x => x.ConcludedOn == null && x.SegmentId == asd.SegmentId).Select(x => x.ActionPointId).ToArray()
                         }).DistinctBy(x => x.SegmentId).FirstOrDefault();

            return new AdvisorSegmentViewDTO
            {
                ActionPoints = apd
            };

        }

        public GridData<AdvisorSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams,int segmentId)
        {

            var actionPoints = DbContext.ActionPointSegments.Where(x => x.SegmentId == segmentId).Select(x => x.ActionPointId).ToArray();

            var query = (from asd in DbContext.ActionPointSegments
                         where asd.SegmentId == segmentId
                         where asd.ConcludedOn == null
                         select new AdvisorSegmentGridDTO
                         {
                             ActionPoint = asd.ActionPointId,
                             Date = asd.Created
                         });


            GridData<AdvisorSegmentGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;

        }

        public void Conclude(AdvisorConcludeActionPointDTO dto)
        {
            var actionPoint = DbContext.ActionPointSegments.FirstOrDefault(x => x.SegmentId == dto.SegmentId && x.ActionPointId == dto.ActionPointId);

            actionPoint.EnsureNotNull(dto.ActionPointId);

            actionPoint.ConcludedOn = DateTime.UtcNow;
        }

        #endregion
    }
}
