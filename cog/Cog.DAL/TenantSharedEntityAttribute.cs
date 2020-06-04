using System;
namespace Cog.DAL
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class TenantSharedEntityAttribute : Attribute
    {
    }
}
