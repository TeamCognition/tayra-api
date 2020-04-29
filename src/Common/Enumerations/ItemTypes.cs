using System.ComponentModel;

namespace Tayra.Common
{
    public enum ItemTypes
    {
        #region Tayra Items

        [Description("Title")]
        TayraTitle = 101,

        [Description("Badge")]
        TayraBadge = 102,

        [Description("Border")]
        TayraBorder = 103,

        #endregion

        #region Digital Items

        [Description("Digital")]
        Digital = 201,

        #endregion

        #region Physical Items

        [Description("Physical Good")]
        PhysicalGood = 301,

        #endregion
    }
}
