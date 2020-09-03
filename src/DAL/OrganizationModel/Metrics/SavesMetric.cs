using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class SavesMetric : SegmentMetric
    {
        public SavesMetric(IEnumerable<Task> tasks, int dateId, int segmentId): base(MetricType.Saves, dateId, segmentId)
        {
            Value = tasks.Count(x => x.IsProductionBugFixing && x.BugSeverity > 3);
        }
        
        public static SavesMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => new SavesMetric(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}