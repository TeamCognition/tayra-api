namespace Tayra.Models.Organizations
{
    public interface IProfileMetric<T> where T : Metric
    {
        int ProfileId { get; }
        int? SegmentId { get; }
    }
}