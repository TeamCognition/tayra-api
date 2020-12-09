using System.ComponentModel;

namespace Tayra.Common
{
    public enum IntegrationType
    {
        None = 0,

        [Description("Atlassian Jira")]
        ATJ = 1,

        [Description("Github")]
        GH = 2,

        [Description("Slack")]
        SL = 3,
    }
}
