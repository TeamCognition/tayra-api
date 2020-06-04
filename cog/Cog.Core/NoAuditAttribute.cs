using System;
namespace Cog.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NoAuditAttribute : Attribute
    {
    }
}
