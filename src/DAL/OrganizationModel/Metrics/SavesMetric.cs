using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class SavesMetric : Metric
    {
        public SavesMetric(float value): base(MetricTypes.Saves, value)
        {
        }
    }
}