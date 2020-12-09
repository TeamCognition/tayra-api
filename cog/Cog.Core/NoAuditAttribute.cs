using System;

namespace Cog.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NoAuditAttribute : Attribute
    {
    }
}