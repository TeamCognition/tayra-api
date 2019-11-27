using System;
namespace Tayra.Models.Organizations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class TenantSharedEntityAttribute : Attribute
    {
    }
}
