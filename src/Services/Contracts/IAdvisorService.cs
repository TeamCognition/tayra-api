using Firdaws.Core;

namespace Tayra.Services
{
    public interface IAdvisorService
    {
        AdvisorOverviewDTO GetActionPointOverview(int? segmentId);
        GridData<AdvisorMemberGridDTO> GetMemberActionPointGrid(GridParams gridParams, int profileId);
        GridData<AdvisorSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams,int segmentId);
        void ConcludeSegmentActionPoints(AdvisorSegmentConcludeDTO dto);
    }
}