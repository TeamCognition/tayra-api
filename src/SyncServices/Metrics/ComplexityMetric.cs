using Tayra.Common;

namespace Tayra.SyncServices.Metrics
{
    public class ComplexityMetric : Metric
    {
        public ComplexityMetric(float value) : base(MetricTypes.Complexity)
        {
            this.Value = value;
        }
    }
}