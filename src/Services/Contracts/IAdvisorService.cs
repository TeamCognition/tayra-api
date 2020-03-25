using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IAdvisorService
    {
        AdvisorOverviewDTO GetActionPointOverview(int? segmentId);
        GridData<AdvisorMemberGridDTO> GetMemberActionPointGrid(GridParams gridParams, int profileId);
        GridData<AdvisorSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams,int segmentId);
        void ConcludeActionPoints(int segmentId, int? apId, ActionPointTypes? apType);
    }
}