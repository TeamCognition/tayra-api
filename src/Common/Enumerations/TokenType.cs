using System.ComponentModel;

namespace Tayra.Common
{
    public enum TokenType
    {
        [Description("Custom")]
        Custom = 0,

        [Description("Company Token")]
        CompanyToken = 1,

        [Description("Experience")]
        Experience = 2,

        OneUp = 999
    }
}
