using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IAdvisorService
    {
        AdvisorOverviewDTO GetActionPointOverview(int segmentId);
        GridData<AdvisorSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams,int segmentId);
        void Conclude(AdvisorConcludeDTO dto);
    }
}