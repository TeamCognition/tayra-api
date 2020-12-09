using System.ComponentModel;

namespace Tayra.Common
{
    public enum QuestStatuses
    {
        [Description("Draft")]
        Draft = 0,

        [Description("Active")]
        Active = 1,

        [Description("Ended")]
        Ended = 2,

        [Description("Archived")]
        Archived = 3,
    }
}