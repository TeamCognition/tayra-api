using System;

namespace Cog.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AuditTypeAttribute : Attribute
    {
        public AuditType AuditType
        {
            get;
            private set;
        }

        public AuditTypeAttribute(AuditType auditType)
        {
            AuditType = auditType;
        }
    }
}
