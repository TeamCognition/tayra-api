using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class TimeWorkedLoggedMetric : PureMetricWithSegment
    {
        private TimeWorkedLoggedMetric(float value, int dateId, int segmentId) : base(MetricType.TimeWorkedLogged, value, dateId, segmentId )
        {
            
        }
        public static TimeWorkedLoggedMetric Create(IEnumerable<Task> tasks, int dateId, int segmentId) => new TimeWorkedLoggedMetric(tasks.Sum(x => x.TimeSpentInMinutes) ?? 0f, dateId, segmentId);

        public static TimeWorkedLoggedMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => TimeWorkedLoggedMetric.Create(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}