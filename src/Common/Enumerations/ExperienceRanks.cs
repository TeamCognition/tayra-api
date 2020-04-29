using System.ComponentModel;

namespace Tayra.Common
{
    public enum ExperienceRanks
    {
        [Description("Bronze")]
        Bronze = 0,

        [Description("Silver")]
        Silver = 150,

        [Description("Gold")]
        Gold = 470,

        [Description("Platinum")]
        Platinum = 1450,

        [Description("Diamond")]
        Diamond = 4600,

        [Description("Master Tier")]
        MasterTier = 10000
    }
}
