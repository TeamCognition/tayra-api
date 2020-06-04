using System;
namespace Cog.Core
{
    /// <summary>
    /// Defines scope of audit
    /// </summary>
    public enum AuditType : byte
    {
        /// <summary>
        /// Logs user and time stamps only.
        /// </summary>
        Basic,
        /// <summary>
        /// Logs user and time stamps, and names of the changed properties.
        /// </summary>
        Limited,
        /// <summary>
        /// Logs user and time stampes, original and current values of properties with names.
        /// </summary>
        Full
    }
}
