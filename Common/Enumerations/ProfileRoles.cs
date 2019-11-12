using System;
using System.ComponentModel;

namespace Tayra.Common
{
    [Flags]
    public enum ProfileRoles
    {
        [Description("Admin")]
        Admin = 1,

        [Description("Manager")]
        Manager = 2,

        [Description("Member")]
        Member = 4
    }
}