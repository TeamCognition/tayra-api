using System.ComponentModel;

namespace Tayra.Common
{
    public enum MetricTypes
    {
        [Description("Impact")]
        OImpact = 1,

        [Description("Speed")]
        Speed = 2,

        [Description("Heat")]
        Heat = 3,

        [Description("Complexity")]
        Complexity = 4,

        [Description("Assists")]
        Assist = 5,

        [Description("Completion")]
        TaskCompletion = 6
    }
}
