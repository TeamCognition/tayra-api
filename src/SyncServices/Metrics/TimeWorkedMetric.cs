using System.Collections.Generic;
using System.Linq;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.SyncServices.Metrics
{
    public class TimeWorkedMetric : PureMetricWithSegment
    {
        private TimeWorkedMetric(float value, int dateId, int segmentId) : base(MetricType.TimeWorked, value, dateId, segmentId )
        {
            
        }
        public static TimeWorkedMetric Create(IEnumerable<Task> tasks, int dateId, int segmentId) => new TimeWorkedMetric(tasks.Sum(x => x.TimeSpentInMinutes ?? x.AutoTimeSpentInMinutes) ?? 0f, dateId, segmentId);
        
        public static TimeWorkedMetric[] CreateForEverySegment(IEnumerable<Task> tasks, int dateId)
        {
            return tasks
                .Where(x => x.SegmentId.HasValue)
                .GroupBy(x => x.SegmentId)
                .Select(s => TimeWorkedMetric.Create(s.AsEnumerable(), dateId, s.Key.Value))
                .ToArray();
        }
    }
}