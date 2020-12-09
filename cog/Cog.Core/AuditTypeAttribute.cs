using System;

namespace Cog.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuditTypeAttribute : Attribute
    {
        public AuditTypeAttribute(AuditType auditType)
        {
            AuditType = auditType;
        }

        public AuditType AuditType { get; }
    }
}