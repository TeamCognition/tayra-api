using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class TimeWorkedMetric : MetricWithSegment
    {
        public TimeWorkedMetric(IEnumerable<Task> tasks, int dateId, int segmentId) : base(MetricType.TimeWorked, dateId, segmentId)
        {
            Value = tasks.Sum(x => x.TimeSpentInMinutes ?? x.AutoTimeSpentInMinutes) ?? 0f;
        }

        public static TimeWorkedMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => new TimeWorkedMetric(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}