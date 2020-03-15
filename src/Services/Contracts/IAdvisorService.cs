using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IAdvisorService
    {
        AdvisorOverviewDTO GetActionPointOverview();
        AdvisorSegmentViewDTO GetSegmentView(int segmentId);
        GridData<AdvisorSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams,int segmentId);
        void Conclude(AdvisorConcludeActionPointDTO dto);
    }
}