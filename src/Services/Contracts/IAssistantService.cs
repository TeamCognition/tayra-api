﻿using Firdaws.Core;
using Tayra.Common;

namespace Tayra.Services
{
    public interface IAssistantService
    {
        AssistantOverviewDTO GetActionPointOverview(int? segmentId);
        GridData<AssistantMemberGridDTO> GetMemberActionPointGrid(GridParams gridParams, int profileId);
        GridData<AssistantSegmentGridDTO> GetSegmentActionPointGrid(GridParams gridParams,int segmentId);
        void ConcludeActionPoints(int segmentId, int[] apId, ActionPointTypes? apType);
    }
}