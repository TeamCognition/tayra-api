using System;
using Cog.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IAssistantService
    {
        AssistantOverviewDTO GetActionPointOverview(Guid? segmentId);
        GridData<AssistantMemberGridDTO> GetMemberActionPointGrid(GridParams gridParams, Guid profileId);
        GridData<AssistantSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams, Guid segmentId);
        void ConcludeActionPoints(Guid segmentId, Guid[] apId, ActionPointTypes? apType);
    }
}