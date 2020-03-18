using Firdaws.Core;

namespace Tayra.Services
{
    public interface IAdvisorService
    {
        AdvisorOverviewDTO GetActionPointOverview(int? segmentId);
        GridData<AdvisorSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams,int segmentId);
        void Conclude(AdvisorConcludeDTO dto);
    }
}