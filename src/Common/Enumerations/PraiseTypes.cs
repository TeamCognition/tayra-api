using System.ComponentModel;

namespace Tayra.Common
{
    public enum PraiseTypes
    {
        [Description("Hard Worker")] 
        HardWorker = 1,
        
        [Description("Team Player")]
        TeamPlayer = 2,
        
        [Description("Helper")]
        Helper = 3
    }
}
