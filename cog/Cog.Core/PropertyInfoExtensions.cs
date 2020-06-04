using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cog.Core
{
    public static class PropertyInfoExtensions
    {
        private static Type[] _basicTypes;

        static PropertyInfoExtensions()
        {
            _basicTypes = new Type[6]
            {
            typeof(string),
            typeof(Enum),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan)
            };
        }

        /// <summary>
        /// Checks if property type is derived from IEnumerable.
        /// </summary>
        /// <param name="pi">PropertyInfo instance.</param>
        /// <returns>True if property is IEnumerable.</returns>
        public static bool IsIEnumerable(this PropertyInfo pi)
        {
            return typeof(IEnumerable).IsAssignableFrom(pi.PropertyType);
        }

        /// <summary>
        /// Checks if property type is derived from IEnumerable of T.
        /// </summary>
        /// <typeparam name="T">Type of collection element.</typeparam>
        /// <param name="pi">PropertyInfo instance.</param>
        /// <returns>True if property is IEnumerable of T.</returns>
        public static bool IsIEnumerable<T>(this PropertyInfo pi)
        {
            return typeof(IEnumerable<T>).IsAssignableFrom(pi.PropertyType);
        }
    }

}
