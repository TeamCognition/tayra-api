using Cog.Core;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface ISegmentsService
    {
        Segment Get(string segmentKey);
        GridData<SegmentGridDTO> GetGridData(int[] segmentIds, SegmentGridParams gridParams);
        GridData<SegmentMemberGridDTO> GetSegmentMembersGridData(string segmentKey, SegmentMemberGridParams gridParams);
        GridData<SegmentTeamGridDTO> GetSegmentTeamsGridData(string segmentKey, SegmentTeamGridParams gridParams);
        SegmentViewDTO GetSegmnetViewDTO(string segmentKey);
        SegmentAverageMetricsDTO GetSegmentAverageMetrics(string segmentKey);
        SegmentRankChartDTO GetSegmentRankChart(string segmentKey);
        void Create(int profileId, ProfileRoles role, SegmentCreateDTO dto);
        void Update(int segmentId, SegmentCreateDTO dto);
        void AddMember(SegmentMemberAddRemoveDTO dto);
        void RemoveMember(SegmentMemberAddRemoveDTO dto);
        bool IsSegmentKeyUnique(string segmentKey);
        void Archive(int segmentId);
    }
}