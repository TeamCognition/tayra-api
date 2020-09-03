using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class TimeWorkedLoggedMetric : SegmentMetric
    {
        public TimeWorkedLoggedMetric(IEnumerable<Task> tasks, int dateId, int segmentId): base(MetricType.TimeWorkedLogged, dateId, segmentId)
        {
            Value = tasks.Sum(x => x.TimeSpentInMinutes) ?? 0f;
        }
        
        public static TimeWorkedLoggedMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => new TimeWorkedLoggedMetric(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}