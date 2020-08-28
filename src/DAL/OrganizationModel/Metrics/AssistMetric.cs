using System.Collections.Generic;
using System.Linq;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public class AssistMetric : Metric
    {
        public AssistMetric(PraisesReceivedMetric praisesReceivedMetric): base(MetricTypes.Assist, praisesReceivedMetric.DateId)
        {
            Value = praisesReceivedMetric.Value;
        }
    }
}