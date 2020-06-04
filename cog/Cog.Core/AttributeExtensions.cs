using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cog.Core
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// Checks whether specified class type is decorated with attribute of type T.
        /// </summary>
        /// <typeparam name="T">Type of attribute.</typeparam>
        /// <param name="element">Class type.</param>
        /// <returns>True if attribute was found.</returns>
        public static bool HasAttribute<T>(this MemberInfo element) where T : Attribute
        {
            return element.GetAttribute<T>() != null;
        }

        public static T GetAttribute<T>(this MemberInfo element) where T : Attribute
        {
            return CustomAttributeExtensions.GetCustomAttribute<T>(element, inherit: true);
        }
    }
}