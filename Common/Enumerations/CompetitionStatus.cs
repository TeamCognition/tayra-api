namespace Tayra.Common
{
    public enum CompetitionStatus
    {
        Draft = 1, //not started
        Started = 2, //started
        Completed = 3, //completed on scheduled time
        Stopped = 4 //forcefully stopped after it started
    }
}
