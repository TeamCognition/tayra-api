using Firdaws.Core;
using Firdaws.DAL;
using MoreLinq;
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

        public GridData<ActionPointGridDTO> GetActionPointGrid(GridParams gridParams)
        {

            var query = from s in DbContext.Segments
                        select new ActionPointGridDTO
                        {
                            SegmentKey = s.Key,
                            TotalActionPoints = DbContext.ActionPointSegments.Where(x => x.SegmentId == s.Id).Sum(x => x.ActionPointId),
                            HighImpact = DbContext.ActionPointSegments.Where(x => x.SegmentId == s.Id).Sum(x => x.ActionPointId),
                            MediumImpact = DbContext.ActionPointSegments.Where(x => x.SegmentId == s.Id).Sum(x => x.ActionPointId),
                            LowImpact = DbContext.ActionPointSegments.Where(x => x.SegmentId == s.Id).Sum(x => x.ActionPointId)
                        };

            GridData<ActionPointGridDTO> gridData = query.GetGridData(gridParams);

            gridData.Records = gridData.Records.DistinctBy(x => x.SegmentKey).ToList();

            return gridData;

        }


        #endregion
    }
}
