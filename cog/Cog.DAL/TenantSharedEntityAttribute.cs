using System;

namespace Cog.DAL
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class TenantSharedEntityAttribute : Attribute
    {
    }
}