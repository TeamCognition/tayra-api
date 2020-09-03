using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class ErrorsMetric : SegmentMetric
    {
        public ErrorsMetric(IEnumerable<Task> tasks, int dateId, int segmentId) : base(MetricType.Errors, dateId, segmentId)
        {
            Value = tasks.Where(x => x.IsProductionBugCausing).Sum(x => x.BugSeverity * x.BugPopulationAffect) ?? 0f;
        }

        public static ErrorsMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => new ErrorsMetric(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}